#!/usr/bin/env python3
"""Paginate MDPCZ public register via Livewire and optionally trigger export."""

from __future__ import annotations

import json
import re
import time
from pathlib import Path

import requests
from bs4 import BeautifulSoup

OUT = Path(__file__).resolve().parent / "raw" / "other"
OUT.mkdir(parents=True, exist_ok=True)

BASE = "https://www.mdpcz.co.zw"
URL = f"{BASE}/public_register"


def extract_livewire(html: str) -> dict:
    soup = BeautifulSoup(html, "html.parser")
    # Livewire v3: wire:snapshot on component root
    component = None
    for tag in soup.find_all(True):
        if tag.has_attr("wire:id") or tag.has_attr("wire:snapshot"):
            component = tag
            break
    data = {
        "wire_id": None,
        "snapshot": None,
        "effects": None,
        "csrf": None,
        "update_uri": f"{BASE}/livewire/update",
    }
    if component is not None:
        data["wire_id"] = component.get("wire:id")
        data["snapshot"] = component.get("wire:snapshot")
        data["effects"] = component.get("wire:effects")
    m = re.search(
        r'<meta[^>]+name=["\']csrf-token["\'][^>]+content=["\']([^"\']+)["\']',
        html,
        re.I,
    )
    if m:
        data["csrf"] = m.group(1)
    # Livewire script config
    m = re.search(r"livewireScriptConfig\s*=\s*(\{.*?\});", html, re.S)
    if m:
        try:
            cfg = json.loads(m.group(1))
            data["config"] = cfg
            if cfg.get("csrf"):
                data["csrf"] = cfg["csrf"]
            if cfg.get("uri"):
                data["update_uri"] = requests.compat.urljoin(BASE, cfg["uri"])
        except Exception:
            pass
    # Another common pattern
    m = re.search(r'"csrf"\s*:\s*"([^"]+)"', html)
    if m and not data["csrf"]:
        data["csrf"] = m.group(1)
    m = re.search(r'"uri"\s*:\s*"([^"]*livewire[^"]*)"', html)
    if m:
        data["update_uri"] = requests.compat.urljoin(BASE, m.group(1))
    return data


def parse_rows(html: str) -> list[dict]:
    soup = BeautifulSoup(html, "html.parser")
    table = soup.find("table")
    rows = []
    if not table:
        return rows
    headers = [th.get_text(" ", strip=True) for th in table.find_all("th")]
    for tr in table.find_all("tr"):
        tds = [td.get_text(" ", strip=True) for td in tr.find_all("td")]
        if not tds:
            continue
        if headers and len(headers) == len(tds):
            rows.append(dict(zip(headers, tds)))
        else:
            rows.append(
                {
                    "Name": tds[0] if tds else "",
                    "Gender": tds[1] if len(tds) > 1 else "",
                    "Registration Number": tds[2] if len(tds) > 2 else "",
                    "Qualification": tds[3] if len(tds) > 3 else "",
                    "Specialty": tds[4] if len(tds) > 4 else "",
                }
            )
    return rows


def main() -> None:
    s = requests.Session()
    s.headers.update(
        {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Accept": "text/html,application/xhtml+xml",
        }
    )
    r = s.get(URL, timeout=60)
    r.raise_for_status()
    html = r.text
    (OUT / "mdpcz_page1.html").write_text(html, encoding="utf-8")
    meta = extract_livewire(html)
    print("meta keys", {k: (str(v)[:80] if v else v) for k, v in meta.items() if k != "config"})
    print("config", meta.get("config"))
    all_rows = parse_rows(html)
    print("page1 rows", len(all_rows))

    # Try Livewire update for next pages
    if not meta.get("snapshot") and not meta.get("wire_id"):
        print("No livewire snapshot/id found; dumping script tags for debug")
        soup = BeautifulSoup(html, "html.parser")
        for script in soup.find_all("script"):
            txt = script.string or ""
            if "livewire" in txt.lower() or "csrf" in txt.lower():
                print(txt[:500])
                print("---")
        # Try X-Livewire style from cookie/session with wire:id only via POST body patterns
        return

    # Livewire v3 payload
    snapshot = meta["snapshot"]
    # snapshot may be HTML-escaped JSON
    if snapshot:
        snapshot_obj = json.loads(snapshot.replace("&quot;", '"'))
    else:
        snapshot_obj = None

    headers = {
        "X-Livewire": "true",
        "Content-Type": "application/json",
        "X-CSRF-TOKEN": meta.get("csrf") or "",
        "Referer": URL,
        "Accept": "application/json",
    }

    # Attempt export
    if snapshot_obj is not None:
        payload = {
            "_token": meta.get("csrf"),
            "components": [
                {
                    "snapshot": json.dumps(snapshot_obj) if isinstance(snapshot_obj, dict) else snapshot,
                    "updates": {},
                    "calls": [{"path": "", "method": "exportToExcel", "params": []}],
                }
            ],
        }
        # Try multiple payload shapes
        for label, body in [
            ("v3", payload),
            (
                "v2",
                {
                    "fingerprint": {"id": meta["wire_id"], "name": "public-register", "locale": "en", "path": "public_register", "method": "GET"},
                    "serverMemo": {"data": {}},
                    "updates": [],
                    "calls": [{"method": "exportToExcel", "params": [], "path": ""}],
                },
            ),
        ]:
            try:
                resp = s.post(meta["update_uri"], headers=headers, json=body, timeout=120)
                print(label, "export status", resp.status_code, resp.headers.get("content-type"), len(resp.content))
                (OUT / f"mdpcz_export_{label}.bin").write_bytes(resp.content)
                print(resp.text[:400])
            except Exception as exc:  # noqa: BLE001
                print(label, "export err", exc)

    # Paginate a sample of pages via gotoPage
    if snapshot_obj is not None:
        collected = {json.dumps(r, sort_keys=True) for r in all_rows}
        current_snapshot = snapshot
        for page in [2, 3, 10, 100, 200, 465]:
            body = {
                "components": [
                    {
                        "snapshot": current_snapshot,
                        "updates": {},
                        "calls": [{"path": "", "method": "gotoPage", "params": [page, "page"]}],
                    }
                ]
            }
            resp = s.post(meta["update_uri"], headers=headers, json=body, timeout=60)
            print("goto", page, resp.status_code, len(resp.content))
            try:
                data = resp.json()
            except Exception:
                print(resp.text[:300])
                break
            # Extract HTML effects
            html_chunk = ""
            comps = data.get("components") or []
            if comps:
                effects = comps[0].get("effects") or {}
                html_chunk = effects.get("html") or ""
                if comps[0].get("snapshot"):
                    current_snapshot = comps[0]["snapshot"]
            rows = parse_rows(html_chunk) if html_chunk else []
            print("  rows", len(rows), "sample", rows[:1])
            for r in rows:
                collected.add(json.dumps(r, sort_keys=True))
            time.sleep(0.3)
        out_rows = [json.loads(x) for x in collected]
        (OUT / "mdpcz_sample_pages.json").write_text(json.dumps(out_rows, indent=2), encoding="utf-8")
        print("collected unique", len(out_rows))


if __name__ == "__main__":
    main()

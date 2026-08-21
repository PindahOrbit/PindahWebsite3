#!/usr/bin/env python3
"""Download and inspect additional provider sources; enrich MDPCZ/ZACH/Alliance/CellMed."""

from __future__ import annotations

import json
import re
from pathlib import Path

import pdfplumber
import requests
from bs4 import BeautifulSoup

RAW = Path(__file__).resolve().parent / "raw" / "other"
RAW.mkdir(parents=True, exist_ok=True)
SESSION = requests.Session()
SESSION.headers["User-Agent"] = "Mozilla/5.0 (compatible; ZimbabweHealthcareResearch/1.0)"


def download(url: str, name: str) -> Path | None:
    dest = RAW / name
    print(f"GET {url}")
    try:
        r = SESSION.get(url, timeout=120)
        print(" ", r.status_code, r.headers.get("content-type"), len(r.content))
        if r.status_code == 200 and len(r.content) > 1000:
            dest.write_bytes(r.content)
            print("  saved", dest)
            return dest
    except Exception as exc:  # noqa: BLE001
        print("  ERR", exc)
    return None


def list_pdf_links(page_url: str) -> list[tuple[str, str]]:
    r = SESSION.get(page_url, timeout=60)
    r.raise_for_status()
    soup = BeautifulSoup(r.text, "html.parser")
    out = []
    for a in soup.find_all("a", href=True):
        href = a["href"]
        if ".pdf" in href.lower():
            if href.startswith("/"):
                href = requests.compat.urljoin(page_url, href)
            out.append((a.get_text(" ", strip=True), href))
    return out


def inspect_pdf(path: Path, pages: int = 3) -> None:
    print("INSPECT", path.name)
    with pdfplumber.open(path) as pdf:
        print(" pages", len(pdf.pages))
        for i in range(min(pages, len(pdf.pages))):
            p = pdf.pages[i]
            tables = p.extract_tables() or []
            print(f" page {i+1} tables={len(tables)}")
            if tables:
                for r in tables[0][:8]:
                    print("  ", r)
            else:
                text = p.extract_text() or ""
                print(text[:900])


def scrape_zach_full() -> list[dict]:
    url = "https://zach.org.zw/membership/"
    r = SESSION.get(url, timeout=60)
    soup = BeautifulSoup(r.text, "html.parser")
    rows = []
    for tr in soup.select("table tr"):
        tds = [td.get_text(" ", strip=True) for td in tr.find_all("td")]
        if tds:
            rows.append(tds)
    print("ZACH rows", len(rows))
    # maybe paginated via query
    for q in ["?page=1", "?page=2", "?page=all", "/membership/page/2/"]:
        try:
            rr = SESSION.get("https://zach.org.zw/membership/" + q.lstrip("/"), timeout=30)
            ss = BeautifulSoup(rr.text, "html.parser")
            n = len(ss.select("table tr"))
            print(" ZACH try", q, rr.status_code, "rows", n)
        except Exception as exc:  # noqa: BLE001
            print(" ZACH try fail", q, exc)
    (RAW / "zach_rows.json").write_text(json.dumps(rows, indent=2), encoding="utf-8")
    return rows


def inspect_mdpcz() -> None:
    url = "https://www.mdpcz.co.zw/public_register"
    r = SESSION.get(url, timeout=60)
    html = r.text
    (RAW / "mdpcz_public_register.html").write_text(html, encoding="utf-8")
    print("MDPCZ html saved", len(html))
    clicks = re.findall(r"wire:click(?:\.prevent)?=\"([^\"]+)\"", html)
    print("wire clicks", clicks[:30])
    snaps = re.findall(r"wire:snapshot=\"([^\"]{0,80})", html)
    print("snapshots found", len(snaps))
    ids = re.findall(r"wire:id=\"([^\"]+)\"", html)
    print("wire ids", ids[:10])
    # look for csrf / livewire endpoint
    for pat in ["/livewire/update", "livewireScriptConfig", "csrf-token"]:
        print(pat, pat in html)
    soup = BeautifulSoup(html, "html.parser")
    for b in soup.find_all(["button", "a"]):
        t = b.get_text(" ", strip=True)
        if "export" in t.lower() or "excel" in t.lower():
            print("BTN", t, {k: v for k, v in b.attrs.items() if "wire" in k or k in {"href", "class"}})


def main() -> None:
    # Alliance
    try:
        links = list_pdf_links("https://www.alliancehealth.co.zw/service-providers-pdf")
        print("Alliance links:")
        for t, h in links:
            print(" ", t, h)
            if ".pdf" in h.lower():
                name = h.rstrip("/").split("/")[-1] or "alliance_providers.pdf"
                p = download(h, name if name.lower().endswith(".pdf") else "alliance_providers.pdf")
                if p:
                    inspect_pdf(p)
    except Exception as exc:  # noqa: BLE001
        print("Alliance fail", exc)

    # CellMed via zimmedicover
    p = download(
        "https://zimmedicover.com/wp-content/uploads/2024/12/CellMed-Service-Provider-Directory.pdf",
        "CellMed-Service-Provider-Directory.pdf",
    )
    if p:
        inspect_pdf(p, pages=4)

    # CellMed self-service page
    try:
        r = SESSION.get("https://cellgroup.co.zw/self-service/medical-aid-service-providers/", timeout=60)
        print("cellgroup", r.status_code, len(r.text))
        (RAW / "cellgroup_providers.html").write_text(r.text, encoding="utf-8")
        soup = BeautifulSoup(r.text, "html.parser")
        for a in soup.find_all("a", href=True):
            if ".pdf" in a["href"].lower():
                print("cell pdf", a.get_text(strip=True), a["href"])
    except Exception as exc:  # noqa: BLE001
        print("cellgroup fail", exc)

    scrape_zach_full()
    inspect_mdpcz()


if __name__ == "__main__":
    main()

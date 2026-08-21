#!/usr/bin/env python3
import re
from pathlib import Path
import requests
from bs4 import BeautifulSoup

OUT = Path(__file__).resolve().parent / "raw" / "other"
OUT.mkdir(parents=True, exist_ok=True)
s = requests.Session()
s.headers["User-Agent"] = "Mozilla/5.0"
url = "https://www.alliancehealth.co.zw/service-providers-pdf"
r = s.get(url, timeout=60)
print(r.status_code, len(r.text))
(OUT / "alliance_page.html").write_text(r.text, encoding="utf-8")
soup = BeautifulSoup(r.text, "html.parser")
for a in soup.find_all("a", href=True):
    print("A", a.get_text(" ", strip=True)[:80], "->", a["href"])
for m in sorted(set(re.findall(r"[^\s\"']+\.pdf", r.text, re.I))):
    print("pdf", m)
for m in sorted(set(re.findall(r"/system/files/[^\s\"']+", r.text))):
    print("sys", m)
for m in sorted(set(re.findall(r"/sites/[^\s\"']+\.pdf", r.text, re.I))):
    print("sites", m)

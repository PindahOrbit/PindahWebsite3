# AI for Impact Challenge 2026 — Solution Proposal

**Project:** Pindah AI-Augmented Enterprise Platform  
**Applicant:** Pindah Private Limited (Pindah / Orbit Frame)  
**Team Leader:** Leeroy Tonderai Mubaiwa  
**Date:** 6 July 2026  
**Website:** https://pindah.org

---

## 1. Executive Summary

Pindah Private Limited has built a unified enterprise operating platform for Zimbabwe and Southern Africa. The platform integrates ERP, CRM, healthcare, education (Frame), manufacturing, logistics, insurance, construction, HR, and document management on a single data layer — localized for USD/ZiG multi-currency, IFRS compliance, ZIMRA fiscal integration, offline operation, and WhatsApp/EcoCash connectivity.

Our AI layer adds practical public value: an LLM-powered sales assistant, automated enterprise insights publishing, OCR and intelligent document classification, and full-text search across educational resources. Unlike generic AI tools, Pindah AI is grounded in real operational workflows and Zimbabwe-specific compliance requirements.

---

## 2. Problem Statement

Organizations across Zimbabwe — schools, clinics, manufacturers, distributors, and public institutions — frequently outgrow spreadsheets and disconnected systems. Information sits in silos, compliance requires manual effort, and unreliable connectivity limits cloud-only solutions. Generic international ERP products are expensive, poorly localized, and lack integration with ZIMRA, NSSA, ZIMSEC, and local payment rails.

There is a clear need for affordable, locally engineered enterprise infrastructure with embedded AI that reduces administrative friction, improves record-keeping, and accelerates decision-making.

---

## 3. Proposed Solution

### 3.1 Platform Architecture

| Layer | Technology |
|-------|------------|
| Application | ASP.NET Core 8.0 MVC, API-first architecture |
| Database | SQLite / relational stores with Entity Framework Core |
| AI inference | Ollama LLM integration with streaming chat |
| Scheduling | Quartz.NET for background jobs (OCR, content generation) |
| Search | SQLite FTS5 full-text search |
| Deployment | Cloud, on-premise, or hybrid |

### 3.2 Core Modules (14+ verticals)

- **ERP & Accounting:** General ledger, AP/AR, inventory, procurement, POS, IFRS reporting, ZIMRA fiscal devices
- **CRM:** Sales pipeline, lead tracking, quotations, service desk
- **Healthcare:** Patient registration, clinical workflows, pharmacy, laboratory, billing
- **Education (Frame):** Enrollment, fees, attendance, timetabling, ZIMSEC-aligned reporting
- **Manufacturing:** BOM, production scheduling, shop floor, quality, maintenance
- **Logistics:** Fleet, route optimization, cross-border documentation
- **Insurance, Construction, HR, DMS, SCM**

### 3.3 AI Capabilities (Implemented)

1. **AI Sales Assistant** — Website chat agent recommending modules and indicative pricing with WhatsApp handoff
2. **Automated Content Generation** — Scheduled AI-generated enterprise insights and blog articles
3. **Document Intelligence (DMS)** — OCR, barcode/QR classification, background batch processing
4. **Educational Search** — Full-text search over ZIMSEC document corpus

### 3.4 Localization & Compliance

- Native USD/ZiG multi-currency
- IFRS transaction-level compliance
- ZIMRA fiscal integration
- NSSA/PAYE payroll (HR module)
- Offline synchronization for unreliable connectivity
- ISO 27001-aligned security architecture

---

## 4. Innovation & Differentiation

| Feature | Pindah Advantage |
|---------|------------------|
| Unified data layer | Finance, ops, and vertical workflows share one source of truth |
| Zimbabwe-first design | Built for local currency, tax, curriculum, and infrastructure realities |
| Embedded AI | Grounded in operational data, not generic chatbots |
| Affordability | SaaS from $28–45/user/month; Frame from $1/student/month |
| Multi-sector reach | One platform serving education, health, manufacturing, logistics, mining |

---

## 5. Technical Specifications

- **Role-based access control** with segregation of duties
- **Immutable audit trails** for compliance readiness
- **Real-time GL posting** — inventory movements update ledger automatically
- **API-first** integration with EcoCash, WhatsApp, and third-party systems
- **Privacy by design** — client tenant data not used to train public third-party models

---

## 6. Projected Benefits & Impact

**Operational:**
- Month-end close compressed from weeks to days
- Continuous audit readiness
- Staff capacity shifts from reconciliation to analysis

**Sector impact:**
- **Schools:** Digital admissions, fee management, parent communication, ZIMSEC reporting
- **Healthcare:** End-to-end patient/clinical/billing workflows
- **Manufacturing:** MES, traceability, quality, OEE tracking
- **SMEs:** Affordable digital infrastructure replacing spreadsheets

**Economic:**
- Job creation in software engineering and implementation
- Reduced cost of compliance for Zimbabwean enterprises
- Strengthened local AI innovation ecosystem

---

## 7. Implementation Status

- **Live platform:** https://pindah.org
- **Client portal:** https://basa.pindah.org
- **Active deployments:** Schools (Frame/SMS), commercial ERP clients
- **Markets:** Zimbabwe, Zambia, Mozambique, South Africa

---

## 8. Track Selection: Development

Pindah fits the **Development** track because we have shipped production AI features (LLM chat agent, automated content pipeline, OCR/DMS intelligence) on a full-stack enterprise platform. Secondary fit: **Data** (FTS search, OCR, analytics dashboards) and **Deployment** (cloud/on-prem/hybrid).

---

## 9. Post-Challenge Roadmap

1. Expand AI assistant to in-app operational queries (inventory, payroll, student records)
2. Predictive analytics for manufacturing maintenance and logistics routing
3. Multi-tenant AI fine-tuning on anonymized operational patterns
4. Expanded ZIMSEC and public-sector integrations
5. Milestone-based rollout to shortlisted incubation partners

---

## 10. Team & Organization

**Pindah Private Limited** — Harare, Zimbabwe  
**Contact:** admin@pindah.org | +263 714 856 897  
**Team Leader:** Leeroy Tonderai Mubaiwa — enterprise software engineer with experience building Balanced Scorecard, psychometric, ERP, and school management systems for Zimbabwean organizations.

---

*This proposal is the original work of Pindah Private Limited. All information is accurate to the best of our knowledge.*

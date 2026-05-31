# Enterprise Software Module Reference
**Pindah Private Limited · Internal Reference**  
Verticals: 12 | Modules: 120+ | Standards: ISO, IFRS, ICD, OSHA, SCORM | Compiled: 2025

---

## Table of Contents

1. [Enterprise Resource Planning (ERP)](#1-enterprise-resource-planning-erp)
2. [Customer Relationship Management (CRM)](#2-customer-relationship-management-crm)
3. [School Management System (SMS)](#3-school-management-system-sms)
4. [Manufacturing Management](#4-manufacturing-management)
5. [Insurance Management](#5-insurance-management)
6. [Accounting](#6-accounting)
7. [Logistics & Fleet Management](#7-logistics--fleet-management)
8. [Human Resources (HR) & Payroll](#8-human-resources-hr--payroll)
9. [Hospital / Clinic Management](#9-hospital--clinic-management)
10. [Document Management System (DMS)](#10-document-management-system-dms)
11. [Construction Management](#11-construction-management)
12. [Supply Chain Management (SCM)](#12-supply-chain-management-scm)

---

## 1. Enterprise Resource Planning (ERP)

> **Scope:** Integrated core business operations — finance, procurement, inventory, sales, and projects.

An ERP centralises all operational data and processes into a single system of record. It eliminates data silos, enforces workflow discipline, and provides real-time visibility across the organisation. World-class benchmarks: **SAP S/4HANA**, **Oracle NetSuite**, **Microsoft Dynamics 365**, **Odoo**. For Zimbabwe SMEs, multi-currency (USD/ZiG), ZIMRA fiscal device integration, and EcoCash payment reconciliation are non-negotiable local requirements.

**Standards:** IFRS · IAS 1/2/16/36 · ZIMRA SI.104 · ISO 27001

---

### 1.1 General Ledger

The financial backbone of the ERP. Records every debit and credit across the chart of accounts. Every transaction anywhere in the system posts here automatically.

- Chart of accounts management (configurable account hierarchy)
- Journal entry creation, approval workflows, and reversals
- Recurring journal entries (depreciation, prepayments)
- Multi-currency posting with exchange rate tables
- Period-end and year-end closing controls with lock-down
- Automatic financial statement generation: Income Statement, Balance Sheet, Cash Flow
- Inter-company elimination for group reporting
- Full immutable audit trail on every posting

**Standards:** IAS 1 · IFRS 9

---

### 1.2 Accounts Payable (AP)

Manages all outgoing payments to suppliers. Prevents duplicate payments, overpayments, and missed early-payment discounts.

- Supplier invoice registration (manual and OCR-based capture)
- 3-way matching engine: Purchase Order → Goods Received Note → Invoice
- Invoice approval workflows (email/in-app)
- Debtor aging analysis (30 / 60 / 90 / 120 days)
- Batch payment runs: EFT, RTGS, cheque, EcoCash, ZiG
- Early payment discount tracking
- Withholding tax computation per ZIMRA rates
- Supplier statement reconciliation
- AP sub-ledger reconciliation to GL

**Standards:** IAS 37 · ZIMRA Withholding Tax Regulations

---

### 1.3 Accounts Receivable (AR)

Tracks money owed by customers. Drives collections discipline and minimises bad debt.

- Customer invoicing and credit note issuance
- Credit limit definition and enforcement
- Automated payment reminders (email, SMS, WhatsApp)
- Receipt allocation and matching against open invoices
- Debtor aging reports (30 / 60 / 90 / 120 days)
- Bad debt provisioning and write-off workflow
- AR sub-ledger reconciliation to GL
- Interest on overdue accounts

**Standards:** IFRS 15 · IFRS 9 (expected credit loss model)

---

### 1.4 Inventory Management

Tracks stock quantities and valuations across multiple warehouses and locations. The system prevents stockouts, overstocking, and shrinkage.

- Multi-location / multi-warehouse stock tracking
- Serial number and batch / lot number tracking
- FIFO, LIFO, and Weighted Average Cost (WAC) valuation methods
- Reorder point and min-max replenishment alerts
- Stock adjustment (variance capture with reason codes)
- Stock transfers between locations
- Cycle counting and full stock count management
- Landed cost allocation to stock items
- Slow-moving and obsolete stock reports

**Standards:** IAS 2 · FIFO/WAC costing

---

### 1.5 Procurement / Purchasing

End-to-end purchase process from internal request to goods receipt.

- Purchase requisition with multi-level approval chains
- Budget check at requisition stage (prevents overspend)
- Supplier Request for Quotation (RFQ) distribution
- Quotation comparison matrix (price, delivery, quality)
- Purchase order creation, amendment, and version control
- Blanket purchase orders with release orders
- Goods Received Note (GRN) with partial receipting
- Return-to-supplier workflow
- Supplier performance scorecard (on-time delivery, quality, price variance)

**Standards:** ISO 20400 (Sustainable Procurement)

---

### 1.6 Point of Sale (POS)

Retail transaction interface for cashiers. Designed for high-volume, multi-tender environments including Zimbabwe's mobile money ecosystem.

- Product lookup by barcode, QR code, or name search
- Multi-tender: cash (USD/ZiG), card (Visa/Mastercard), EcoCash, OneMoney, ZiG RTGS
- ZIMRA fiscal device integration (receipt printing and VAT reporting)
- Cashier session management (float in, cash out, till reconciliation)
- Void, refund, and exchange controls with supervisor override
- Loyalty points accumulation at POS
- Offline mode with sync-on-reconnect for poor connectivity

**Standards:** ZIMRA SI.104 (Fiscalisation)

---

### 1.7 Sales & Invoicing

Manages the full sales cycle from quotation to tax-compliant invoice.

- Quotation → Sales Order → Delivery Note → Invoice workflow
- Customer-specific and quantity-based price lists
- Trade discount and promotional discount management
- Proforma invoice generation for export/prepayment
- E-invoicing (PDF and email delivery)
- Sales rep commission tracking
- Sales order backorder management
- Credit note and return merchandise authorisation (RMA)

**Standards:** IFRS 15 (Revenue Recognition) · ZIMRA VAT Act

---

### 1.8 Project Management

Tracks project budgets, tasks, resources, and time. Bills time and materials to clients with precise job costing.

- Project and phase/milestone setup
- Task assignment and Gantt chart view
- Time and expense capture (timesheet entry)
- Job costing: direct material, direct labour, overhead
- WIP (Work-In-Progress) valuation feeding GL
- Client billing: time-and-material or fixed-fee
- Project P&L reporting
- Budget vs actual variance by phase

**Standards:** IAS 11 (Construction Contracts) · PMI PMBOK

---

### 1.9 Fixed Assets

Manages the full lifecycle of capital assets from acquisition to disposal.

- Asset register and categorisation (land, plant, equipment, vehicles, IP)
- Depreciation computation: straight-line, reducing balance, units of production
- Depreciation run automation (monthly/annual)
- Asset revaluation and impairment testing
- Asset disposal / retirement with gain-or-loss calculation
- Capital expenditure (CAPEX) budgeting and tracking
- Physical asset verification workflow
- Insurance value tracking and renewal alerts
- Asset tagging and barcode/QR scanning

**Standards:** IAS 16 (Property, Plant & Equipment) · IAS 36 (Impairment)

---

### 1.10 Budgeting & Forecasting

Financial planning tool that connects targets to actuals in real time.

- Annual and rolling budget entry by cost centre or department
- Bottom-up and top-down budget consolidation
- Budget vs actual variance analysis with drilldown
- Rolling forecast updates (monthly reforecast)
- What-if scenario modelling
- Multi-year strategic planning support
- Executive dashboard KPIs (revenue, margin, opex, capex)

---

### 1.11 Reporting & Analytics

Consolidated reporting layer with drilldown, scheduling, and BI connectivity.

- Standard financial statements (IFRS-compliant)
- Management accounts pack (monthly / quarterly)
- Custom report builder (row, column, formula configuration)
- Scheduled report delivery via email
- Export to Excel, PDF, CSV
- Role-based data visibility (department heads see only their data)
- Power BI / Tableau connector via REST API

---

### 1.12 Audit & Compliance

Full audit log and segregation-of-duties (SOD) enforcement.

- Immutable change log: every create, edit, delete with user and timestamp
- SOD conflict detection and alerting
- ZIMRA VAT audit file export (standard audit file format)
- Data retention policy enforcement
- User access review and certification workflow
- Regulatory report library (ZIMRA, NSSA, ZIMDEF)

**Standards:** ISO 27001 · ZIMRA Audit Requirements

---

> **Case Study — SAP S/4HANA at Unilever Zimbabwe**  
> Unilever Zimbabwe consolidated finance, procurement, and supply chain on SAP S/4HANA, eliminating manual spreadsheet reconciliations across 6 product lines. Real-time inventory visibility reduced stock write-offs by 18% in year one. A ZIMRA e-VAT module was custom-built for local fiscal device compliance — a pattern directly applicable to Pindah Basa deployments.

---

## 2. Customer Relationship Management (CRM)

> **Scope:** Lead-to-loyalty customer lifecycle — sales pipeline, marketing automation, and after-sale service.

A CRM centralises all customer interactions and gives the sales team a structured system to convert prospects into revenue. World benchmarks: **Salesforce Sales Cloud**, **HubSpot**, **Microsoft Dynamics 365 Sales**, **Zoho CRM**. In Zimbabwe's context: WhatsApp-first communication, mobile UX, and ZiG/USD dual-currency quotations are key differentiators.

**Standards:** ISO 10002 (Customer Satisfaction) · POTRAZ Consumer Protection · GDPR principles

---

### 2.1 Lead Management

Captures, qualifies, and assigns incoming enquiries before they go cold.

- Lead capture from web forms, phone, WhatsApp, walk-in, referral
- Lead scoring based on demographic and engagement signals
- Assignment rules: territory-based, product-based, round-robin
- Automated follow-up task creation on lead arrival
- SLA tracking: first-contact response time
- Lead source attribution (where are best leads coming from?)
- Duplicate detection and merge

---

### 2.2 Opportunity & Sales Pipeline

Tracks deals through defined stages with pipeline visibility for management.

- Configurable sales stages (Prospecting → Proposal → Negotiation → Closed Won/Lost)
- Probability weighting per stage for revenue forecasting
- Kanban and list pipeline views
- Win / loss reason capture for coaching
- Activity timeline: calls logged, emails sent, meetings held
- Sales cycle duration tracking
- Competitor tracking per opportunity

---

### 2.3 Contact & Account Management

360° view of every customer — contacts, history, transactions, documents in one profile.

- Contact profile: name, role, communication preferences, history
- Account profile: company, address, revenue tier, credit terms
- Parent-child account hierarchy (e.g. Trinity Pharmacy head office → 18 branches)
- Interaction timeline: calls, emails, WhatsApp messages, visits
- Document library per account (contracts, proposals, correspondence)
- Custom fields and tags for segmentation
- Duplicate detection and merge

---

### 2.4 Marketing Automation

Plans, executes, and measures campaigns across email, SMS, and WhatsApp.

- Campaign builder with drag-and-drop sequence editor
- Contact list segmentation (industry, geography, engagement, product interest)
- Drip sequence scheduling (day 0 → day 3 → day 7 follow-ups)
- WhatsApp Business API integration for campaign messages
- Open rate, click-through rate, and conversion tracking
- A/B testing on subject lines and message content
- Campaign ROI calculation (revenue influenced / campaign cost)
- GDPR-compliant opt-out management

---

### 2.5 Customer Service & Helpdesk

Manages after-sale support and complaint resolution through a structured ticketing system.

- Ticket creation from email, WhatsApp, web form, or phone log
- Ticket categorisation and priority assignment
- SLA definition by ticket category (e.g. billing: 4hr response, technical: 8hr)
- SLA breach alerts and escalation routing
- Internal notes and external reply threads on each ticket
- Knowledge base article library (self-service FAQ)
- Customer satisfaction (CSAT) survey sent on ticket closure
- Recurring issue trend reporting for product feedback

**Standards:** ISO 10002

---

### 2.6 Quotation & Proposal Management

Generates branded quotes from within the CRM, pulling product catalogue and pricing.

- Product and service catalogue with standard pricing
- Multi-currency quotes (USD / ZiG with live exchange rates)
- Branded PDF proposal with company letterhead
- Quote versioning (V1, V2, V3 comparison)
- E-signature workflow for acceptance
- Expiry date and automated reminder
- Quote-to-sales-order conversion (pushes to ERP)
- Discount approval thresholds (e.g. >10% requires manager approval)

---

### 2.7 CRM Analytics & Reporting

Sales intelligence dashboards covering pipeline, performance, and customer value.

- Sales funnel conversion rates by stage
- Revenue forecast (weighted pipeline)
- Sales rep performance leaderboard
- Deal velocity (average days to close by product/industry)
- Customer Lifetime Value (CLV) modelling
- Churn risk scoring based on engagement signals
- Campaign contribution to pipeline and revenue

---

### 2.8 Loyalty & Retention

Manages loyalty programmes, customer tiers, and win-back campaigns.

- Points accumulation rules (per purchase, per referral)
- Points redemption at POS or on invoice
- Customer tier management (Bronze / Silver / Gold / Platinum)
- Tier upgrade and downgrade triggers
- Automated win-back campaigns for dormant customers
- Net Promoter Score (NPS) survey integration
- Customer anniversary and birthday triggers

---

> **Case Study — Salesforce at Old Mutual Zimbabwe**  
> Old Mutual Zimbabwe deployed Salesforce Financial Services Cloud to manage advisor-client relationships across 15 branches. Automated client follow-up reminders lifted policy renewal rates significantly. A WhatsApp Business integration was added locally — now considered table-stakes for financial services CRM in Zimbabwe.

---

## 3. School Management System (SMS)

> **Scope:** K-12 school administration — admissions, academics, fees, staff, parent engagement. Product: **Frame by Pindah**.

Zimbabwe has over 5,000 K-12 schools, the vast majority still paper-based or using fragmented tools. Frame targets this gap at **$1/student/month** with a **$5/student setup fee**. World benchmarks: **PowerSchool**, **Classtar**, **Fedena**, **SIMS (UK)**. Key local requirements: ZIMSEC subject mapping, ZiG/USD fee collection, WhatsApp parent communication, and NSSA/PAYE payroll compliance.

**Standards:** ZIMSEC · MoPSE Curriculum · Labour Act Ch.28:01 · NSSA Act

---

### 3.1 Admissions & Enrolment

Manages the full student intake process from application to class placement.

- Online application form with parent-facing portal
- Document upload: birth certificate, national ID, previous school report
- Placement test result capture and class assignment
- Auto-generated student ID numbers and login credentials
- Sibling linkage for multi-child families
- Waiting list management with priority ranking
- Form / class capacity controls
- Historical cohort tracking (Form 1 2025, Form 1 2026)

---

### 3.2 Attendance Management

Digital attendance registers replacing paper roll-call. Absent student alerts sent to parents automatically.

- Daily and period-by-period attendance registers
- Mobile app or web interface for teacher mark-off
- Automated parent alerts: SMS, email, or WhatsApp on absence
- Late arrival and early departure logging
- Leave application and HOD/principal approval
- Attendance trend reports per student, class, and subject
- Term attendance percentage for report card (e.g. must attend ≥80%)
- Chronic absenteeism flagging for pastoral intervention

---

### 3.3 Grades & Assessment

Teachers enter continuous assessment (CA) marks and exam results; the system computes totals, grades, and rankings.

- Subject-specific CA mark entry (e.g. tests, projects, practicals)
- Exam mark entry with configurable CA:Exam weighting
- ZIMSEC grading scale configuration (A*, A, B, C, D, E, U)
- Custom school grading table support
- Class rank and subject rank computation
- Teacher-level and head-of-department approval workflow for results
- Term-end report card PDF generation (branded per school)
- Historical results archive per student across all years

**Standards:** ZIMSEC Grading Framework

---

### 3.4 Timetabling

Builds and manages school timetables, assigning teachers, subjects, rooms, and periods while detecting conflicts.

- Period and day configuration (5-day, 6-day week; multi-period blocks)
- Teacher-subject-class assignment matrix
- Conflict detection: teacher double-booking, room clash
- Room and lab allocation
- Relief teacher assignment for absent staff
- Timetable PDF export (wall display, student copy, teacher copy)
- Academic calendar integration (terms, half-terms, public holidays)

---

### 3.5 Fee Management

Manages school fee billing, payment collection, and arrears in USD and ZiG.

- Term fee schedule configuration per form/grade
- Additional fees: boarding, uniform, sport, excursion
- Sibling discount and scholarship deduction management
- Multi-currency invoicing (USD / ZiG)
- Payment recording: cash, EcoCash, bank transfer, RTGS
- EcoCash and ZiPay merchant reconciliation
- ZIMRA-compliant fiscal receipt generation
- Arrears reports with parent notification (WhatsApp / SMS)
- Fee clearance check (prevents report card release if outstanding)

**Standards:** ZIMRA Fiscalisation · Consumer Protection Act

---

### 3.6 Library Management (PindahLibra)

Manages the school library catalogue, borrowing, and returns.

- Book catalogue with Dewey Decimal classification
- Barcode/QR code-based check-in and check-out
- Student borrowing history and current loans
- Overdue fine calculation and collection
- Book reservation / hold requests
- Digital resource cataloguing (e-books, web links)
- Annual stock count and lost book reporting
- Reading statistics per class (total books borrowed, average per student)

---

### 3.7 Staff & Payroll

Manages teacher and support staff HR records, leave, and monthly payroll computation per Zimbabwe law.

- Staff profiles: personal, contract, qualifications, next-of-kin
- Employment contract storage (scanned documents)
- Leave application and approval (annual, sick, maternity, study)
- Leave balance tracking and accrual
- Monthly payroll computation: basic, allowances, overtime
- Statutory deductions: PAYE, NSSA, ZIMDEF, AIDS Levy
- Payslip generation (PDF and email)
- P2 form output for ZIMRA
- NSSA monthly return generation (NEC Form)
- Bulk bank upload file generation for salary payment

**Standards:** Labour Act Ch.28:01 · Income Tax Act · NSSA Act · ZIMDEF Act

---

### 3.8 Parent & Student Portal

Secure web and WhatsApp access for parents and students to view school data in real time.

- Parent login linked to one or more students
- Real-time results and attendance view
- Fee balance and payment history
- School announcements, circulars, and event calendar
- Direct messaging to class teacher or school admin
- Student self-service: timetable, assignments, resources
- WhatsApp chatbot integration (view results, check fee balance via WhatsApp message)
- Mobile-responsive web interface (no app download required)

---

### 3.9 Examinations

Schedules internal examinations, assigns invigilators, and manages results entry post-exam.

- Internal exam schedule creation
- Venue and invigilator assignment with workload balancing
- Automated seating plan generation
- ZIMSEC exam entry list generation and export
- Invigilation duty notification (email/WhatsApp to teacher)
- Exam irregularity/incident reporting log
- Results upload from teacher mark books

**Standards:** ZIMSEC Examination Regulations

---

### 3.10 Hostel & Boarding Management

For boarding schools, manages room assignments, meal plans, and boarder welfare.

- Dormitory and bed assignment per student
- Boarder fee invoicing (separate from tuition fee schedule)
- Weekend exeat application and approval workflow
- Welfare check logs (nightly roll call records)
- Parent contact for absence or health incidents
- Boarding house master shift log

---

> **Case Study — Frame at Lower Gwelo Adventist High School**  
> Lower Gwelo Adventist High School became Frame's first paying client at $1/student/month with a $5/student setup fee. The Form 1 2026 cohort student records were migrated from Excel using custom SQL UPDATE scripts patching student IDs inside ASP.NET Identity's JSON `custom_args` column. Parent communication now flows through Frame's notification engine, replacing printed circulars.

---

## 4. Manufacturing Management

> **Scope:** Production planning, shop floor execution, quality control, batch traceability, and product costing.

A manufacturing system manages the conversion of raw materials into finished goods. It enforces production schedules, tracks shop floor activities in real time, controls quality at every stage, and calculates precise product costs. World benchmarks: **SAP Manufacturing**, **Oracle Manufacturing Cloud**, **Infor CloudSuite Industrial**, **Epicor**. For **Pharmanova Zimbabwe** (Pindah client), GMP compliance, batch traceability to MCAZ standards, and validated systems (21 CFR Part 11 equivalent) are mandatory.

**Standards:** ISO 9001 · GMP (WHO Guidelines) · MCAZ Regulations · IAS 2 · ISO 55001

---

### 4.1 Bill of Materials (BOM)

Defines the exact recipe or formula for every finished product — ingredients, quantities, and substitution rules.

- Multi-level BOM creation (finished goods → sub-assemblies → raw materials)
- Component quantity and unit-of-measure definition
- Substitution / alternative component rules
- BOM version management with effective date control
- Component availability check before production order release
- Cost rollup: total standard material cost from BOM
- Scrap and yield factor definition per component

---

### 4.2 Production Planning & Scheduling

Converts demand into feasible production orders with full material and capacity awareness.

- Master Production Schedule (MPS) from sales orders or forecast
- Material Requirements Planning (MRP) engine: calculates what to buy and when
- Capacity planning by work centre and shift
- Production order creation (manual, MPS-driven, or MRP-driven)
- Schedule optimisation: sequence production to minimise changeovers
- Lead time management: procurement + production lead times
- What-if simulation: impact of rush orders on the schedule

**Standards:** MRP II · APICS CPIM

---

### 4.3 Shop Floor Control

Real-time tracking of production execution on the factory floor.

- Work order dispatch to work centres / departments
- Worker time and attendance at each machine/station
- Production quantity reporting: started, completed, scrapped
- Real-time production progress dashboard
- Machine downtime logging (planned vs unplanned, reason codes)
- Shift handover report generation
- Job card / traveller document printing
- Worker productivity metrics per shift

---

### 4.4 Quality Control

Enforces quality checks at defined inspection points (in-process and finished goods).

- Inspection plan definition: checkpoints, test methods, acceptable limits
- Test result capture: pass/fail, numeric value, visual observation
- Non-Conformance Report (NCR) creation and disposition workflow
- Quarantine and hold workflow (for Pharmanova's chemical quarantine process)
- Statistical Process Control (SPC) charts for process monitoring
- Certificate of Analysis (CoA) generation per batch
- GMP batch release: Quality Assurance sign-off before dispatch
- Corrective and Preventive Action (CAPA) tracking

**Standards:** ISO 9001 · WHO GMP · MCAZ

---

### 4.5 Batch & Lot Traceability

Full forward and backward traceability from raw material supplier to end customer. Critical for pharmaceutical recalls and regulatory audits.

- Batch number assignment at goods receipt
- Batch genealogy: parent batch → child batches (for Pharmanova's granulation to compression to packaging stages)
- Forward trace: which customers received a specific batch?
- Backward trace: which supplier provided the raw material in this finished batch?
- Expiry date (shelf life) management and alerts
- Recall management workflow with customer notification
- Regulatory traceability report for MCAZ audit

**Standards:** GMP Annex 11 · MCAZ Batch Traceability Requirements

---

### 4.6 Product Costing

Calculates the true manufacturing cost of each product and compares standard to actual.

- Standard cost definition: material, labour, machine rate, overhead absorption
- Actual cost collection from production (materials issued, time booked)
- Overhead absorption rate calculation by work centre
- Standard vs actual cost variance analysis (material, labour, overhead)
- WIP (Work-In-Progress) valuation for financial reporting
- Cost of goods manufactured report
- Product profitability by SKU

**Standards:** IAS 2 (Inventory Valuation)

---

### 4.7 Machine & Equipment Maintenance (CMMS)

Schedules and tracks maintenance on production equipment to minimise unplanned downtime.

- Preventive maintenance schedule by machine (hours, calendar, cycles)
- Maintenance work order creation and assignment to technician
- Spare parts inventory linked to maintenance work orders
- Breakdown / corrective maintenance log (time to repair, root cause)
- Overall Equipment Effectiveness (OEE) reporting: Availability × Performance × Quality
- Maintenance cost tracking per machine
- Equipment calibration schedule and certificates (for pharma lab instruments)

**Standards:** ISO 55001 (Asset Management) · GMP Equipment Validation

---

### 4.8 Warehouse (Manufacturing Stores)

Manages raw material stores and finished goods warehouse within the factory.

- Raw material receipt and goods-in inspection
- Issuing materials to production orders (pick list generation)
- Finished goods receipt from production
- Finished goods despatch to customers
- Shelf-life and FEFO (First Expiry First Out) enforcement
- Bonded / quarantine zone management (Pharmanova chemicals)
- Physical stock count against production records

---

> **Case Study — Pharmanova Zimbabwe (Pindah Client)**  
> Pharmanova Zimbabwe requires an integrated system covering Warehouse, Granulation, Compression, Wets, Packaging, Quality, Validation, Dispatch, and Regulatory departments — with role-based access per department. Batch traceability (pill drop testing, chemical quarantine), GMP-compliant quality sign-off, and MCAZ regulatory document generation are the core functional requirements driving Pindah's manufacturing module specification.

---

## 5. Insurance Management

> **Scope:** Policy administration, underwriting, claims, reinsurance, and regulatory reporting.

Insurance software manages the full insurance value chain — from quoting a policy through to claims settlement and reinsurance cession. World benchmarks: **Guidewire**, **Majesco**, **Duck Creek**, **Nexus Broking System** (used by ZIB). Key Zimbabwean requirements: IPEC compliance, ZiG/USD multi-currency premiums, and ZIMRA withholding tax on premiums.

**Standards:** IPEC Insurance Act · IAS 4 (Insurance Contracts) · IFRS 17

---

### 5.1 Policy Administration

Manages the full lifecycle of insurance policies from inception to renewal or cancellation.

- New policy creation: personal, commercial, marine, motor, life
- Policy schedule generation (PDF, branded)
- Endorsement (mid-term adjustment) processing
- Renewal management with automated notifications
- Policy lapse, reinstatement, and cancellation workflows
- Multi-currency premium management (USD / ZiG)
- Policy document storage (scan and attach)
- Client portal for policy self-service

---

### 5.2 Underwriting

Assesses risk and determines premium pricing for new and renewed policies.

- Risk assessment questionnaire per product class
- Rating engine: configurable premium calculation rules
- Underwriting rules and acceptance criteria
- Referral workflow for risks exceeding underwriter authority
- No-claims discount (NCD) calculation
- Sum insured adequacy check (asset valuation vs insured value)
- Coinsurance and co-insurer participation management
- Underwriter performance dashboard

---

### 5.3 Claims Management

End-to-end claims processing from First Notification of Loss (FNOL) to settlement payment.

- FNOL registration: date, peril, insured details, description
- Claim assignment to assessor/adjuster
- Assessment report capture and document attachment
- Reserve setting and reserve revision history
- Claim approval workflow with authority limits
- Settlement calculation and payment authorisation
- Subrogation and salvage recovery tracking
- Claims ratio reporting by product, branch, and underwriter

---

### 5.4 Reinsurance

Manages facultative and treaty reinsurance arrangements that protect the insurer's own risk exposure.

- Reinsurance treaty setup (proportional, excess of loss)
- Automatic cession calculation on policy inception
- Facultative placement workflow
- Reinsurance premium bordereaux generation
- Claims recoveries from reinsurers
- Reinsurance receivables tracking
- Treaty statement and reconciliation

---

### 5.5 Premium Collection & Receipting

Manages premium invoicing, payment collection, and reconciliation.

- Premium invoice generation on policy inception and renewal
- Payment plan management (instalments)
- EcoCash, bank transfer, card payment recording
- Premium receipt issuance (ZIMRA fiscal compliant)
- Overdue premium follow-up workflow
- Premium-in-suspense management (payment received, cover not yet confirmed)
- Commission calculation and payment to brokers/agents

---

### 5.6 Agency & Broker Management

Manages the intermediary distribution channel — brokers, agents, and bancassurance partners.

- Broker and agent profile management
- License and appointment tracking (IPEC registration)
- Commission schedule configuration per product
- Commission statement generation (monthly)
- Production report by intermediary
- Binding authority management (what can intermediary write directly?)

---

### 5.7 Regulatory & Compliance Reporting

Generates IPEC-required statutory returns and internal compliance reports.

- IPEC quarterly return preparation
- Solvency margin calculation
- Premium register and claims register export
- Insurance fund statement
- ZIMRA withholding tax on premiums report
- Audit trail of all policy and claims events

**Standards:** IPEC Insurance and Pensions Commission · IFRS 17 (Insurance Contracts)

---

> **Case Study — ZIB Flutter Mobile App (Pindah Build)**  
> Zimbabwe Insurance Brokers Limited (ZIB) required a mobile app integrated with their Nexus Broking System via REST API. Built in Flutter with Riverpod state management, the app gives clients real-time policy views, claims notifications, and document downloads — modelled on a McKinsey Insights-inspired aesthetic adapted to ZIB's brand palette. This is a direct example of a mobile-first insurance CRM layer on top of a core policy administration system.

---

## 6. Accounting

> **Scope:** Standalone accounting for businesses not requiring a full ERP — general ledger, AR, AP, bank reconciliation, tax, and financial reporting.

Standalone accounting software is the entry point for most SMEs before they need a full ERP. World benchmarks: **QuickBooks**, **Xero**, **Sage**, **Pastel**. For Zimbabwe: ZiG/USD multi-currency, ZIMRA VAT return automation, and ZIMDEF/NSSA statutory return generation are essential.

**Standards:** IFRS for SMEs · ZIMRA VAT Act · Income Tax Act Ch. 23:06

---

### 6.1 Chart of Accounts & General Ledger

- Configurable chart of accounts (income, expense, asset, liability, equity)
- Journal entry with approval and reversal
- Multi-currency posting with exchange gain/loss computation
- Period closing controls
- Inter-entity / branch reporting

---

### 6.2 Bank Reconciliation

- Bank statement import (CSV, MT940)
- Auto-matching of bank transactions to GL entries
- Unreconciled items report
- Outstanding cheque and deposit tracking
- EcoCash merchant statement reconciliation

---

### 6.3 VAT Management

- VAT registration management (standard rate, zero-rated, exempt)
- Input and output tax tracking
- VAT return (VAT7) preparation and export
- Reverse charge VAT for imported services
- ZIMRA e-filing integration

**Standards:** ZIMRA VAT Act

---

### 6.4 Payroll (Accounting-Integrated)

- Employee salary and wage computation
- PAYE, NSSA, ZIMDEF, AIDS Levy deduction
- Payslip generation
- GL journal posting of payroll totals
- P2 / NSSA Form generation

---

### 6.5 Financial Reporting

- Income Statement, Balance Sheet, Cash Flow Statement
- Trial Balance and aged debtors/creditors
- Management accounts pack (monthly)
- IFRS for SMEs financial statement templates
- Audit file export (standard audit file format)

---

### 6.6 Cash Flow Management

- 13-week cash flow forecast
- Cash position dashboard
- Payment due date alerts (AP and loan repayments)
- Foreign currency (USD) cash flow segregation

---

## 7. Logistics & Fleet Management

> **Scope:** Delivery order management, route optimisation, driver tracking, vehicle maintenance, and proof of delivery.

Logistics software coordinates the movement of goods from origin to destination — scheduling trips, tracking drivers in real time, and capturing proof of delivery. World benchmarks: **Trimble**, **Samsara**, **Oracle Transportation Management**, **Bringg**. For Zimbabwe: OSRM-based local routing (Google Maps API cost), EcoCash driver advances, and Android foreground service background location tracking are relevant technical constraints.

**Standards:** OSHA (Driver Safety) · ISO 39001 (Road Traffic Safety)

---

### 7.1 Delivery Order Management

- Delivery order creation from sales orders or manual entry
- Customer address geocoding and map verification
- Delivery scheduling by date and time window
- Load planning: weight, volume, and vehicle capacity constraints
- Multi-stop consolidated delivery run planning
- Priority and SLA-based sequencing

---

### 7.2 Route Optimisation

- Automatic route calculation (OSRM or Google Maps API)
- Multi-stop route sequencing to minimise distance/time
- Traffic and road condition awareness
- Driver-facing turn-by-turn navigation (mobile app)
- Alternative route suggestions on road blockage
- Route history vs planned route comparison

---

### 7.3 Real-Time Driver Tracking

- Live GPS location on dispatch dashboard map
- Android foreground service for continuous background location (OEM battery optimisation bypass)
- iOS background location mode (significant location change + background fetch)
- Geofence alerts: driver arrived at delivery point, departed depot
- Speed violation alerts (configurable threshold)
- Tracking history replay

---

### 7.4 Proof of Delivery (POD)

- Driver captures recipient signature on mobile app
- Photo capture at delivery (condition of goods)
- Timestamp and GPS-stamped POD record
- Failed delivery reason capture (not home, wrong address, goods refused)
- Customer-facing delivery notification (SMS / WhatsApp)
- POD document attached to sales order and invoice

---

### 7.5 Vehicle Fleet Management

- Vehicle register: make, model, year, registration, VIN
- Vehicle assignment to drivers (daily / trip-based)
- Fuel consumption tracking (fill-ups vs km travelled)
- Vehicle inspection checklists (pre-trip and post-trip)
- Service and maintenance schedule per vehicle
- Licence, fitness, insurance renewal alerts
- Vehicle utilisation reporting (idle %, km per day)

---

### 7.6 Driver Management

- Driver profile: licence, PDP (professional driving permit), medical
- Driver scheduling and shift management
- Trip assignment and acceptance workflow (mobile app)
- Driver performance scoring (on-time delivery %, incidents, fuel efficiency)
- Driver advance (cash) and expense claim management
- EcoCash driver advance disbursement

---

### 7.7 Logistics Analytics

- On-time delivery rate (OTD)
- Average delivery time by route / zone
- Cost per delivery (fuel + driver time)
- Vehicle utilisation and downtime report
- Customer delivery satisfaction tracking
- Failed delivery analysis by reason code

---

> **Case Study — Trinity Pharmacy Delivery App (Pindah Pitch)**  
> Trinity Pharmacy operates 18 branches across Zimbabwe with no synchronised delivery system. Pindah pitched a delivery logistics app using OSRM routing, animated driver markers, and EcoCash driver advance disbursement — positioning it as the entry point to larger Dispensing and Inventory module contracts. The demo (PindahRx) was built in vanilla JS with OSRM and served as the proof-of-concept for stakeholder buy-in.

---

## 8. Human Resources (HR) & Payroll

> **Scope:** Employee lifecycle management — recruitment, onboarding, performance, leave, payroll, and statutory compliance.

HR software manages everything from job advertisement to retirement. World benchmarks: **Workday**, **SAP SuccessFactors**, **BambooHR**, **Sage HR**. Zimbabwe-specific requirements: PAYE, NSSA, ZIMDEF, AIDS Levy, Labour Act Ch.28:01, and the National Employment Councils (NEC) for various industries.

**Standards:** Labour Act Ch.28:01 · NSSA Act · Income Tax Act · ZIMDEF Act · NEC Codes

---

### 8.1 Recruitment & Applicant Tracking

- Job requisition and approval workflow
- Job advertisement publishing (internal portal, LinkedIn, jobs board)
- Online application form
- Applicant tracking through stages: Applied → Screened → Interviewed → Offered → Hired
- Interview scheduling and feedback capture
- Offer letter generation
- Background check integration
- Rejection notification workflow

---

### 8.2 Employee Onboarding

- Digital onboarding checklist (documents to submit, accounts to create, training to complete)
- Contract of employment generation from template
- Employee profile creation across all linked systems (HR, payroll, ERP user account)
- Equipment assignment log (laptop, phone, access card)
- Induction training tracking
- Probation review scheduling and sign-off

---

### 8.3 Employee Records Management

- Personal profile: demographics, contacts, qualifications, NID
- Employment history (positions held, promotions, transfers)
- Disciplinary record and grievance log
- Document library per employee (contracts, warnings, performance reviews)
- Organisational chart (reporting lines)
- Headcount and staff strength reports

---

### 8.4 Leave Management

- Leave type configuration: annual, sick, maternity, paternity, study, compassionate
- Leave balance tracking and annual accrual
- Online leave application and multi-level approval
- Leave calendar (team leave visibility)
- Leave encashment computation
- Public holiday management (Zimbabwe gazetted holidays)
- NEC leave entitlement compliance check

**Standards:** Labour Act minimum leave entitlements

---

### 8.5 Payroll Processing

- Salary and wage computation per employee
- Allowances: housing, transport, fuel, phone
- Overtime calculation (time and a half, double time)
- Statutory deductions: PAYE (progressive tax table), NSSA (3.5% employee + 3.5% employer), ZIMDEF (1% of payroll), AIDS Levy (3% of PAYE)
- Non-statutory deductions: pension, medical aid, loan repayments, savings club
- Multi-currency payroll (USD / ZiG)
- Payslip generation (PDF and emailed to employee)
- Bulk bank upload file (CBZ, Stanbic, FBC format)
- ZIMRA P2 tax deduction return
- NSSA monthly return (NEC Form)
- ZIMDEF monthly return

**Standards:** Income Tax Act · NSSA Act · ZIMDEF Act

---

### 8.6 Performance Management

- KPI / goal setting per employee (aligned to company scorecard)
- Mid-year and annual performance review cycle
- 360-degree feedback (peers, subordinates, supervisor)
- Balanced Scorecard integration (strategic objectives → individual KPIs)
- Performance rating distribution analysis
- Automatic merit increment calculation based on rating
- Performance improvement plan (PIP) workflow
- Succession planning (high-potential employee identification)

---

### 8.7 Training & Development (LMS Lite)

- Training needs analysis from performance reviews
- Training calendar and attendance tracking
- Certification and qualification expiry alerts
- Training cost tracking per employee
- SCORM-compatible e-learning module hosting (basic)
- Training ROI reporting

**Standards:** SCORM 2004

---

### 8.8 HR Analytics & Reporting

- Headcount by department, grade, and location
- Labour cost by department vs budget
- Turnover rate and exit reason analysis
- Leave liability valuation (for financial statement accrual)
- Gender and diversity metrics
- Payroll variance report (month-on-month)
- Statutory compliance calendar (P2 due dates, NSSA return deadlines)

---

> **Case Study — IPC Consultants (Pindah Build)**  
> Leeroy built a Balanced Scorecard application, psychometric testing software, and surveys platform for IPC Consultants — a direct predecessor to the performance management and HR analytics modules now being incorporated into Pindah Basa's HR suite.

---

## 9. Hospital / Clinic Management

> **Scope:** Patient registration, clinical records, pharmacy, billing, theatre management, and health reporting.

Healthcare management software digitises the clinical and administrative operations of hospitals and clinics. World benchmarks: **Epic**, **Cerner (Oracle Health)**, **Meditech**, **OpenMRS** (open-source). Zimbabwe-specific: Ministry of Health (MoHCC) DHIS2 reporting, Medical Aid Society (CIMAS, PSMAS, Premier) billing, ZiG/USD fee structures, and rural clinic connectivity constraints.

**Standards:** ICD-10 (Disease Classification) · HL7 FHIR · ISO 13606 · WHO DHIS2 · Health Professions Authority (HPA)

---

### 9.1 Patient Registration

- Patient demographics capture (name, DOB, NID, address, next of kin)
- Patient ID / hospital number generation
- Medical aid membership verification
- Previous visit history lookup
- Walk-in, appointment, and referred patient workflows
- Biometric patient identification (fingerprint) option
- Patient consent form capture

---

### 9.2 Outpatient / OPD Management

- Appointment scheduling by doctor and clinic type
- Patient queue management (triage, waiting room display)
- Consultation notes capture (SOAP format: Subjective, Objective, Assessment, Plan)
- Diagnosis coding (ICD-10)
- Prescription generation linked to pharmacy
- Referral letter generation (to specialist or hospital)
- Vitals recording (BP, temperature, weight, oxygen saturation)
- Chronic disease management programme enrolment

---

### 9.3 Inpatient / Ward Management

- Bed assignment and ward map
- Admission and discharge documentation
- Doctor's order entry (medications, investigations, procedures)
- Nursing notes and observation charts
- Ward round documentation
- Discharge summary generation
- Patient transfer between wards
- Occupancy and bed utilisation reporting

---

### 9.4 Electronic Medical Records (EMR)

- Longitudinal patient health record
- Problem list, allergy list, medication history
- Lab and radiology result filing
- Clinical document scanning and attachment
- Surgical history and anaesthetic records
- Vaccination and immunisation records
- Patient summary view for emergency care

**Standards:** ISO 13606 · HL7 FHIR

---

### 9.5 Laboratory Management (LIS)

- Test order creation from clinical encounter
- Sample collection and labelling
- Result entry by technician
- Reference range checking and critical value alerts
- Result release and notification to requesting doctor
- Lab workload and turnaround time reporting
- External lab order management (send-out tests)

---

### 9.6 Radiology & Imaging (RIS)

- Radiology order registration
- Appointment scheduling for radiology procedures
- Image study worklist for radiographer
- Radiologist report entry and authorisation
- PACS (Picture Archiving and Communication System) integration
- Report notification to requesting doctor

---

### 9.7 Pharmacy Management

- Prescription receiving from OPD, ward, or casualty
- Drug dispensing and verification
- Drug inventory with expiry date management (FEFO)
- Narcotic and controlled substance log (MoHCC requirement)
- Drug interaction checking
- Formulary and restricted drug management
- Pharmacy stock reorder linked to procurement module

---

### 9.8 Theatre / Operating Suite Management

- Theatre booking and scheduling
- Pre-operative checklist (WHO Surgical Safety Checklist)
- Anaesthesia record capture
- Intraoperative notes
- Implant and prosthesis tracking
- Theatre utilisation reporting
- Surgical consent form management

---

### 9.9 Billing & Medical Aid Claims

- Patient invoice generation (consultation, ward, pharmacy, lab, theatre)
- Medical aid tariff schedule (CIMAS, PSMAS, Premier, NICOZ Diamond)
- Medical aid claim submission (electronic)
- Co-payment and excess calculation
- Patient statement and receipt
- Medical aid remittance reconciliation
- ZIMRA VAT application (medical services VAT-exempt / zero-rated)
- Bad debt management for self-pay patients

---

### 9.10 Health Information & Reporting

- Ministry of Health DHIS2 data reporting (outpatient attendances, diagnoses, maternal health indicators)
- Notifiable disease reporting (cholera, typhoid, COVID-19)
- Mortality and morbidity statistics
- Hospital-acquired infection (HAI) tracking
- Quality indicator dashboard (readmission rate, average length of stay, bed occupancy)
- HPA (Health Professions Authority) reporting compliance

**Standards:** WHO DHIS2 · ICD-10 · HPA Act

---

> **Reference System — OpenMRS in Zimbabwe**  
> OpenMRS (open-source EMR) has been deployed in multiple MoHCC primary healthcare clinics in Zimbabwe, integrated with DHIS2 for national health reporting. The architecture lesson for Pindah: HL7 FHIR APIs enable interoperability with government systems, making compliance a competitive advantage rather than a burden.

---

## 10. Document Management System (DMS)

> **Scope:** Document capture, classification, version control, workflow routing, search, and retention management.

A DMS replaces physical filing and email-chain-based document management. It provides a single searchable repository with version control, approval workflows, and retention policies. World benchmarks: **Microsoft SharePoint**, **OpenText**, **DocuWare**, **Laserfiche**, **Alfresco** (open-source). Pindah's DMS aspirations include a free/open-source PDF signing pipeline (PdfSharp + BouncyCastle) and Quartz.NET-based background document processing.

**Standards:** ISO 9001 (Document Control) · ISO 27001 · ISO 15489 (Records Management) · GDPR principles

---

### 10.1 Document Capture & Ingestion

- Scan-to-document (TWAIN scanner integration)
- Email-to-document (designated inboxes auto-file attachments)
- Mobile camera document capture
- Drag-and-drop web upload
- Bulk import from shared network drives
- OCR (Optical Character Recognition) for scanned documents — makes text searchable
- Barcode/QR code recognition for auto-classification

---

### 10.2 Document Classification & Metadata

- Folder and category hierarchy (configurable per organisation)
- Mandatory metadata fields (document type, date, author, department, reference number)
- Automatic metadata extraction from OCR (invoice number, date, supplier)
- Custom tag/label system
- Document sensitivity classification (public, internal, confidential, restricted)

---

### 10.3 Version Control

- Full version history on every document
- Check-out / check-in mechanism (prevents simultaneous edit)
- Version comparison (diff view)
- Rollback to previous version
- Version comment / change description
- Draft vs published status management

---

### 10.4 Approval & Workflow Routing

- Configurable document approval workflows (e.g. contract → legal review → MD sign-off)
- Sequential and parallel approval paths
- Deadline and escalation rules
- Digital signature capture (Pindah: PdfSharp + BouncyCastle + signature_pad)
- Audit trail of every approval action (who approved, when, from what IP)
- Bulk document approval for high-volume processes (e.g. payroll payslips)

---

### 10.5 Search & Retrieval

- Full-text search across document content (OCR-indexed)
- Metadata filter search (date range, type, author, department)
- Saved search queries
- Recent documents and favourites
- Relevance-ranked results
- Boolean search operators (AND, OR, NOT)

---

### 10.6 Access Control & Security

- Role-based access: who can view, edit, approve, delete documents
- Folder-level and document-level permissions
- Watermarking of sensitive documents on view/print
- Download restrictions (view-only mode)
- Audit log: every view, download, edit, share action
- Encryption at rest and in transit

**Standards:** ISO 27001

---

### 10.7 Records Retention & Disposal

- Retention schedule configuration (7-year ZIMRA, 5-year employment records, etc.)
- Automated retention expiry alerts
- Legal hold workflow (freezes disposal for litigation)
- Secure disposal workflow with approval and log
- Destruction certificate generation

**Standards:** ISO 15489 · Companies Act (Zimbabwe) document retention requirements

---

### 10.8 Collaboration & Sharing

- Internal link sharing with expiry
- External share link with password protection
- Document annotation (comments, highlights) without modifying the original
- Co-authoring (real-time edit for MS Office documents via WebDAV)
- Integration with email (attach from DMS directly)

---

### 10.9 Background Document Processing

- Queue-based document processing (Quartz.NET job scheduler for Pindah)
- Background OCR processing for large scan batches
- Automated PDF splitting and renaming
- Conversion pipeline: Word/Excel → PDF on upload
- Thumbnail and preview generation for all uploaded files

---

> **Pindah Implementation Note**  
> Pindah has built a C# PDF splitter using PdfSharp with a ReportLab-generated test PDF branded for Frame. The free/open-source digital signature pipeline (PdfSharp + BouncyCastle + signature_pad) with QuestPDF as the generation layer is a foundational DMS component ready for productisation.

---

## 11. Construction Management

> **Scope:** Project tendering, contract management, site management, subcontractor control, progress billing, and cost control.

Construction management software controls the delivery of construction projects — from bidding through to final account. World benchmarks: **Procore**, **Autodesk Construction Cloud**, **Viewpoint Vista**, **Aconex**. Zimbabwe-specific: IDBZ (Infrastructure Development Bank of Zimbabwe) compliance, NSSA site safety requirements, ZITF procurement, and USD/ZiG contract values.

**Standards:** IAS 11 (Construction Contracts) · FIDIC Contract Conditions · NSSA Act · PMI PMBOK · ISO 9001

---

### 11.1 Tendering & Bid Management

- Bid opportunity registration from IDBZ, local authorities, private clients
- Tender document library (bills of quantities, drawings, specifications)
- Estimating: material takeoff from BOQ, labour rates, plant costs
- Tender submission tracking (submission deadline, tender bond)
- Bid/no-bid decision workflow
- Post-tender negotiation and clarification log
- Tender outcome tracking (won, lost, pending)

---

### 11.2 Contract Management

- Contract setup: client, scope, value, duration, contract type (lump sum, re-measure, cost-plus)
- FIDIC or NEC contract clause library
- Variation order (VO) management: submission, approval, valuation, incorporation
- Contractual correspondence log (letters, notices, claims)
- Dispute register and resolution tracking
- Contract close-out checklist
- Practical completion and defects liability period management

---

### 11.3 Project Planning & Scheduling

- Work breakdown structure (WBS) creation
- Gantt chart scheduling (linked activities, critical path)
- Resource loading (labour, plant, materials) per activity
- Baseline schedule and progress tracking (S-curve)
- Look-ahead programme (2-week and 4-week rolling plan)
- Delay analysis and extension of time (EOT) documentation
- Milestone tracking and completion certificates

**Standards:** PMI PMBOK · CPM Scheduling

---

### 11.4 Site Management

- Daily site diary (weather, resources on site, work done, visitors)
- Site instruction log
- Request for Information (RFI) register
- Non-conformance report (NCR) on site
- Site inspection and quality sign-off
- Material delivery records
- Site meeting minutes with action tracking
- Photo log (date, location, description tagged)

---

### 11.5 Subcontractor Management

- Subcontractor registration and prequalification vetting
- Subcontract award and purchase order generation
- Subcontractor progress measurement
- Subcontractor invoice verification against measured work
- Retention deduction and retention release management
- Subcontractor performance evaluation

---

### 11.6 Cost Control & Job Costing

- Budget setup from tender estimate (cost codes by work section)
- Committed cost tracking (purchase orders + subcontract orders)
- Actual cost recording (supplier invoices, payroll)
- Cost-to-complete (CTC) forecast
- Cost variance analysis (budget vs committed vs actual vs forecast)
- Earned value management (EVM): SPI, CPI, BCWP vs BCWS vs ACWP
- Monthly cost report for client and management

**Standards:** IAS 11 · PMI EVM

---

### 11.7 Progress Billing & Valuations

- Monthly progress valuation preparation (measured work + materials on site + variations)
- Application for payment (AFP) generation
- Client certificate processing and payment tracking
- Retention deduction and cumulative retention schedule
- Final account preparation
- Cash flow forecast (billings vs costs)

---

### 11.8 NSSA / Safety & Health Management

- Site safety plan registration
- Hazard identification and risk assessment (HIRA) log
- Safety induction records for all site workers
- Near-miss and incident reporting
- Accident investigation report (NSSA LD3 form)
- NSSA site inspection compliance tracking
- PPE issuance register

**Standards:** NSSA Act · OSHA · ISO 45001

---

> **Case Study — Infrastructure Development Bank of Zimbabwe (IDBZ)**  
> IDBZ-funded projects require contractors to submit detailed monthly progress reports, cost certificates, and procurement compliance records. Construction management software that auto-generates IDBZ-format progress reports and maintains a complete audit trail of variations, payments, and correspondence provides contractors with a measurable tendering advantage.

---

## 12. Supply Chain Management (SCM)

> **Scope:** Demand planning, supplier management, procurement optimisation, inventory strategy, and logistics coordination across the extended supply chain.

SCM software optimises the flow of goods, information, and money from raw material origin to end customer — across multiple organisations. World benchmarks: **SAP Integrated Business Planning**, **Oracle SCM Cloud**, **Blue Yonder**, **Kinaxis**. Zimbabwe context: import/export currency controls (RBZ regulations), cross-border logistics via Beit Bridge and Chirundu, and EcoCash supplier payment integration.

**Standards:** APICS SCOR Model · ISO 28000 (Supply Chain Security) · Incoterms 2020 · RBZ Exchange Control Regulations

---

### 12.1 Demand Planning

- Historical sales analysis and statistical demand forecasting (moving average, exponential smoothing)
- Seasonal demand pattern detection
- Promotion and event uplift planning
- Collaborative demand planning (customer-vendor managed inventory)
- Forecast accuracy tracking (MAPE, bias)
- Demand sensing (using real-time POS data for short-range forecast)

---

### 12.2 Supplier Relationship Management (SRM)

- Supplier master data and risk profile
- Supplier prequalification and onboarding workflow
- Supplier scorecard: on-time delivery, quality, price competitiveness, financial stability
- Preferred supplier list management
- Supplier development programme tracking
- Supplier audit and certification management (ISO 9001, GMP, HACCP)
- Supplier portal (suppliers submit invoices, view PO status)

---

### 12.3 Strategic Procurement

- Category management (direct, indirect, services spend analysis)
- Spend analysis dashboard (who are we buying from, what, at what price?)
- Sourcing strategy: single vs dual vs multi-source
- Long-term supply agreement (LTA) management
- Total cost of ownership (TCO) analysis (not just unit price)
- Import cost model: CIF price + duties + clearing + inland transport
- Currency hedging requirements (RBZ-compliant USD vs ZiG purchasing)

---

### 12.4 Inventory Optimisation

- ABC analysis (A = high value, B = medium, C = low — apply different control policies)
- Safety stock and reorder point calculation (based on lead time variability and service level targets)
- Economic Order Quantity (EOQ) optimisation
- Slow-moving and dead stock identification
- Multi-echelon inventory optimisation (central warehouse → branch network)
- Vendor Managed Inventory (VMI) programme management
- Consignment stock management

---

### 12.5 Warehouse Management (WMS)

- Receiving dock scheduling and goods-in inspection
- Put-away rules and warehouse slotting optimisation
- Wave picking and batch picking for order fulfilment
- FEFO (First Expiry First Out) / FIFO picking enforcement
- Pick, pack, and ship workflow
- Returns (reverse logistics) processing
- Warehouse labour productivity tracking
- RF scanner / mobile device integration for paperless warehouse

---

### 12.6 Transport & Logistics Coordination

- Freight mode selection (road, rail, air, sea)
- Carrier management and rate contracts
- Load planning and groupage consolidation
- Shipment tracking (container tracking, consignment note status)
- Customs documentation: Bill of Lading, Commercial Invoice, Packing List, Certificate of Origin
- Import duty and freight cost accrual
- ZIMRA import declaration (Form 21) status tracking
- Border clearance tracking (Beit Bridge / Chirundu)

**Standards:** Incoterms 2020 · ZIMRA Customs and Excise Act

---

### 12.7 Supply Chain Risk Management

- Supply chain risk register (supplier concentration, geographic risk, currency risk)
- Disruption scenario modelling (key supplier failure, border closure, currency devaluation)
- Business continuity plan for critical materials
- Lead time monitoring and early warning alerts
- Alternative supplier activation workflow

**Standards:** ISO 28000

---

### 12.8 SCM Analytics & Control Tower

- End-to-end supply chain visibility dashboard (orders → stock → in-transit → delivery)
- Fill rate and service level tracking
- Perfect Order Index (on-time, in-full, error-free)
- Supply chain cost-to-serve by customer / channel
- Supplier on-time-in-full (OTIF) dashboard
- Inventory turns and days-of-inventory-on-hand
- Exception management: late shipments, stockouts, excess stock

**Standards:** APICS SCOR Model KPIs

---

> **Case Study — Cloud Migration at Zimbabwe Insurance Brokers (Pindah Portfolio)**  
> Pindah's cloud migration project for ZIB demonstrated how digitising a fragmented, paper-heavy operation (insurance document processing, client record management) onto a centralised cloud platform transforms supply chain information flow. The same principle applies to SCM: replacing WhatsApp-based procurement communication with a structured supplier portal and purchase order workflow eliminates the information gaps that drive procurement inefficiency.

---

## Reference Standards Summary

| Vertical | Key Standards |
|---|---|
| ERP | IFRS, IAS 1/2/16/36, ZIMRA SI.104, ISO 27001 |
| CRM | ISO 10002, POTRAZ, GDPR principles |
| School Management | ZIMSEC, MoPSE, Labour Act Ch.28:01, NSSA |
| Manufacturing | ISO 9001, WHO GMP, MCAZ, IAS 2, ISO 55001 |
| Insurance | IPEC, IFRS 17, IAS 4 |
| Accounting | IFRS for SMEs, ZIMRA VAT Act, Income Tax Act |
| Logistics | OSHA, ISO 39001 |
| HR & Payroll | Labour Act Ch.28:01, NSSA Act, ZIMDEF Act, PAYE |
| Hospital / Clinic | ICD-10, HL7 FHIR, WHO DHIS2, HPA Act |
| Document Management | ISO 9001, ISO 27001, ISO 15489 |
| Construction | IAS 11, FIDIC, NSSA, PMI PMBOK, ISO 45001 |
| Supply Chain | APICS SCOR, ISO 28000, Incoterms 2020 |

---

*Compiled by Pindah Private Limited · Stand 18057, Phase 4, Damofalls, Ruwa · admin@pindah.org · +263 774 454 447*

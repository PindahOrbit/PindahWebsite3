# School Management (Frame)

## Executive Summary

**Frame** is the school management module in Operations. It is built for Zimbabwean primary and secondary schools that need one system for learners, teachers, marks, attendance, report cards, and digital learning — without juggling spreadsheets, separate accounting tools, and disconnected communication apps.

Frame supports the way Zimbabwe schools are actually organised: **ECD and Grades 1–7** in primary; **Forms 1–6** in secondary (including O-Level and A-Level streams); **classrooms** under each form or grade; **terms** and **assessment periods**; weighted continuous assessment; and **official-style report cards** with subject scores, class position, class-teacher remarks, and headmaster comments. Schools running **ZIMSEC**, **Cambridge**, **HEXCO**, or internal grading schemes can configure grading scales and map them to streams (Sciences, Arts, Commercials, and others).

From a single workspace, administrators manage enrolment and class placement; teachers create assessments, capture marks, and maintain attendance registers; the academic office generates and publishes report cards in bulk; students sign in to view results, complete online assessments, and access e-learning materials; and leadership monitors performance on a live dashboard filtered by period, stage, class, and subject.

Frame is not an island. It sits inside the wider **Operations** platform, where your organisation can activate additional modules on the same login, shared data, and one sidebar — no duplicate student lists, no exporting marks to a separate fees system, and no WhatsApp-only parent communication.

Most schools start with **Frame** for academics, then add modules as needs grow: **HR & Payroll** for teachers and payroll, **Accounting** for fees and tuck-shop sales, **CRM** for parent follow-ups and admissions, **Logistics** for buses and trips, **Documents** and **Forms** for policies and applications, **Inventory** for uniforms and stationery, **Clinic** for sick bay, and more. See [Expand Your Platform](#expand-your-platform) for recommended bundles and [How Frame Connects to Other Modules](#how-frame-connects-to-other-modules) for integration detail.

---

## Who This Is For

| Role | How they use Frame |
|------|---------------------|
| **Headmaster / Principal** | Dashboard overview, approve report card comments, review attendance trends |
| **Deputy Head (Academic)** | Period setup, report card generation, grading policy, sync marks across forms |
| **Registrar / Bursar** | Student records, class lists, export data, fees linkage via Accounting (if used) |
| **Class teacher** | Daily attendance, class marks, class-teacher remarks on reports |
| **Subject teacher** | Assessments, question papers, mark entry, e-learning uploads |
| **Student** | Student dashboard, online assessments, e-learning, personal performance view |
| **Parent / Guardian** | Indirect access via CRM, printed reports, or future parent portal (organisation-dependent) |
| **IT / System administrator** | Stages, subjects, grading scales, user accounts, module activation |

---

## Zimbabwe School Context

### Primary schools

Typical structure Frame supports:

| Level | Common naming in Zimbabwe | Notes in Frame |
|-------|---------------------------|----------------|
| Early childhood | ECD A, ECD B | Modelled as **stages** with classrooms underneath |
| Primary grades | Grade 1 – Grade 7 | Each grade is a stage; classes may be 1A, 1B, 2A, etc. |
| Assessment | Continuous assessment, end-of-term tests | **Assessments** linked to subjects and periods |
| Reporting | Term report cards | **Report Cards** with remarks and overall position |

Primary schools often need simpler grading (letter grades or competency bands). Use **Grading System** to define percentage ranges and labels appropriate for your phase (for example *Excellent*, *Very Good*, *Good*, *Needs Improvement*).

### Secondary schools

Typical structure:

| Level | Common naming | Notes in Frame |
|-------|---------------|----------------|
| O-Level | Form 1 – Form 4 | Stages with streams such as Sciences, Arts, Commercials |
| A-Level | Lower 6, Upper 6 (Form 5–6) | Separate stages or classrooms per stream |
| Examinations | ZIMSEC, Cambridge, HEXCO | Configure **grading scales** to match your examination board’s grade boundaries |
| Reporting | Term reports with position in class | **Sync Marks**, **Refresh Grades**, **Generate Remarks**, PDF export |

Secondary schools usually run more assessments per term (tests, assignments, practicals) with **weighted contributions** to the final report card mark. Frame’s report generator lets you select multiple assessments per subject and assign weights (for example Test 1 = 30%, Examination = 70%).

### Terms and periods

Most Zimbabwe schools operate on **three terms** per year. In Operations, academic time is controlled through **Organisation → Periods** (and related calendar settings). Frame assessments and report cards are filtered by **period** so Term 1 marks never mix with Term 2.

**Best practice:** Create periods named clearly (for example `2026 – Term 1`, `2026 – Term 2`) with correct start and end dates before teachers enter marks or the office generates reports.

### Streams and classes

| Concept | Meaning | In Frame |
|---------|---------|----------|
| **Stage / Form / Grade** | Year level (e.g. Form 3) | **Stages** tree under **Frame → Grading → Stages** |
| **Classroom** | Class unit (e.g. 3 Sciences) | Child node under a stage |
| **Stream** | Subject grouping (Sciences, Arts) | Often the stage name or a dedicated stream group used in grading assignment |
| **Subject** | Teaching subject (Maths, English, Shona, etc.) | **Frame → Subjects** |

---

## Key Capabilities

- **School dashboard** — enrolment, assessment completion, attendance trends, performance charts; filter by period, stage, class, and subject
- **Student register** — create, update, import-style workflows; assign form and classroom; student login (`stf` username prefix)
- **Staff** — links to **HR** user list for teachers and administrators
- **Stages and classrooms** — hierarchical tree (add stage, add classroom, dynamic groups)
- **Subjects** — maintain subject catalogue for assessments and reports
- **Assessments** — create papers, attach questions, filter by stage and period, enter marks, student online writing
- **Student mark sheets** — grid view of marks/grades per period with Excel export and assessment weight configuration
- **Grading system** — percentage buckets (A, B, C…), assign scales to streams
- **Report cards** — generate in bulk, sync marks from assessments, refresh grades, auto-remarks, PDF per student or merged class PDF
- **Attendance registers** — daily registers, import templates, mark-all-present, attendance reports
- **E-learning** — reading materials and take-assessment flow for students
- **Student profile** — profile, assessments, e-learning, settings, performance (role-based)
- **Organisation branding** — report card template and official stamps on PDFs

---

## Important Concepts

| Term | Meaning |
|------|---------|
| **Frame** | Product name shown in the menu for the school module |
| **Stage** | Top-level academic year band (Grade 4, Form 2, Lower 6) |
| **Classroom** | Class group within a stage (4A, 2 Commercial) |
| **Period** | Academic term or reporting window from Organisation settings |
| **Assessment** | A marked activity (test, exam, assignment) tied to subject, stage, and dates |
| **Sync Marks** | Pull latest assessment scores into generated report cards |
| **Refresh Grades** | Recalculate letter grades from current grading scale |
| **Stream** | Group of learners sharing a grading scale (often aligned to Sciences/Arts/Commercials) |
| **Mark sheet** | Tabular view of all students and subjects for mark entry and review |
| **Student role** | Login role that sees student dashboard and profile, not admin menus |

---

## Table of Contents

1. [Getting Started](#getting-started)
2. [School Dashboard](#school-dashboard)
3. [Students](#students)
4. [Staff](#staff)
5. [Stages, Classrooms, and Subjects](#stages-classrooms-and-subjects)
6. [Assessments and Marking](#assessments-and-marking)
7. [Grading System](#grading-system)
8. [Report Cards](#report-cards)
9. [Attendance](#attendance)
10. [E-Learning](#e-learning)
11. [Student and Parent Access](#student-and-parent-access)
12. [Expand Your Platform](#expand-your-platform)
13. [How Frame Connects to Other Modules](#how-frame-connects-to-other-modules)
14. [Implementation Guide for Zimbabwe Schools](#implementation-guide-for-zimbabwe-schools)
15. [Daily Routines and Best Practices](#daily-routines-and-best-practices)
16. [Troubleshooting Common Situations](#troubleshooting-common-situations)

---

## Getting Started

### Opening Frame

1. Sign in to Operations.
2. Open **Frame** in the sidebar (available when the school module is activated for your organisation).
3. You land on the **Dashboard** — academic overview for leadership and teachers with admin access.

![Screenshot: Frame sidebar and dashboard entry — placeholder]

### First-time setup checklist

Before going live with a full term, complete this with your administrator:

| Step | Where | Why it matters |
|------|--------|----------------|
| 1 | **Organisation → Periods** | Terms must exist with correct dates |
| 2 | **Frame → Grading → Stages** | Build Grade 1–7 or Form 1–6 structure and classrooms |
| 3 | **Frame → Subjects** | All teaching subjects used in assessments |
| 4 | **Frame → Grading → Grading System** | Grade boundaries per stream (ZIMSEC/Cambridge/internal) |
| 5 | **HR → Users / Staff** | Teacher accounts with correct roles |
| 6 | **Frame → Students** | Enrol learners and assign form/class |
| 7 | **Organisation → School settings** | Report card template and official stamps (if used) |
| 8 | **Frame → Attendance** | Create registers per class or cohort |

---

## School Dashboard

Open **Frame → Dashboard** for the operational picture of the school.

### What you see

The dashboard typically includes:

- **Summary counts** — students, assessments, completion rates
- **Performance trend** — average scores over time (line chart)
- **Score distribution** — how learners are spread across grade bands
- **Subject breakdown** — comparative performance by subject
- **Gender / demographic breakdowns** — where data is captured on student profiles
- **Attendance trend** — presence patterns over the selected period
- **Quick links** — jump to students, assessments, reports, attendance

### Filters

Use the banner filters to narrow the view:

- **Period** — Term 1, Term 2, etc.
- **Stage** — Form 3, Grade 5, etc.
- **Classroom** — 3 Sciences, 5A, etc.
- **Subject** — Mathematics, English, etc.

Leadership can compare a single class against the whole school for a given term before parent meetings or staff briefings.

![Screenshot: School Command Center dashboard with filters and charts](https://storage.pindah.org/IMAGES/screenshots/frame-dashboard.PNG)

### Student dashboard (learners)

Students with the **Student** role see **Frame → Student Dashboard** instead of the admin dashboard. It focuses on:

- Their current form/class
- Completed vs pending assessments
- Recent scores and subject trends
- Upcoming assessments

![Screenshot: Student dashboard — placeholder]

---

## Students

### Student register

Go to **Frame → Students** to manage the learner roll.

**List view**

- Search by name, email, or related fields
- Open a student to edit or view summary
- Create new students from **Add Student**

**Creating a student**

1. Click **Add Student** (or open an existing record).
2. Enter **First name**, **Last name**, and **Username** (must use the `stf` prefix — the system can auto-add it).
3. Optionally add **Email**, **Phone**, **Form/Stage**, and **Classroom**.
4. Complete any **custom fields** your school configured (birth certificate number, parent contact, medical notes, etc.).
5. Save.

Assigning **Form/Stage** and **Classroom** correctly is essential — assessments, attendance registers, and report cards all depend on class membership.

![Screenshot: Students list — placeholder]

![Screenshot: Create student form with stage and classroom — placeholder]

### Student summary

**Frame → Students → Summary** (or open a student’s summary) gives a consolidated view of a learner’s academic record — useful for disciplinary meetings, transfers, or parent conferences.

![Screenshot: Student summary — placeholder]

---

## Staff

Teachers and school administrators are managed under **Frame → Staff**, which opens the shared **HR → Users** list filtered for school use.

| Task | Where |
|------|--------|
| Add a new teacher | HR user creation (with appropriate role) |
| Assign subjects/classes | User groups, timetables (HR), and assessment ownership |
| Deactivate leavers | HR user status |

Frame does not duplicate HR — it **reuses** the organisation’s user directory so payroll, permissions, and performance modules stay aligned.

![Screenshot: Staff list via HR — placeholder]

---

## Stages, Classrooms, and Subjects

### Stages tree

**Frame → Grading → Stages** defines your school structure.

**Example — Primary**

```
Primary
├── ECD A
│   ├── ECD A Green
│   └── ECD A Blue
├── Grade 1
│   ├── 1A
│   └── 1B
...
└── Grade 7
    ├── 7A
    └── 7B
```

**Example — Secondary**

```
Secondary
├── Form 1
│   ├── 1 Sciences
│   ├── 1 Arts
│   └── 1 Commercials
├── Form 2
...
└── Upper 6
    └── 6 Sciences
```

**Actions**

- **Add Stage** — new form or grade level
- **Add Classroom** — class under a stage
- **Dynamic** flag — for groups that change membership automatically (advanced)
- **Save** per row — names update immediately in dropdowns across the system

![Screenshot: Stages and classrooms tree — placeholder]

### Subjects

**Frame → Subjects** (also under **Grading → Subjects**) lists subjects taught in the school: Mathematics, English Language, Shona, Ndebele, Combined Science, Geography, Accounts, etc.

Every **assessment** and **report card line** links to a subject. Keep naming consistent with your report card template (ZIMSEC subject names vs internal short names).

![Screenshot: Subjects list — placeholder]

---

## Assessments and Marking

### Assessment list

**Frame → Assessments** is where teachers and heads of department manage tests and assignments.

**Filters**

- **Stage / Grade** — only assessments for that cohort
- **Period** — assessments whose dates fall in the term window
- **Search** — find by name or subject

**Actions menu** typically includes bulk operations and navigation to mark sheets (organisation configuration may vary).

![Screenshot: Assessments list with stage and period filters — placeholder]

### Creating an assessment

1. Click **Add Assessment**.
2. Set **Name** (e.g. “Form 3 Mathematics – Term 1 Test”).
3. Choose **Subject**, **Stage**, and date range aligned to the **period**.
4. Define maximum marks and assessment type as required.
5. Save, then open **Questions** to build the paper or upload content.

### Questions and online writing

**Frame → Assessments → [Assessment] → Questions**

- Add structured questions for online delivery
- Students with access open **Write Assessment** from their assessment list
- Submissions flow back for marking

This supports schools moving past paper-only exams toward controlled online tests (computer lab or BYOD policies).

![Screenshot: Assessment question editor — placeholder]

![Screenshot: Student online assessment writing screen — placeholder]

### Entering marks

Marks can be entered:

- Within the assessment detail screens
- Via **Student Mark Sheets** (**Frame → Reports → User Scores** or linked from Assessments)

### Student mark sheets

**Student Mark Sheets** provide a spreadsheet-style grid:

- Select **Period**
- Toggle **Marks** vs **Grades** view
- **Configure assessments** — choose which tests count and set **weights**
- **Download Excel** for offline editing or archival

Use this for:

- Departmental mark schedule meetings
- Comparing class performance before report cards
- External submission to examination boards (export)

![Screenshot: Student mark sheet grid — placeholder]

---

## Grading System

**Frame → Grading → Grading System** defines how numeric marks become grades on reports.

### Grading groups

Each **group** (scale) includes:

- **Name** — e.g. “ZIMSEC O-Level”, “Internal Primary”
- **Score range** — min/max percentage for the scale
- **Buckets** — ranges mapped to labels (A, B, C, D, U or 1–9, etc.)

**Example buckets (illustrative)**

| Percentage | Grade |
|------------|-------|
| 70 – 100 | A |
| 60 – 69 | B |
| 50 – 59 | C |
| 40 – 49 | D |
| 0 – 39 | U |

### Assign to streams

Each grading group can be **assigned to streams** (Form 3 Sciences, Form 4 Arts, etc.) so report cards use the correct boundaries for that cohort.

When examination boards change grade thresholds, update buckets once and run **Refresh Grades** on report cards.

![Screenshot: Grading system cards with buckets and stream assignment — placeholder]

---

## Report Cards

**Frame → Reports** (Report Cards) is the academic office hub for end-of-term publishing.

### Report card list

Reports are grouped by **Period → Stage → Class** for easy bulk operations.

Columns typically include:

- Student name
- Level (form/grade)
- Class
- Period
- Overall score
- Class position
- Created date

**Filters:** search, period, stage, class.

![Screenshot: Report cards grouped list — placeholder]

### Generating report cards

1. Click **Generate Reports**.
2. Select **Stream/Grade** (required).
3. Select **Period**.
4. Choose **Assessments** — tick each test/assignment to include; set **weight %** per assessment (must reflect school policy).
5. Optional: enable **Test Mode** — generate for **one student** first to verify layout and marks.
6. Click **Generate**.

The generator combines weighted assessment marks into subject scores and overall performance.

### After generation — office workflow

| Action | Purpose |
|--------|---------|
| **Sync Marks** | Pull latest marks from assessments into existing reports |
| **Refresh Grades** | Recalculate grades after grading scale changes |
| **Generate Remarks** | Auto-fill remark text where missing |
| **View / Download PDF** | Single student PDF |
| **Download Class PDF** | Merged PDF for every learner in a class — printing for distribution |
| **Bulk select + Delete** | Remove incorrect batch |

### Editing comments

Open a report in the **detail drawer** to edit:

- **Position of responsibility** (Head Boy, Prefect, etc.)
- **Co-curricular activities** (sport, choir, debate)
- **Class teacher comment**
- **Headmaster comment**

Save comments before printing final PDFs for parents.

Official **stamps** and **report card templates** come from **Organisation** school settings (template ID, uploaded stamp images).

![Screenshot: Generate report cards modal with weights — placeholder]

![Screenshot: Report card PDF preview — placeholder]

---

## Attendance

### Daily registers

**Frame → Attendance Registers**

1. Select a **register** (linked to a class or group).
2. Mark each learner **Present**, **Absent**, **Late**, or as configured.
3. Save the register for the day.

**Actions**

- **Import Registers** — bulk create register definitions from CSV/Excel template
- **Import Attendance Sheet** — upload filled attendance for a date range
- **Mark All Present** — shortcut for days when full attendance is confirmed
- **Reports** — attendance analysis export

Zimbabwe schools often maintain statutory attendance records — digital registers reduce lost books and support audits.

![Screenshot: Attendance register — placeholder]

### Attendance report

**Frame → Attendance Report** summarises presence over time for leadership and truancy follow-up.

![Screenshot: Attendance report — placeholder]

---

## E-Learning

Frame includes a lightweight learning layer for schools investing in digital content.

| Area | Path | Purpose |
|------|------|---------|
| **E-Learning dashboard** | Frame → E-Learning | Overview and navigation |
| **Reading materials** | E-Learning → Reading | Documents and resources per subject/topic |
| **Take assessment** | E-Learning → Take / student flow | Linked assessments for self-paced work |

Students also reach materials from **My Profile → E-Learning**.

**Use cases in Zimbabwe**

- Share revision notes before ZIMSEC examinations
- Host homework reading during load-shedding (download when connected)
- Flip classroom content for Sciences and Mathematics

![Screenshot: E-learning dashboard — placeholder]

---

## Student and Parent Access

### Student profile (logged-in learner)

**Frame → Student Profile** (or **My Profile**) sections:

| Tab | Content |
|-----|---------|
| **Profile** | Personal details |
| **Assessments** | List, scores, links to online writing |
| **E-Learning** | Assigned materials |
| **Performance** | Charts and summaries |
| **Settings** | Password and preferences |

### Authentication

Students receive usernames with the **`stf`** prefix and organisation-managed passwords. Role **Student** restricts menus to learner-safe screens only.

### Parents and guardians

There is no separate “parent” product menu in core Frame today. Common patterns:

- **Printed report cards** and SMS via external process
- **CRM** module for structured parent communication and follow-ups
- **Forms** module for feedback surveys and admission enquiries
- Future parent portal (organisation-specific projects)

---

## Expand Your Platform

Frame covers academics — marks, attendance, report cards, and e-learning. Most Zimbabwe schools also run **fees**, **payroll**, **transport**, a **tuck shop**, **parent communication**, and sometimes a **sick bay** or **boarding** operation. Operations lets you activate those capabilities as **additional modules** on the same platform: one sign-in for staff, shared student and staff records, and a single operational picture for leadership.

You do not need every module on day one. Schools typically activate Frame first, then add modules when a pain point appears — for example fee arrears (Accounting + CRM), bus safety (Logistics), or uniform sales (Inventory + Accounting POS).

### Modules schools commonly add

| Module | What it adds for your school | Typical trigger |
|--------|------------------------------|-----------------|
| **[HR & Payroll](hr-payroll.md)** | Teacher accounts, roles, onboarding, payroll, timetables, performance scorecards | Any school with paid staff — Frame staff lists come from HR |
| **[Accounting & Sales](accounting.md)** | Term fees, receipts, fee arrears, tuck-shop and uniform **POS**, multi-currency (USD/ZWL/ZAR) | Bursar tired of parallel spreadsheets; parents want printed receipts |
| **[CRM](crm.md)** | Parent contacts, SMS/email history, fee reminders, admission enquiries from prospect to enrolled | Registrar wants a proper parent register and follow-up trail |
| **[Logistics](logistics.md)** | School bus routes, trip planning, driver assignments, live map **Command Center** | Transport officer or boarding school with daily routes |
| **[Inventory & Stock](inventory-stock.md)** | Stationery, textbooks, uniforms, tuck-shop stock, reorder alerts | School shop or bookroom needs stock control tied to sales |
| **[Documents](documents.md)** | Prospectuses, policies, schemes of work, circulars — versioned, searchable | Leadership wants one file library instead of email attachments |
| **[Forms & Surveys](forms.md)** | Online admission applications, parent satisfaction surveys, staff feedback | Open day season or end-of-term feedback collection |
| **[Healthcare (Clinic)](clinic-healthcare.md)** | Sick-bay records, allergies, visit history linked to learners | Day school sick bay or boarding school health centre |
| **[Asset Management](assets.md)** | Laptops, projectors, lab kits — assignment and maintenance per department | IT or estates team tracking devices across forms |
| **[Chat](chat.md)** | Official staff channels — exam period, sports, department coordination | Reduce unofficial WhatsApp groups for school business |
| **[Analyst](analyst.md)** | Cross-module exports and deeper reports for ministry returns and board papers | Academic office or bursar preparing term-end packs |
| **[Insurance](insurance.md)** | Student accident cover and group policies brokered through the school | Schools offering optional cover at enrolment |
| **[Projects](projects.md)** | Non-academic projects — building works, events, IT rollouts | Admin team already managing capitation projects |
| **[Mobile Scanner](mobile-scanner.md)** | Barcode/QR for library books, assets, or shop stock | Library or store wants fast scanning on a phone |
| **[Agriculture & Feed](agriculture.md)** | Farm units, feed, and livestock for agricultural colleges or rural boarding schools | School runs a teaching farm or college farm block |
| **[Pharmacy](pharmacy.md)** | Dispensary workflow if the school operates a licensed medical centre | Rare — usually colleges or schools with on-site clinic pharmacy |
| **[Dashboard](dashboard.md)** | Executive KPIs across Frame, fees, HR, and other active modules | Head and board want one leadership view beyond Frame |

**Organisation** is the foundation for all of the above — academic **periods**, report branding, module activation, and roles. It is configured for every school deployment even when only Frame is live.

→ See [Organisation](organisation.md) and [All Modules](all-modules.md)

### Module previews

Screenshots below are taken from each module’s own product documentation — a preview of what your school can activate alongside Frame. Modules without a screenshot here do not yet have a published image in their guide.

**HR & Payroll** — teacher accounts, payroll, and performance management:

![Screenshot: HR dashboard full view](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(53).png)

**Accounting & Sales** — term fee invoices and tuck-shop POS:

![Screenshot: Invoices list](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(25).png)

![Screenshot: Point of sale full screen](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(23).png)

**Logistics** — school bus routes, trip planning, and live Command Center map:

![Screenshot: Logistics dashboard](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(47).png)

![Screenshot: Command centre map](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(52).png)

**Inventory & Stock** — uniforms, stationery, and tuck-shop stock control:

![Screenshot: Inventory dashboard full view](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(7).png)

**Pharmacy** — dispensary Command Center (for schools with a licensed on-site pharmacy; Clinic sick-bay screenshots are not yet published):

![Screenshot: Pharmacy Command Center full view](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(34).png)

**Executive Dashboard** — leadership KPIs across all active modules:

![Screenshot: Main dashboard home](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(6).png)

*CRM, Documents, Forms, Assets, Chat, Analyst, Insurance, Projects, Mobile Scanner, and Agriculture screenshots are not yet published in their module guides.*

### Suggested bundles (activate in phases)

| School profile | Suggested modules beyond Frame |
|----------------|------------------------------|
| **Primary day school** | HR, Accounting (fees), CRM (parents), Documents |
| **Secondary day school** | Above + Forms (admissions), Analyst (reports) |
| **School with transport** | Above + Logistics |
| **School with tuck shop / uniforms** | Above + Inventory, Accounting POS |
| **Boarding school** | Above + Clinic, Asset Management, Chat |
| **Agricultural college** | Frame + HR + Agriculture + Inventory + Accounting |

**Phase 4** of the [implementation guide](#implementation-guide-for-zimbabwe-schools) maps to turning on these modules after academics are stable.

Contact your **Operations implementer** or account team to activate modules — sidebar menus and permissions update once a module is enabled for your organisation.

---

## How Frame Connects to Other Modules

Frame is designed to sit inside the wider **Operations** platform. The sections below explain **how** each module connects in practice — useful after you decide which capabilities to add from [Expand Your Platform](#expand-your-platform).

### Organisation

- **Academic periods** (terms) drive assessments and report cards
- **School settings** — report card template, official stamp images on PDFs
- **Branding** — logo and header on printed reports
- **Custom fields** on students — national ID, parent phone, sponsorship, etc.

→ See [Organisation](organisation.md)

### HR & Payroll

- **Staff records** — all teachers are HR users
- **Roles and permissions** — who can create assessments, approve reports, edit marks
- **Timetables and shifts** — class scheduling (where configured)
- **Performance management** — link teacher KPIs to school strategic goals

![Screenshot: HR dashboard full view](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(53).png)

→ See [HR & Payroll](hr-payroll.md)

### Accounting & Sales

- **School fees** — invoice families per term (where the school uses Operations finance)
- **Receipts** — track payments against fee invoices
- **Multi-currency** — USD, ZWL, ZAR fee collection with exchange rates
- **POS** — bookshop or uniform sales at school tuck shop

![Screenshot: Invoices list](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(25).png)

![Screenshot: Point of sale full screen](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(23).png)

→ See [Accounting & Sales](accounting.md)

### CRM (Customer Relationship Management)

- **Parents as contacts** — phone, email, communication history
- **Follow-ups** — fee reminders, meeting schedules
- **Pipeline** — admission enquiries from prospect to enrolled student

→ See [CRM](crm.md)

### Documents

- Store **prospectuses**, **policies**, **staff handbooks**, **scheme of work**
- Versioned files accessible to leadership without email attachments

→ See [Documents](documents.md)

### Forms & Surveys

- **Admission application** forms
- **Parent satisfaction** surveys after term
- **Staff feedback** forms

→ See [Forms & Surveys](forms.md)

### Logistics

- **School bus routes** and trip monitoring
- **Driver assignments** for field trips and sports fixtures
- Emergency and GPS features where the school uses fleet modules

![Screenshot: Logistics dashboard](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(47).png)

![Screenshot: Command centre map](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(52).png)

→ See [Logistics](logistics.md)

### Projects

Assessments are backed by the platform **Projects** engine internally — advanced schools already using **Projects** for non-academic work can align project management culture with IT administration.

→ See [Projects](projects.md)

### Inventory & Stock

- **Stationery** and **textbook** stock at school shop
- **Uniform** inventory if sold on campus
- Stock-take and reorder like any retail operation

![Screenshot: Inventory dashboard full view](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(7).png)

→ See [Inventory & Stock](inventory-stock.md)

### Healthcare (Clinic)

- **School clinic** patient records for sick bay
- Allergies and conditions visible if the learner is also a clinic patient
- Useful for boarding schools and large day schools

→ See [Healthcare (Clinic)](clinic-healthcare.md)

### Pharmacy

Generally for dispensary operations — relevant only if the school runs a medical centre with licensed dispensing.

![Screenshot: Pharmacy Command Center full view](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(34).png)

→ See [Pharmacy](pharmacy.md)

### Insurance

- **Student accident cover** or **group policies** where the school brokers insurance

→ See [Insurance](insurance.md)

### Asset Management

- Track **laptops**, **projectors**, **lab equipment** assigned to departments
- Maintenance schedules for school assets

→ See [Asset Management](assets.md)

### Chat

- Staff coordination — department channels, exam period announcements
- Reduces WhatsApp fragmentation for official school communication

→ See [Chat](chat.md)

### Mobile Scanner

- Barcode/QR scanning for **library books**, **assets**, or **inventory** in the school store

→ See [Mobile Scanner](mobile-scanner.md)

### Analyst

- Deeper reporting and exports across modules for ministry returns or board papers

→ See [Analyst](analyst.md)

### Core Platform, Authentication, Dashboard

- **Single sign-on**, permissions, audit trails
- Executive **Dashboard** may include school KPIs when modules are combined

![Screenshot: Main dashboard home](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(6).png)

→ See [Core Platform](core-platform.md), [Authentication & Security](authentication.md), [Dashboard](dashboard.md)

### Agriculture, Insurance, and specialised modules

Rural boarding schools or agricultural colleges may additionally use **Agriculture & Feed** for farm units — contact your implementer for cross-module setup.

→ See [All Modules](all-modules.md)

---

## Implementation Guide for Zimbabwe Schools

### Suggested rollout phases

**Phase 1 — Foundation (Week 1–2)**

- Organisation periods for current year
- Stages and classrooms mirror your physical classes
- Subjects match your timetable
- Import or enter students
- Create teacher accounts in HR

**Phase 2 — Daily operations (Week 3–4)**

- Attendance registers live in every class
- Pilot assessments in two subjects per form
- Train class teachers on mark entry

**Phase 3 — Reporting (Week 5–6)**

- Configure grading scales per stream (ZIMSEC/Cambridge/internal)
- Test report generation in **Test Mode** for one learner
- Full cohort report cards; PDF distribution to parents

**Phase 4 — Integration (Ongoing)**

- Review [Expand Your Platform](#expand-your-platform) and activate the next modules (fees, parents, transport, shop, clinic, etc.)
- Fees in Accounting (if applicable)
- CRM for parents
- Logistics for transport
- E-learning content upload

### Naming conventions (recommended)

| Item | Convention |
|------|------------|
| Periods | `2026 – Term 1` (include year) |
| Stages | `Form 3`, `Grade 5`, `Lower 6` |
| Classrooms | `3 Sciences`, `5A` |
| Assessments | `Form 3 Maths – T1 Test 1` |
| Usernames | `stf` + unique suffix (e.g. `stfjmoyo`) |

### ZIMSEC vs Cambridge on one campus

Many Zimbabwe schools run both curricula on one campus:

1. Create **separate grading groups** — “ZIMSEC O-Level” and “Cambridge IGCSE”.
2. Assign each group to the correct **streams**.
3. Generate report cards **per stream** so grades on PDFs match the examination system parents expect.

---

## Daily Routines and Best Practices

### Start of term (registrar / academic office)

1. Confirm **period dates** in Organisation.
2. Verify **stages and classrooms** match physical class lists.
3. Enrol new students; archive leavers in HR.
4. Create **attendance registers** for each class.
5. Communicate assessment naming standards to HODs.

### Weekly (teachers)

1. Complete **attendance** for every teaching day.
2. Enter marks within **48 hours** of each test (school policy).
3. Upload **e-learning** material for the week’s topics.

### End of term (academic office)

1. Confirm all assessments are **closed** and marks entered.
2. Run **Sync Marks** on draft report cards.
3. **Refresh Grades** after any grading scale change.
4. Class teachers enter **remarks**; headmaster approves.
5. **Download Class PDFs** for printing or parent digital distribution.
6. Archive Excel mark sheets for examination board records.

### Data protection

- Limit **Student** role permissions — learners see only their own data.
- Use HR role assignments — avoid sharing admin passwords.
- Export reports only on secure devices — report cards contain personal information protected under Zimbabwe data practice expectations.

---

## Troubleshooting Common Situations

| Situation | What to do |
|-----------|------------|
| Student not in assessment class list | Check **Form/Stage** and **Classroom** on student profile |
| Report card shows wrong grade | Verify grading group **assigned to stream**; run **Refresh Grades** |
| Missing subject on report | Ensure assessment exists for subject in period; **Sync Marks** |
| Cannot generate reports — no assessments | Select stream and period; confirm assessments have marks entered |
| Username rejected on create | Username must start with **`stf`** prefix |
| Attendance register empty | Select correct register; import registers if new term |
| Student sees admin menus | User must have **Student** role only, not Teacher/Admin |
| PDF missing school stamp | Upload stamps in **Organisation** school settings |
| Marks out of date on report | Run **Sync Marks** after late mark entry |
| Test report looks wrong | Use **Test Mode** with one student before bulk generate |

---

## Related Documentation

- [All Modules](all-modules.md) — full platform map and every module you can add
- [Organisation](organisation.md) — periods, branding, school settings, module activation
- [HR & Payroll](hr-payroll.md) — staff, roles, timetables, payroll
- [Accounting & Sales](accounting.md) — fees, receipts, tuck-shop POS
- [CRM](crm.md) — parent and admission communication
- [Logistics](logistics.md) — school transport and trip monitoring
- [Inventory & Stock](inventory-stock.md) — uniforms, stationery, tuck-shop stock
- [Healthcare (Clinic)](clinic-healthcare.md) — sick bay and learner health records
- [Forms & Surveys](forms.md) — applications and feedback
- [Documents](documents.md) — policies and circulars
- [Asset Management](assets.md) — laptops, lab equipment, maintenance
- [Chat](chat.md) — staff coordination channels
- [Analyst](analyst.md) — cross-module reporting for leadership
- [Core Platform](core-platform.md) — permissions and configuration

# Pharmacy

## Executive Summary

Pharmacy is the dispensary operations centre of Operations. It is designed for pharmacies, hospital dispensaries, and retail medicine outlets that need to serve patients safely, maintain an accurate audit trail, and stay connected to the wider business — inventory, patient records, and payments — without switching between separate systems.

From a single workspace, staff can dispense medicines to walk-in patients, work through a verification queue, manage scheduled refills, maintain patient profiles with allergy alerts, browse medicine stock with batch and expiry visibility, define compound mixture recipes, and configure how dosing instructions appear on printed labels. Every dispense reduces stock using a first-expiry-first-out approach so older batches are used before newer ones, and optional payment collection links each transaction to your sales and accounting records.

Pharmacy shares patient records with the Healthcare (Clinic) module: a patient registered in the clinic appears automatically in the pharmacy patient search, complete with allergies and chronic conditions. Products dispensed are drawn from the same inventory catalogue used in Stock and Accounting, which means stock levels, pricing, and batch tracking stay consistent across the organisation.

The Pharmacy Command Center dashboard gives managers a live picture of today’s activity — dispenses completed, refills overdue, items awaiting pharmacist verification, stock running low, and batches approaching expiry — so problems are visible before they affect patient care.

---

## Who This Is For

| Role | How they use Pharmacy |
|------|------------------------|
| **Pharmacists** | Verify prescriptions, dispense medicines, manage dosing labels, approve queue items |
| **Dispensary assistants** | Process walk-in sales, print labels, collect payments, manage the dispense queue |
| **Pharmacy managers** | Monitor dashboard metrics, review reports, oversee refill compliance and stock alerts |
| **Clinical staff** | Register patients (via Clinic) whose records flow into pharmacy dispensing |
| **Finance teams** | Reconcile dispensary sales recorded through the payment step after each dispense |

---

## Key Capabilities

- Pharmacy Command Center dashboard with operational KPIs and charts
- Quick Dispense workstation (patient search, line items, dosing, refills, payment, labels)
- Dispense queue with table and kanban views (Dispensed → Verified → Collected → Paid)
- Dispensing records — full searchable audit history
- Pending refills worklist with process, reschedule, and cancel actions
- Pharmacy patient register with profile drawer (history, active medications, allergies)
- Pharmacy products view linked to inventory stock, pricing, and batch expiry
- Mixture recipe library for compounded preparations
- Dosage cypher settings for standard frequency codes (OD, BD, TDS, PRN, etc.)
- Five operational reports: Dispensing Summary, Product Movement, Refill Compliance, Expiry Tracking, Patient History
- PDF dispense documents and printable medicine labels
- Optional point-of-sale payment after each dispense

---

## Important Concepts

Before using Pharmacy day to day, it helps to understand these terms:

| Term | Meaning |
|------|---------|
| **Dispense** | A completed transaction where one or more medicine lines are issued to a patient. Each dispense is recorded with patient, products, quantities, dosing instructions, and status. |
| **Dispense queue** | A workflow board tracking where each dispense sits in the verification and collection process — not an inbox of clinic prescriptions, but the internal status of pharmacy work. |
| **Refill** | A scheduled repeat of a medicine based on an interval (for example every 28 days). Created when staff tick “Schedule refills” during dispensing. |
| **Dosage cypher** | A short code (such as OD for once daily) that expands to full text on printed labels. |
| **Mixture** | A compound recipe listing ingredient products and quantities. Maintained as a catalogue; individual products are still dispensed separately today. |
| **FEFO** | First Expiry, First Out — the system uses batches with the earliest expiry date first when deducting stock. |
| **Script attachment** | An uploaded or photographed copy of the prescriber’s prescription, attached to the dispense for audit purposes. |

---

## Table of Contents

1. [Getting Started](#getting-started)
2. [Pharmacy Command Center](#pharmacy-command-center)
3. [Quick Dispense — The Main Workstation](#quick-dispense--the-main-workstation)
4. [Dispense Queue](#dispense-queue)
5. [Dispensing Records](#dispensing-records)
6. [Pending Refills](#pending-refills)
7. [Patients](#patients)
8. [Products and Stock Visibility](#products-and-stock-visibility)
9. [Mixtures](#mixtures)
10. [Dosage Cyphers and Label Settings](#dosage-cyphers-and-label-settings)
11. [Payments and Labels](#payments-and-labels)
12. [Reports](#reports)
13. [How Pharmacy Connects to Other Modules](#how-pharmacy-connects-to-other-modules)
14. [Daily Routines and Best Practices](#daily-routines-and-best-practices)
15. [Troubleshooting Common Situations](#troubleshooting-common-situations)

---

## Getting Started

### Opening Pharmacy

1. Sign in to Operations.
2. Open **Pharmacy** in the sidebar (available when the Pharmacy module is activated for your organisation, typically for healthcare-related setups).
3. You land on the **Dashboard** — the Pharmacy Command Center.

![Screenshot: Pharmacy sidebar and dashboard entry — placeholder]

### First-time setup checklist

Before dispensing at volume, confirm the following with your administrator:

1. **Locations and tills** are configured in Stock (each dispense deducts stock from a selected location).
2. **Products** exist in inventory with correct stock levels, costs, and selling prices.
3. **Dosage cyphers** are seeded or configured under **Pharmacy → Settings → Dosage Cyphers**.
4. **Payment methods** are set up in Accounting if you will collect payment at the till.
5. **Patient records** exist (via Clinic or Pharmacy → Patients) for regular customers.

---

## Pharmacy Command Center

The dashboard is your operational nerve centre. Open it from **Pharmacy → Dashboard**.

![Screenshot: Pharmacy Command Center full view](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(34).png)

### What you see

The Command Center typically shows:

- **Today’s dispenses** — count and total value for the current day
- **Pending refills** — how many refills are scheduled or overdue
- **Verification backlog** — dispenses saved but not yet verified by a pharmacist
- **Awaiting collection** — verified dispenses the patient has not yet picked up
- **Low stock alerts** — medicines below reorder level
- **Expiring batches** — stock expiring within the next 90 days
- **Monthly value chart** — dispensing trend over time
- **Status breakdown** — doughnut chart of dispenses by workflow status
- **Recent activity** — latest dispense transactions

### Date and filter controls

Use date presets (Today, This Week, This Month, This Quarter, This Year, All Time) to change the reporting window. Filter by dispense status to focus on, for example, only items on hold.

### Quick actions

From the dashboard you can jump directly to:

- Start a **New Dispense**
- Open the **Dispense Queue**
- Review **Pending Refills**
- Browse **Products** or **Reports**

Refresh the dashboard periodically during busy periods, or rely on the auto-updating queue screen for live workflow status.

![Screenshot: Dashboard KPI cards and charts — placeholder]

---

## Quick Dispense — The Main Workstation

**Quick Dispense** (`Pharmacy → Quick Dispense`) is where most dispensing work happens. The screen is organised in three columns: patient on the left, add-line form in the centre, and the current dispense basket on the right.

![Screenshot: Quick Dispense three-column layout](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(35).png)

### Step 1 — Select location and patient

At the top of the screen, confirm the **stock location** (and linked till) from which stock will be deducted. This is critical — dispensing from the wrong location will affect the wrong branch’s inventory.

**Find a patient:**

1. Click in the patient search box (keyboard shortcut: **P**).
2. Search by name, phone number, or national ID.
3. Select the matching patient from the results.

If no patient exists, click **Add Patient** to register a new one. You will enter first name, last name, ID number, phone, and email. The new patient is immediately available for dispensing.

![Screenshot: Patient search and selection — placeholder]

**After selecting a patient, review:**

- **Allergy alerts** — displayed prominently; never dispense contraindicated medicines without prescriber confirmation
- **Chronic conditions** — context for counselling
- **Pending refills panel** — medicines due for renewal; click **Go** to process a refill directly
- **Dispensing history** — past dispenses; use **Repeat** to copy a previous line

### Step 2 — Add a medicine line

In the centre column:

1. **Product** — search inventory by name or stock code (shortcut: **M**). Only products with available stock appear.
2. **Quantity** — use the stepper or type the amount to dispense.
3. **Dosing — Frequency** — select from dosage cyphers (OD, BD, TDS, etc.) or type in the directions field.
4. **Dosing — Route** — Oral, IV, IM, Topical, and other standard routes.
5. **Directions for label** — free-text instructions printed on the medicine label. Tips:
   - Type a cypher code wrapped in underscores (for example `_BD_`) to expand to full frequency text on the label
   - Type a route wrapped in asterisks (for example `*Oral*`) for route formatting
   - Double-space or use the arrow key to open the cypher picker inline
6. **Schedule refills** (optional) — tick the checkbox, set number of refills (1–12), and interval (7, 14, 28, or 30 days).
7. Click **Add to Dispense**.

![Screenshot: Add line form with dosing fields — placeholder]

**Attach a prescription script (recommended):**

Use the script attachment area to upload or photograph the prescriber’s prescription. The system warns if you attempt to save without an attached script — follow your organisation’s policy on when a script is mandatory.

### Step 3 — Review the dispense basket

The right column lists all lines added:

- Edit quantity or dosing on any line
- Remove a line if added in error
- Review **subtotal**, optional **fees and taxes**, and **total**
- Confirm **currency**

### Step 4 — Save, print, and pay

1. Click **Save & Print** (shortcut: **Ctrl+S**).
   - The dispense is saved with status **Dispensed**
   - Stock is deducted using FEFO batch allocation
   - Refill schedule rows are created if you scheduled refills
   - A dispense PDF downloads automatically
2. The **Process Sale** payment modal opens:
   - Select payment method
   - Enter amount tendered
   - Confirm change due
   - You may skip payment if the patient pays later
3. After payment (or skip), click **Print Labels** to generate medicine label PDFs for each line.

![Screenshot: Save and print with payment modal — placeholder]

![Screenshot: Printed medicine labels — placeholder]

### Editing an existing dispense

Open a dispense from the queue or dispensing records to edit it. The URL includes the dispense ID (`/pharmacy/dispense/:id`). Make corrections, re-save, and re-print as needed.

### Keyboard shortcuts

Press **?** on the dispense screen to view available shortcuts. Common ones include **P** (patient search), **M** (medicine search), and **Ctrl+S** (save).

---

## Dispense Queue

The queue tracks each dispense through your internal workflow. Open **Pharmacy → Dispense Queue**.

![Screenshot: Dispense queue table view](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(36).png)

### Workflow stages

| Stage | Meaning | Typical action |
|-------|---------|----------------|
| **Dispensed** | Saved and stock deducted; awaiting pharmacist check | Pharmacist reviews and verifies |
| **Verified** | Pharmacist has confirmed accuracy | Patient collects medicine |
| **Collected** | Patient has picked up | Payment collected if not yet paid |
| **Paid** | Payment recorded | Complete — no further action |
| **On Hold** | Paused pending clarification | Resolve issue, then release |

### Table view

The default table shows all dispenses with search and status filters (Dispensed, Verified, On Hold). Actions per row:

- **View / Edit** — opens the dispense screen
- **Verify** — moves from Dispensed to Verified (pharmacist sign-off)
- **Hold** — pauses with a reason prompt

The queue auto-refreshes approximately every 30 seconds.

### Kanban view

Switch to kanban for a visual board with columns for each stage. Drag cards between **Dispensed** and **Verified** or **On Hold** columns. Other moves may require using the action buttons rather than drag-and-drop.

![Screenshot: Dispense queue kanban board — placeholder]

### Starting a new dispense from the queue

Click **New Dispense** from the queue banner to open a fresh Quick Dispense session without leaving the queue context.

---

## Dispensing Records

**Pharmacy → Dispensing Records** is the complete audit trail of every dispense ever recorded.

![Screenshot: Dispensing records list](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(37).png)

### Searching and filtering

- **Search** by patient name, dispense reference, or product
- **Filter by status** — Dispensed, Verified, Collected, Paid, On Hold, Cancelled
- **Filter by date range** — narrow to a specific week or month for audits

### Actions

From each record you can:

- **View** — read-only summary
- **Edit** — opens Quick Dispense for corrections
- **Re-print** — download dispense PDF or labels again

Use this screen for regulatory audits, patient queries (“when did I last collect this medicine?”), and internal quality reviews.

---

## Pending Refills

Scheduled refills appear under **Pharmacy → Pending Refills**.

![Screenshot: Pending refills worklist](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(38).png)

### How refills are created

Refills are created automatically when staff tick **Schedule refills** on a dispense line and specify count and interval. The system calculates due dates and lists them here.

### Filter tabs

| Tab | Shows |
|-----|-------|
| **All** | Every open refill |
| **Overdue** | Past due date, not yet dispensed |
| **Due Today** | Due today |
| **Due Soon** | Due within the next 3 days |
| **Scheduled** | Due more than 3 days away |

### Processing a refill

**Option A — From Pending Refills list:**

1. Find the refill row (patient, medicine, quantity, due date).
2. Click **Process Refill**.
3. A new dispense is created pre-filled with the medicine and patient.
4. Verify quantity and dosing, then save and complete as normal.

**Option B — From Quick Dispense patient panel:**

1. Select the patient.
2. In the **Pending Refills** panel, click **Go** next to the due item.
3. Confirm when prompted — the refill is processed through the dispense workflow.

### Rescheduling or cancelling

- **Reschedule** — pick a new due date and enter a reason (patient travelling, prescriber changed dose, etc.)
- **Cancel** — enter a reason; the refill is closed without dispensing

![Screenshot: Refill detail drawer with reschedule — placeholder]

### Refill statuses

| Status | Meaning |
|--------|---------|
| Scheduled | Active, not yet due |
| Overdue | Past due, needs action |
| Dispensed | Completed — linked to a dispense record |

---

## Patients

**Pharmacy → Patients** lists every patient registered for pharmacy use.

![Screenshot: Pharmacy patients list](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(39).png)

### Patient list

Search by name, phone, or ID. The list shows key identifiers at a glance. Click a row to open the **patient profile drawer**.

### Registering a new patient

1. Click **Add Patient**.
2. Complete: first name, last name, national ID (if applicable), phone, email.
3. Save — the patient is immediately searchable in Quick Dispense.

Patients created here are also available in **Clinic → Patients** (and vice versa) because both modules share the same patient register.

### Patient profile drawer

The drawer has several tabs:

| Tab | Content |
|-----|---------|
| **Details** | Contact information, ID, demographics |
| **Allergies & Conditions** | Clinical alerts shown during dispensing |
| **History** | All past dispenses with dates and products |
| **Active Medications** | Current non-cancelled dispense lines |
| **Refills** | Open and completed refill schedules |

From the drawer, click **New Dispense** to start dispensing for this patient immediately.

![Screenshot: Patient profile drawer with history tab — placeholder]

---

## Products and Stock Visibility

**Pharmacy → Products** shows the medicine catalogue as the pharmacy team sees it — linked directly to inventory.

![Screenshot: Pharmacy products list](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(40).png)

### What each product shows

- Product name and stock code (SKU)
- Category
- Current stock quantity at your locations
- Selling price
- Batch details and expiry dates (open the batch drawer)

### Using products during dispensing

Products are searched from Quick Dispense, not usually from this list — but this screen is valuable for:

- Checking stock before promising a patient a medicine
- Identifying batches nearing expiry
- Confirming pricing before dispensing

Low-stock and expiring-batch alerts on the dashboard originate from this inventory data.

![Screenshot: Product batch and expiry drawer — placeholder]

---

## Mixtures

**Pharmacy → Mixtures** maintains a library of compound preparation recipes.

![Screenshot: Mixtures list](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(41).png)

### Creating a mixture recipe

1. Click **Add Mixture**.
2. Enter **Name** (for example “Paediatric Cough Mixture”).
3. Enter **Label text** — instructions printed if this mixture were dispensed as a named compound.
4. Add **Components** — each row is an inventory product with quantity and unit.
5. Save.

### Viewing and editing

Open any mixture in the side drawer to view components, edit quantities, or delete obsolete recipes.

### Important note

Mixtures are maintained as a recipe catalogue. Today, dispensing still uses individual products line by line in Quick Dispense rather than selecting a mixture as a single dispense item. Use mixtures as reference recipes and dispensing guides until full mixture dispensing is integrated.

![Screenshot: Mixture recipe editor with components](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(42).png)

---

## Dosage Cyphers and Label Settings

**Pharmacy → Settings → Dosage Cyphers** manages the short codes used on labels and in the dispense form.

![Screenshot: Dosage cyphers list](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(45).png)

### Standard codes

When first opened, the system may seed common codes:

| Code | Typical meaning |
|------|-----------------|
| OD | Once daily |
| BD | Twice daily |
| TDS | Three times daily |
| QDS | Four times daily |
| PRN | As needed |
| NOCTE | At night |
| OM | Every morning |

### Managing cyphers

- **Add** — create a new code and its full description
- **Edit** — update description text (what expands on the label)
- **Delete** — remove unused codes

You can also open **Manage Codes** from within Quick Dispense without leaving the dispensing screen.

### How cyphers appear on labels

When directions contain `_CODE_`, the label PDF expands the code to its full description. Routes wrapped in `*Route*` format consistently on the printed label.

---

## Payments and Labels

### Payment collection

After **Save & Print**, the **Process Sale** modal connects the dispense to your accounting sales flow:

1. Select **payment method** (cash, card, mobile money, etc.)
2. Enter **amount tendered**
3. Review **change due**
4. Confirm — status moves toward **Paid**
5. Or click **Skip** if payment happens later (patient collects on account)

![Screenshot: Process sale payment modal — placeholder]

### Dispense PDF

A formal dispense document downloads on save — useful for patient records and audit. Re-print from Dispensing Records if needed.

### Medicine labels

Click **Print Labels** after save (or after payment) to generate label PDFs — one per medicine line with patient name, product, quantity, and expanded dosing directions.

![Screenshot: Label print preview — placeholder]

---

## Reports

Open **Pharmacy → Reports** to run operational reports.

![Screenshot: Pharmacy reports screen](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(43).png)

### Available reports

| Report | What it shows | Typical use |
|--------|---------------|-------------|
| **Dispensing Summary** | All dispense transactions in a date range — reference, date, patient, status | Daily/monthly volume audit |
| **Product Movement** | Quantity dispensed and dispense count per product | Identify fast-moving medicines |
| **Refill Compliance** | All refill records — scheduled, overdue, completed | Monitor repeat medication adherence |
| **Expiry Tracking** | Batches expiring within 90 days | Proactive stock rotation and write-offs |
| **Patient History** | Full dispense history for one patient (enter patient ID) | Clinical review or patient enquiry |

### Running a report

1. Select **Report type**.
2. Set **From date** and **To date**.
3. For Patient History, enter the **Patient ID**.
4. Click **Run**.
5. Review the on-screen table and record count.

![Screenshot: Dispensing summary report results](https://storage.pindah.org/IMAGES/screenshots/Screenshot%20(44).png)

---

## How Pharmacy Connects to Other Modules

### Inventory & Stock

- All medicines dispensed come from the **inventory product catalogue**.
- Stock is deducted on save using **FEFO batch allocation** (earliest expiry first).
- **Location and till** selection determines which branch’s stock is affected.
- Low-stock and expiry alerts on the pharmacy dashboard read from inventory batch data.

### Healthcare (Clinic)

- **Patients** are shared — clinic registers patients; pharmacy dispenses to the same records.
- **Allergies and chronic conditions** entered in clinic appear as alerts during dispensing.
- Clinic does not push prescriptions into the pharmacy queue automatically today — attach script images manually or process clinic-generated requests through your local workflow.

### Accounting & Sales

- Each dispense is recorded as a **dispense-type sale**.
- Optional **POS payment** after save posts to sales with payment method and till.
- Finance teams reconcile dispensary revenue through Accounting → Sales.

---

## Daily Routines and Best Practices

### Opening routine (start of shift)

1. Open the **Pharmacy Command Center** — scan KPIs and alerts.
2. Check **Pending Refills** — process overdue items first.
3. Open the **Dispense Queue** — verify any items left from the previous shift.
4. Review **Expiry Tracking** report if batches are flagged on the dashboard.

### During the shift

- Always confirm **patient allergies** before adding lines.
- Attach **prescription scripts** per your organisation’s policy.
- Select the correct **location** before saving — wrong location means wrong stock deduction.
- **Verify** dispenses in the queue before patients collect — pharmacist sign-off is your quality gate.
- Use **Hold** with a clear reason when information is missing — do not guess.

### Closing routine (end of shift)

1. Clear or hand over **On Hold** items with notes for the next shift.
2. Run **Dispensing Summary** for the day.
3. Reconcile **Paid** vs **Collected** statuses in the queue.
4. Check for **low stock** items dispensed heavily today — flag for reordering.

---

## Troubleshooting Common Situations

| Situation | What to do |
|-----------|------------|
| Product not found in search | Check it exists in Stock inventory and has quantity at your selected location |
| Cannot save — script warning | Attach a prescription image or confirm your policy allows saving without one |
| Stock deducted but patient did not collect | Item stays at **Verified** until **Collected** — do not create a duplicate dispense |
| Wrong quantity dispensed | Open the dispense from records, edit, re-save; stock adjusts accordingly |
| Refill shows overdue | Process from Pending Refills or reschedule with reason if prescriber changed treatment |
| Payment skipped but patient paid later | Open dispense in queue, collect payment through Process Sale, move to **Paid** |
| Patient not found | Register via Add Patient; check spelling; try phone or ID search |
| Label directions wrong | Edit the line’s directions field before save; use cyphers for consistent expansion |

![Screenshot: Pharmacy help and support footer — placeholder]

---

## Related Documentation

- [Pharmacy Home Delivery](pharmacy-delivery.md) — home delivery fulfilment with Logistics and the Rider app
- [Inventory & Stock](inventory-stock.md) — product catalogue, locations, batches
- [Healthcare (Clinic)](clinic-healthcare.md) — patient registration and clinical records
- [Accounting & Sales](accounting.md) — payment methods, sales register, reconciliation
- [Logistics](logistics.md) — trips, drivers, vehicles, Command Center

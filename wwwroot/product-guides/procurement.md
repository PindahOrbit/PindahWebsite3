# Procurement

## Executive Summary

Procurement is the **source-to-pay** engine of Operations. It takes your organisation from an internal need to buy something, through approval and supplier selection, to a formal purchase order, physical goods receipt, supplier invoice validation, and payment — with full traceability at every step.

Unlike bolt-on purchasing tools that only handle orders, Operations Procurement is **woven into Inventory and Accounting**. When goods are received, stock levels and product costs update immediately. When supplier invoices are recorded, the system runs **3-way matching** against the purchase order and goods receipt note (GRN) before payment is released. Purchase orders live alongside your financial documents in Accounting, while requisitions, RFQs, invoices, and payments are managed in a dedicated Procurement workspace.

The **Procurement Command Center** dashboard maps the entire lifecycle — from requisition to payment — with actionable KPIs, spend analytics, accounts-payable aging, and one-click shortcuts to the next task in the pipeline.

Whether you run a single-site retailer, a multi-branch distributor, a hospital supply chain, or a centralised corporate procurement office, this module gives finance and operations teams shared visibility and control over what is being bought, from whom, at what price, and whether it has been received and paid for correctly.

---

## Who This Is For

| Role | How they use Procurement |
|------|--------------------------|
| **Department requesters** | Raise purchase requisitions with justification, items, and required-by dates |
| **Managers and approvers** | Review and approve or reject requisitions; monitor the approval queue |
| **Procurement officers** | Convert approved requests to RFQs or POs; manage supplier sourcing |
| **Warehouse and receiving staff** | Record goods received against suppliers and locations (GRN) |
| **Accounts payable clerks** | Capture supplier invoices, link PO and GRN, review match results |
| **Finance managers** | Approve invoices for payment, monitor AP aging and spend, configure thresholds |
| **Business owners** | Command Center dashboard — pipeline health, spend by supplier, overdue POs |

---

## Key Capabilities

- Procurement Command Center with lifecycle workflow map and actionable KPIs
- Purchase requisitions (PR) with draft, approval, and conversion workflow
- Multi-step approval queue with configurable amount thresholds
- Request for quotation (RFQ) with multi-supplier invites and side-by-side quote comparison
- Purchase order creation from approved requisitions, RFQ awards, or direct entry
- Goods receipt (GRN) integrated with stock posting, batch tracking, and optional PO link
- Supplier invoices with automatic 3-way match (PO + GRN + invoice)
- Variance tolerance for near-matches within configurable percentage
- Fiscal receipt verification gate for regulated environments
- Supplier payment recording against approved invoices
- Organisation-wide procurement settings — auto-approve limits, RFQ rules, prefixes
- Spend by supplier charts, requisition pipeline breakdown, and AP aging buckets
- Full integration with supplier master data, currencies, payment methods, and banks

---

## Important Concepts

| Term | Meaning |
|------|---------|
| **Purchase Requisition (PR)** | An internal request to buy goods or services before any commitment to a supplier. |
| **RFQ** | Request for Quotation — sent to multiple suppliers to compare prices before awarding an order. |
| **Purchase Order (PO)** | A formal order to a supplier specifying items, quantities, and agreed prices. |
| **GRN (Goods Receipt Note)** | A stock receipt record proving goods were physically received at a location. |
| **3-way match** | Validation that supplier invoice totals align with the linked PO and/or GRN before payment. |
| **Match status** | `matched` (exact), `variance` (within or over tolerance), or `unmatched` (missing links). |
| **Variance tolerance** | Allowed percentage difference between invoice total and PO/GRN reference total. |
| **Auto-approve below** | Requisitions at or below a set amount skip the approval queue on submit. |
| **Fiscal verification** | Optional requirement to record a fiscal receipt number before invoice approval. |
| **Source-to-pay** | The full chain from internal request through to supplier payment. |

---

## Table of Contents

1. [Getting Started](#getting-started)
2. [Procurement Command Center](#procurement-command-center)
3. [Purchase Requisitions](#purchase-requisitions)
4. [Approvals](#approvals)
5. [Request for Quotation (RFQ)](#request-for-quotation-rfq)
6. [Purchase Orders](#purchase-orders)
7. [Goods Receipt (GRN)](#goods-receipt-grn)
8. [Supplier Invoices and 3-Way Match](#supplier-invoices-and-3-way-match)
9. [Supplier Payments](#supplier-payments)
10. [Procurement Settings](#procurement-settings)
11. [End-to-End Workflow](#end-to-end-workflow)
12. [Roles and Permissions](#roles-and-permissions)
13. [How Procurement Connects to Other Modules](#how-procurement-connects-to-other-modules)
14. [Setup Guide for New Organisations](#setup-guide-for-new-organisations)
15. [Daily and Monthly Routines](#daily-and-monthly-routines)
16. [Troubleshooting Common Situations](#troubleshooting-common-situations)

---

## Getting Started

1. Open **Procurement** in the sidebar.
2. Land on the **Command Center** dashboard for a lifecycle overview.
3. Complete [Procurement Settings](#procurement-settings) before going live.
4. Ensure **suppliers**, **products**, and **locations** exist in Stock (see [Inventory & Stock](inventory-stock.md)).

![Screenshot: Procurement sidebar — placeholder]

### Prerequisites

Before your first procurement cycle, confirm:

| Master data | Where to set up |
|-------------|-----------------|
| Suppliers | **Stock → Suppliers** |
| Products | **Stock → Inventory** |
| Locations / warehouses | **Stock → Locations** |
| Currencies | **Accounting → Currencies** |
| Payment methods and banks | **Accounting → Payment Methods**, **Banks** |

---

## Procurement Command Center

Open **Procurement → Dashboard** (`/procurement/dashboard`).

The Command Center is designed so that **every next action in the procurement lifecycle is one click away**.

### Lifecycle workflow map

Six stages are displayed as connected cards:

| Stage | What happens | Typical status flow |
|-------|--------------|---------------------|
| **1. Request** | Raise a purchase requisition | Draft → Pending Approval |
| **2. Approve** | Managers review and decide | Approved / Rejected |
| **3. Quote** | Competitive sourcing via RFQ | Draft → Sent → Awarded |
| **4. Order** | Create and send purchase order | Draft → Sent → Accepted |
| **5. Receive** | Record goods into inventory | Received (stock posted) |
| **6. Match & Pay** | Invoice, 3-way match, payment | Match → Approve → Paid |

Each card links to the primary screen for that stage (new requisition, approval queue, RFQ list, PO list, receive stock, supplier invoices).

### Action KPIs

Clickable cards surface work waiting to be done:

| KPI | What it means |
|-----|---------------|
| **PRs Awaiting Approval** | Requisitions submitted and not yet approved |
| **POs to Dispatch** | Purchase orders still in draft — ready to send |
| **GRNs to Post** | Receipts awaiting final posting (where applicable) |
| **Invoices to Match** | Supplier invoices with unmatched or variance status |
| **Overdue POs** | Orders past expected delivery date |
| **Month Spend** | Total supplier invoice value in the current month |

### Summary metrics and charts

- **Open requisitions** and **open POs** — pipeline volume
- **Pending approvals** — items in the approval queue
- **Total AP outstanding** — unpaid supplier liability
- **Requisition pipeline** — donut chart (draft, pending, approved, rejected, converted, cancelled)
- **Spend by supplier** — top suppliers by invoice value
- **AP aging** — current, 1–30, 31–60, 61+ days outstanding
- **Recent requisitions**, **recent POs**, and **pending GRNs** — quick-access tables

### Date range filter

Filter dashboard figures by custom date range or presets: **7 days**, **30 days**, **This month**, or **Quarter**.

### Quick actions

From the dashboard banner:

- **New Requisition**
- **New PO (direct)** — skip requisition when you already know what to order
- **Receive Stock**
- **Refresh**

![Screenshot: Procurement Command Center — placeholder]

---

## Purchase Requisitions

Go to **Procurement → Requisitions** (`/procurement/requisitions`).

Purchase requisitions are how departments formally request purchases **before** money is committed to a supplier.

### Requisition list

Filter by status:

| Status | Meaning |
|--------|---------|
| Draft | Being prepared; not yet submitted |
| Pending Approval | Submitted; awaiting approver |
| Approved | Cleared to convert to PO or RFQ |
| Rejected | Declined with reason |
| Converted | A purchase order was created from this PR |
| Cancelled | Withdrawn |

### Creating a requisition

Go to **Procurement → Requisitions → Create** (`/procurement/requisitions/create`).

1. Select **Supplier** (required before converting to PO).
2. Set **Priority** — low, normal, high, or urgent.
3. Enter **Required by** date.
4. Add **Cost centre** and **Justification** — why this purchase is needed.
5. Add **Line items** — search products, enter quantity and estimated unit cost.
6. Choose an action:
   - **Save as Draft** — continue editing later.
   - **Submit for Approval** — sends to approvers (or auto-approves if below threshold).

**Requisition numbers** are assigned automatically (format `PR-0000001`).

![Screenshot: Create purchase requisition — placeholder]

### After approval

On an approved requisition you can:

| Action | When to use |
|--------|-------------|
| **Convert to PO** | Supplier is known and you want to order directly |
| **Convert to RFQ** | You want competitive quotes from multiple suppliers |

Converting to PO navigates to the purchase order view in Accounting with lines pre-filled from the requisition.

### Viewing and editing

Open any requisition to view status, approval history, line items, and available workflow actions. Only **draft** requisitions can be edited or deleted.

---

## Approvals

Go to **Procurement → Approvals** (`/procurement/approvals`).

The approval queue shows documents awaiting a decision — primarily purchase requisitions, and (where configured) supplier invoices and payments.

### Approving or rejecting

For each pending item:

1. Review document details, amount, and requester justification.
2. Click **Approve** to advance the workflow, or **Reject** and enter a reason.

Requisitions can also be approved directly from the requisition detail screen when status is **Pending Approval**.

### Auto-approval

If **Auto Approve Below** is set in [Procurement Settings](#procurement-settings), requisitions at or below that total amount are approved immediately on submit — no queue entry required. Leave the setting empty to always require manual approval.

### Multi-step approval rules

Organisations can define approval rules by document type and amount band, assigning specific users or roles to each step. Rules are managed through the platform's approval configuration (contact your administrator for setup).

![Screenshot: Approval queue — placeholder]

---

## Request for Quotation (RFQ)

Go to **Procurement → RFQs** (`/procurement/rfqs`).

RFQs support **competitive sourcing** — inviting multiple suppliers to quote on the same requirement before you commit to one.

### When to use an RFQ

- Spend is above your internal RFQ threshold
- No sole-source supplier exists
- You want documented price comparison for audit purposes
- Procurement policy requires multiple quotes

### Creating an RFQ

1. Click **New RFQ**, or convert an **approved requisition** (pre-fills lines).
2. Enter **Title**, **Closing date**, and description.
3. Add **Line items** — products, quantities, and descriptions.
4. Invite **Suppliers** — select two or more vendors to receive the request.
5. Save as **Draft**.

### Sending and comparing quotes

1. Open the RFQ and click **Send to Suppliers** — status becomes **Sent**.
2. Open the **RFQ comparison view** (`/procurement/rfqs/view/:id`).
3. Review the comparison matrix — each supplier column shows quoted unit prices per line; **lowest prices are highlighted**.
4. Click **Award** on the winning supplier column — this creates a **purchase order** using their quoted prices.

**RFQ numbers** follow the format `RFQ-0000001`.

### RFQ statuses

| Status | Meaning |
|--------|---------|
| Draft | Editable; not yet sent |
| Sent | Suppliers invited; awaiting or receiving quotes |
| Awarded | Winning supplier selected; PO created |

![Screenshot: RFQ comparison matrix — placeholder]

---

## Purchase Orders

Purchase orders are managed under **Accounting → Purchase Orders** (`/accounting/purchase-orders`), but they are a core step in the procurement lifecycle.

A PO is the **commercial commitment** to a supplier — what you agreed to buy, at what price, and when you expect delivery.

### Creating a PO

**Three paths:**

| Path | How |
|------|-----|
| From requisition | Approved PR → **Convert to PO** |
| From RFQ | RFQ comparison → **Award** to supplier |
| Direct | **Accounting → Purchase Orders → Create**, or dashboard **New PO (direct)** |

1. Select **Supplier**.
2. Add **Line items** — products, quantities, unit prices.
3. Set **Expected delivery date** and payment terms where applicable.
4. Save as **Draft**.

**PO numbers** are assigned automatically (format `PO-0000001`).

### PO workflow

| Step | Action | Status after |
|------|--------|--------------|
| 1 | Review draft PO | Draft |
| 2 | Click **Send** to supplier | Pending |
| 3 | Supplier confirms | **Accept** → Accepted, or **Reject** → Rejected |
| 4 | Goods arrive | Receive in Stock module |
| 5 | Supplier invoice arrives | Record in Procurement |

From the PO view you can **download PDF**, **send**, **accept**, or **reject**.

![Screenshot: Purchase order view — placeholder]

---

## Goods Receipt (GRN)

Physical receiving happens in **Stock → Receive Stock** (`/stock/orders/receive`).

A goods receipt note (GRN) records that goods have **actually arrived** at a warehouse or store — and posts them into inventory.

### Receiving stock

1. Open **Stock → Orders → Receive Stock**.
2. Select **Supplier** — optionally link a **Purchase Order** for the same supplier.
3. Select **Location** where goods will be stored.
4. Enter **Date received** and optional **Invoice / delivery note number**.
5. Add **Line items** — products, quantities received, and cost prices.
6. For batch-tracked items, enter **batch number**, manufacture date, and expiry.
7. Optional: enable **Auto-calculate selling prices using markup rules** — creates price change requests on save.
8. Click **Receive Stock** — inventory updates immediately; a stock receipt PDF can be downloaded.

### What happens on receipt

When goods are received, Operations:

- Increases stock quantity at the selected location
- Updates product **last purchase cost** and weighted average cost
- Optionally raises **price change requests** for manager approval
- Posts a **stock received** journal entry to Accounting
- Creates an auditable **stock receipt** record linkable to supplier invoices

### Linking to purchase orders

When receiving against a PO, select the purchase order in the supplier card. This links the GRN to the procurement chain for 3-way matching later.

View all receipts at **Stock → Orders** (`/stock/orders`).

![Screenshot: Receive stock screen — placeholder]

---

## Supplier Invoices and 3-Way Match

Go to **Procurement → Supplier Invoices** (`/procurement/supplier-invoices`).

Supplier invoices are your **accounts payable** records — what the supplier is billing you for, tied back to what you ordered and received.

### Invoice list

Filter by status:

| Status | Meaning |
|--------|---------|
| Pending | Recorded; awaiting approval for payment |
| Approved | Cleared to pay |
| Paid | Fully settled |
| Disputed | Under query with supplier |
| Cancelled | Voided |

Filter also by **match status**: matched, variance, or unmatched.

### Creating a supplier invoice

Go to **Procurement → Supplier Invoices → Create**.

1. Enter **Invoice number** from the supplier's document.
2. Select **Supplier**.
3. Link **Purchase Order** and **Goods Receipt (GRN)** — both should reference the same supplier.
4. Set **Invoice date**, **Due date**, and **Currency** (with exchange rate if foreign currency).
5. Add **Line items** — products, quantities, unit prices; total should match the supplier's bill.
6. Save.

On save, the system **automatically runs 3-way matching**.

### 3-way match explained

3-way matching answers: *"Does this invoice match what we ordered and what we received?"*

```
Purchase Order total  ──┐
                        ├── Compare to invoice total
GRN total (qty × cost) ─┘
```

| Match result | Meaning |
|--------------|---------|
| **Matched** | Invoice total equals the PO or GRN reference total |
| **Variance** | Totals differ — within your tolerance % this is flagged but acceptable; over tolerance needs review |
| **Unmatched** | PO and/or GRN not linked, or totals cannot be compared |

A coloured banner on the invoice screen shows the current match status. Each match run is logged for audit.

### Fiscal verification

Where **Require Fiscal Verification** is enabled in settings, invoices must have a **fiscal receipt number** verified before they can be approved — supporting compliance with fiscal device requirements in regulated markets.

### Approving invoices

Once matched (or reviewed), set invoice **Status** to **Approved** to release it for payment. Disputed invoices can be flagged while you resolve differences with the supplier.

![Screenshot: Supplier invoice with 3-way match banner — placeholder]

---

## Supplier Payments

Go to **Procurement → Payments** (`/procurement/payments`).

Payments close the procurement loop — recording money sent to the supplier against an approved invoice.

### Creating a payment

Go to **Procurement → Payments → Create**.

1. Select the **Supplier invoice** (must be approved).
2. Confirm **Supplier** (pre-filled from invoice).
3. Enter **Payment date** and **Amount**.
4. Select **Currency**, **Payment method**, and **Bank**.
5. Enter **Reference** — cheque number, transfer reference, etc.
6. Save.

**Payment numbers** are assigned automatically (format `PAY-0000001`).

When the payment is processed, the linked invoice moves toward **Paid** status.

![Screenshot: Create supplier payment — placeholder]

---

## Procurement Settings

Go to **Procurement → Settings** (`/procurement/settings`).

Configure organisation-wide procurement behaviour before users begin submitting requisitions.

| Setting | Purpose |
|---------|---------|
| **Variance Tolerance %** | How much invoice total can differ from PO/GRN and still match (default 5%) |
| **Auto Approve Below** | PRs at or below this amount skip approval on submit; leave empty to always require approval |
| **Require RFQ Above** | Policy threshold — purchases above this amount should go through RFQ |
| **Min RFQ Suppliers** | Minimum suppliers to invite on an RFQ (policy default: 3) |
| **Default Payment Terms (days)** | Standard payment terms for new orders (default 30) |
| **PO Number Prefix** | Custom prefix for purchase order numbers |
| **PR Number Prefix** | Custom prefix for requisition numbers |
| **Centralised Procurement** | Flag for organisations where all purchasing flows through a central team |
| **Require Fiscal Verification** | Block invoice approval until fiscal receipt is recorded |

Save settings once configured. Review periodically as your procurement policy evolves.

![Screenshot: Procurement settings — placeholder]

---

## End-to-End Workflow

### Standard path (with requisition and approval)

```
Requester creates PR
        ↓
Submit for Approval
        ↓
Approver approves
        ↓
Convert to PO  (or RFQ → Award → PO)
        ↓
Send PO → Supplier accepts
        ↓
Warehouse receives goods (GRN)
        ↓
AP clerk records supplier invoice (links PO + GRN)
        ↓
3-way match runs automatically
        ↓
Finance approves invoice
        ↓
Payment recorded → Invoice paid
```

### Competitive sourcing path (with RFQ)

```
Approved PR → Convert to RFQ
        ↓
Add suppliers → Send RFQ
        ↓
Compare quotes → Award winning supplier
        ↓
PO created automatically
        ↓
(continue: Receive → Invoice → Match → Pay)
```

### Express path (known supplier, no requisition)

```
Direct PO (Accounting) → Send → Accept
        ↓
Receive Stock → Supplier Invoice → Payment
```

Use this for repeat orders, emergency purchases, or organisations that do not require requisitions below a certain value.

### Status reference

| Document | Status progression |
|----------|-------------------|
| **PR** | draft → pending approval → approved → converted |
| **RFQ** | draft → sent → awarded |
| **PO** | Draft → Pending → Accepted (or Rejected) |
| **GRN** | received (stock posted on save) |
| **Invoice** | pending → approved → paid (+ match: unmatched / matched / variance) |
| **Payment** | pending → processed |

---

## Roles and Permissions

Procurement uses role-based permissions. Typical permission groups:

| Area | Permissions |
|------|-------------|
| Dashboard | View Command Center |
| Requisitions | View, create, update, delete, approve |
| RFQs | View, create, update, delete |
| Supplier invoices | View, create, update, delete, approve |
| Payments | View, create, approve |
| Approvals | View queue, approve, reject |
| Settings | View and update procurement configuration |

**Cross-module permissions** needed for the full workflow:

| Activity | Module permission |
|----------|-------------------|
| Create and send POs | Accounting — purchase orders / quotations |
| Receive stock | Stock — receipts |
| Manage suppliers | Stock — suppliers |

### Suggested role split

| Role | Typical access |
|------|----------------|
| **Requester** | Create and submit requisitions |
| **Approver** | Approve requisitions; view dashboard |
| **Procurement officer** | RFQs, PO conversion, supplier management |
| **Warehouse** | Receive stock, view POs |
| **AP clerk** | Supplier invoices, 3-way match review |
| **Finance manager** | Approve invoices and payments; settings |

Your administrator assigns permissions per user in **Organisation → Users & Roles**.

---

## How Procurement Connects to Other Modules

```
                    ┌─────────────────────────────────┐
                    │         PROCUREMENT             │
                    │  PR - RFQ - Invoices - Payments │
                    │  Approvals - 3-Way Match        │
                    └──────────────┬──────────────────┘
                                   │
         ┌─────────────────────────┼─────────────────────────┐
         │                         │                         │
         ▼                         ▼                         ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  INVENTORY/STOCK │    │   ACCOUNTING    │    │  ORGANISATION   │
│  Suppliers       │    │  Purchase Orders│    │  Users - Roles  │
│  Receive - GRN   │    │  Currencies     │    │  Permissions    │
│  Products - Cost │    │  Banks - Journals│   │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

| Connection | Detail |
|------------|--------|
| **Inventory & Stock** | Suppliers, products, and locations are shared master data. GRN posts stock, updates costs, and feeds markup pricing. |
| **Accounting** | Purchase orders are financial documents under Accounting. Stock receipts post journal entries. Payments use banks and payment methods from Accounting setup. |
| **Organisation** | User permissions, currencies, and multi-branch structure underpin procurement access and reporting. |
| **CRM** | Supplier relationships may overlap with vendor contacts where CRM is used alongside Stock suppliers. |

---

## Setup Guide for New Organisations

Follow this sequence before your first live procurement cycle:

### Phase 1 — Master data

1. **Suppliers** — add your vendor directory in Stock.
2. **Products** — catalogue items you buy regularly.
3. **Locations** — every warehouse and store that receives goods.
4. **Currencies**, **payment methods**, and **banks** — in Accounting.

### Phase 2 — Procurement configuration

5. Open **Procurement → Settings**.
6. Set **Variance Tolerance %** (start with 5%).
7. Set **Auto Approve Below** if low-value PRs should skip approval (or leave empty).
8. Configure **RFQ rules** and **payment terms** to match your policy.
9. Enable **Fiscal Verification** if required in your jurisdiction.

### Phase 3 — Permissions

10. Assign procurement permissions to requesters, approvers, warehouse staff, and finance users.

### Phase 4 — Pilot cycle

11. Run a **test requisition** end-to-end with a small value (see sample below).
12. Verify stock increased on receipt.
13. Confirm 3-way match shows **matched** on the test invoice.
14. Record a test payment and confirm invoice status updates.

### Sample pilot test

| Field | Example value |
|-------|---------------|
| Supplier | Test Supplier Ltd |
| Product | A4 Paper Ream |
| Quantity | 50 |
| Unit price | 10.00 |
| Line total | 500.00 |

Use the same numbers on the PO, GRN, and supplier invoice so matching succeeds on the first attempt.

![Screenshot: Procurement setup checklist — placeholder]

---

## Daily and Monthly Routines

### Daily

- Check **Command Center** KPIs for pending approvals and invoices to match.
- **Approve** requisitions submitted by departments.
- **Receive stock** against deliveries arriving today.
- **Record supplier invoices** as paperwork arrives; review match banners.

### Weekly

- Follow up **overdue POs** past expected delivery.
- Review **invoices with variance** — resolve with suppliers before payment run.
- Check **GRNs without linked invoices** — ensure AP is not missing bills.

### Monthly

- Review **spend by supplier** chart on the dashboard.
- Analyse **AP aging** — prioritise overdue payables.
- Reconcile procurement spend against Accounting **accounts payable**.
- Review requisition **pipeline** — are requests stalling at approval?
- Update **procurement settings** if policy thresholds change.

---

## Troubleshooting Common Situations

| Situation | What to do |
|-----------|------------|
| Requisition auto-approved on submit | Check **Auto Approve Below** in settings — clear it if all PRs should require approval |
| Cannot convert PR to PO | Ensure a **supplier** is set on the requisition before submit |
| PO Send button unavailable | PO must be in **Draft** status; confirm you have purchase order update permission |
| 3-way match shows **unmatched** | Link both **Purchase Order** and **GRN** on the invoice; confirm same supplier |
| Match shows **variance** | Compare invoice total to PO/GRN total; adjust invoice or increase **Variance Tolerance %** if difference is acceptable |
| Cannot create payment | Invoice must be **Approved** first; check invoice status |
| GRN not appearing on invoice | Select the correct supplier first — GRN list filters by supplier |
| Stock did not increase after receipt | Confirm receipt saved successfully; check location filter on inventory |
| RFQ Award does not create PO | RFQ must be **Sent** with supplier responses recorded |
| Invoice approval blocked | If fiscal verification is required, record and verify the fiscal receipt number first |

![Screenshot: Procurement help — placeholder]

---

## Related Documentation

- [Inventory & Stock](inventory-stock.md) — suppliers, receiving, products, locations, costing
- [Accounting & Sales](accounting.md) — purchase orders, currencies, banks, journal entries
- [Organisation](organisation.md) — users, roles, and permissions
- [Dashboard](dashboard.md) — platform-wide executive dashboards

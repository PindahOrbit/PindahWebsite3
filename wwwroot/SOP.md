<div class="sop-cover" role="doc-cover">
  <div class="sop-cover-hero">
    <img src="/images/sop-cover.jpg" alt="Operations planning desk with checklist, laptop, and coffee — representing structured business procedures" width="1600" height="900" />
  </div>
  <div class="sop-cover-body">
    <p class="sop-cover-brand">Pindah Basa / Operations</p>
    <h1 id="pindah-basa--operations--standard-operating-procedure-sop">Standard Operating Procedure</h1>
    <p class="sop-cover-subtitle">End-to-end system operation from organisation registration through logout — every activated module in <code>Operations.API</code> and <code>operations.ui</code>.</p>
    <dl class="sop-cover-meta">
      <div><dt>Document</dt><dd>Operations SOP</dd></div>
      <div><dt>Version</dt><dd>1.0</dd></div>
      <div><dt>Last updated</dt><dd>2026-08-05</dd></div>
      <div><dt>Classification</dt><dd>Internal — operators &amp; implementers</dd></div>
      <div><dt>Audience</dt><dd>IT, finance, warehouse, clinical, school, sales, insurance</dd></div>
      <div><dt>Owner</dt><dd>Pindah · <a href="https://pindah.org">pindah.org</a></dd></div>
    </dl>
  </div>
</div>

---

## Table of Contents

1. [Purpose and Audience](#1-purpose-and-audience)
2. [System Overview](#2-system-overview)
3. [Prerequisites and Environment](#3-prerequisites-and-environment)
4. [Phase 0 — Registration and Organisation Bootstrap](#4-phase-0--registration-and-organisation-bootstrap)
5. [Phase 1 — Authentication and Session Management](#5-phase-1--authentication-and-session-management)
6. [Phase 2 — Core Platform (Always Active)](#6-phase-2--core-platform-always-active)
7. [Phase 3 — Module Activation and Dependencies](#7-phase-3--module-activation-and-dependencies)
8. [Module SOPs](#8-module-sops)
   - [8.1 Inventory & Stock](#81-inventory--stock)
   - [8.2 Accounting & Sales](#82-accounting--sales)
   - [8.3 Procurement](#83-procurement)
   - [8.4 Pharmacy](#84-pharmacy)
   - [8.5 Hospital / Clinic (HMS)](#85-hospital--clinic-hms)
   - [8.6 School (Frame)](#86-school-frame)
   - [8.7 HR & Payroll](#87-hr--payroll)
   - [8.8 CRM](#88-crm)
   - [8.9 Insurance](#89-insurance)
   - [8.10 Logistics](#810-logistics)
   - [8.11 Agriculture & Feed](#811-agriculture--feed)
   - [8.12 Projects](#812-projects)
   - [8.13 Documents (DMS)](#813-documents-dms)
   - [8.14 Forms & Surveys](#814-forms--surveys)
   - [8.15 Asset Management](#815-asset-management)
9. [Cross-Module Business Workflows](#9-cross-module-business-workflows)
10. [Shared Tools (Chat, Mobile Scanner, Analyst, Guides)](#10-shared-tools)
11. [Phase Final — Logout and Session End](#11-phase-final--logout-and-session-end)
12. [Global Testing Matrix](#12-global-testing-matrix)
13. [Global Error Catalogue and Fixes](#13-global-error-catalogue-and-fixes)
14. [Appendices](#14-appendices)

---

## 1. Purpose and Audience

### 1.1 What this document is

This SOP is the single reference for operating **Pindah Basa** (branded UI: `operations.ui`) backed by **Operations.API** (.NET 8). It describes:

- What each feature is and why it exists
- Where it lives in the UI (route) and API
- Which other modules depend on it or consume its data
- Step-by-step workflows with prerequisites
- How to verify correct behaviour (test steps)
- Known errors, glitches, and remediation

### 1.2 Who should use this

| Role | Primary sections |
|------|------------------|
| System administrator / IT | Phases 0–3, Organisation, permissions, module activation |
| Finance / accounts | Accounting, Procurement payments, Insurance finance |
| Warehouse / stock | Inventory, receiving, transfers, stock takes |
| Clinical / pharmacy staff | Clinic, Pharmacy |
| School registrar / teachers | School (Frame) |
| Sales / CRM | CRM, Accounting customers |
| Insurance operations | Insurance module |
| End users | Authentication, daily module workflows, logout |

### 1.3 Conventions used

| Convention | Meaning |
|------------|---------|
| **Route** | Browser path under the app, e.g. `/stock/inventory/list` |
| **API** | REST endpoint under `{apiUrl}`, e.g. `GET /api/products` |
| **Permission** | Format `module:controller:operation`, e.g. `stock:inventory:view` |
| **Business number** | Real document ID at create time (`RX-`, `INV-`, `PO-`, `PR-`, `RFQ-`) — never `DRAFT-` / `TEMP-` prefixes |
| **Module** | SaaS feature package defined in `wwwroot/data/module-config.json` |

---

## 2. System Overview

### 2.1 Architecture

```
Browser (operations.ui — Angular 20)
  → AuthInterceptor (JWT Bearer)
  → Operations.API (.NET 8 modular monolith)
  → FilteredDbContext (multi-tenant SQL Server)
  → Integrations (Paynow, WhatsApp, AI, R2, Traccar, SignalR)
```

### 2.2 Deployment modes

| Mode | When | Behaviour |
|------|------|-----------|
| **Cloud** | Production / hosted | API + separate Angular app; Swagger at API root |
| **Desktop** | `ASPNETCORE_ENVIRONMENT=Desktop` | API bundles SPA from `wwwroot/app`; local SQL Express; branch sync |

### 2.3 Module catalogue

| Module key | Menu label | Route prefix | Hard dependencies |
|------------|------------|--------------|-------------------|
| `core` | Dashboard, Organisation | `/dashboard`, `/organisation` | Always on |
| `inventory` | Stock | `/stock` | — |
| `accounting` | Accounting | `/accounting` | `inventory` |
| `procurement` | Procurement | `/procurement` | `inventory`, `accounting` |
| `pharmacy` | Pharmacy | `/pharmacy` | `inventory`, `accounting` |
| `clinic` | Hospital | `/clinic` | `inventory`, `accounting` |
| `school` | Frame | `/school` | — |
| `hr` | HR & Payroll | `/hr` | — |
| `crm` | CRM | `/crm` | — |
| `insurance` | Insurance | `/insurance` | `core`, `accounting` |
| `logistics` | Logistics | `/logistics` | — |
| `agriculture` | Agriculture | `/agriculture` | `inventory` |
| `projects` | Projects | `/projects` | — |
| `documents` | Documents | `/documents` | — |
| `forms` | Forms | `/forms` | — |
| `assets` | Assets | `/assets` | — |

### 2.4 Shared master data

| Entity | Owned by | Consumed by |
|--------|----------|-------------|
| Products | Stock | Accounting POS, Pharmacy, Clinic supplies, Agriculture raw materials |
| Customers / Patients | Accounting / Clinic | Invoices, CRM, Pharmacy, Clinic billing |
| Suppliers | Stock | Procurement, Stock receipts |
| Users & roles | Organisation / HR | All modules |
| Locations & tills | Stock | POS, Pharmacy dispense, transfers |
| Periods | Organisation | School, Accounting reporting |

---

## 3. Prerequisites and Environment

### 3.1 Before any user can work

| # | Prerequisite | Where to configure | Verify |
|---|--------------|-------------------|--------|
| 1 | Organisation registered | `/register` or admin-created | `GET /api/organisations/current` returns org |
| 2 | At least one admin user | Registration or invite | Can log in |
| 3 | Modules activated | `/organisation/modules` | Sidebar shows expected menus |
| 4 | Roles assigned | `/organisation/roles` | User has permissions |
| 5 | Base currency | `/accounting/currencies` | POS and invoices show currency |
| 6 | Locations (if selling/stocking) | `/stock/locations` | At least one location |
| 7 | Tills (if POS/pharmacy) | `/stock/tills` | Till linked to location |
| 8 | Chart of accounts (if accounting) | `/accounting/charts-of-accounts` | Trial balance runs |
| 9 | System Cash account mapped | `/accounting/charts-of-accounts/system-accounts` | Record Payment works |

### 3.2 Environment URLs

| Environment | UI | API (`environment.apiUrl`) |
|-------------|-----|---------------------------|
| Local dev | `ng serve` (default 4200) | `https://localhost:44316/api` |
| Production | `https://basa.pindah.org` | `https://api-basa.pindah.org/api` |
| Mobile build | Capacitor app | `https://api.basa.pindah.org/api` |

### 3.3 Browser and client requirements

- Modern Chromium, Firefox, or Safari (latest two versions)
- JavaScript enabled; cookies/localStorage for JWT
- For POS: stable network to API; optional desktop print bridge at `http://127.0.0.1:17890`
- For mobile scanner: phone on same network; SignalR hub `/hubs/barcode-scan`

---

## 4. Phase 0 — Registration and Organisation Bootstrap

### 4.1 What it is

Organisation registration creates a new tenant: database row for organisation, default roles (Administrator, Manager, Viewer), seed permissions, and an owning admin user. This is the entry point for every new customer.

### 4.2 Registration paths

| Path | Route | API | When used |
|------|-------|-----|-----------|
| Self-registration | `/register` | `POST /api/auth/register` | Public signup enabled |
| External registration app | `registrationUrl` in environment | External MVC app | Marketing funnel |
| Admin invite | `/accept-invite/:token` | `POST /api/auth/accept-invite` | Staff onboarding |
| Student activation | `/activate-student` or `/activate-student/:token` | School activation endpoints | Frame schools |

### 4.3 Self-registration workflow

**Prerequisites:** Registration enabled; valid email; organisation name not duplicate (policy-dependent).

**Steps:**

1. Navigate to `/register`.
2. Enter organisation name, admin name, email, password.
3. Submit — API creates org, admin user, default roles.
4. Check email for confirmation link → `/email-confirmation`.
5. Click confirmation link; email verified.
6. Go to `/login`; sign in with credentials.
7. Land on `/dashboard` (or till-selection if POS configured).

**Business rules:**

- Organisation receives real identity immediately (not a temp org code).
- Administrator role gets all permissions auto-synced.
- Business document numbers use real prefixes from first create in each module.

**Test — registration happy path:**

| Step | Action | Expected |
|------|--------|----------|
| 1 | Complete `/register` | 200; redirect or success message |
| 2 | Confirm email | Status verified |
| 3 | Login | JWT in localStorage (`auth_token`) |
| 4 | `GET /api/users/me` | Returns admin user |
| 5 | `GET /api/organisations/current` | Returns new org |
| 6 | Sidebar | Dashboard, Organisation visible |

**Errors and fixes:**

| Error | Cause | Fix |
|-------|-------|-----|
| Email already registered | Duplicate user | Use password reset or invite flow |
| Weak password | Policy rejection | Meet minimum length/complexity |
| Email not received | SMTP misconfiguration | Check spam; admin resends from Organisation → Emails |
| 500 on register | DB connection | Verify `operationsConn` in API appsettings |
| Stuck on login after register | Email unconfirmed | Complete `/email-confirmation` |

### 4.4 Accept invite workflow

**Prerequisites:** Valid invite token from admin (`/organisation/users` or HR invite).

**Steps:**

1. Open link `/accept-invite/:token`.
2. Complete profile and password.
3. Sign in.

**Test:** Invite email → accept → login → permissions match assigned role.

**Errors:**

| Error | Fix |
|-------|-----|
| Token expired | Admin resends invite |
| Invalid token | Copy full URL from email |

### 4.5 Post-registration bootstrap checklist (administrator)

Execute in order after first login:

| # | Task | Route | Required for |
|---|------|-------|--------------|
| 1 | Set organisation profile & logo | `/organisation/general` | Branding |
| 2 | Activate modules | `/organisation/modules` | Feature menus |
| 3 | Configure security (2FA policy) | `/organisation/security` | Compliance |
| 4 | Create periods (school/finance) | `/organisation/periods` | Reporting |
| 5 | Add locations | `/stock/locations` | Stock, POS, pharmacy |
| 6 | Add tills | `/stock/tills` | POS, dispense |
| 7 | Import/create chart of accounts | `/accounting/charts-of-accounts` | GL |
| 8 | Map system Cash account | `/accounting/charts-of-accounts/system-accounts` | Payments |
| 9 | Add payment methods | `/accounting/payment-methods` | POS, invoices |
| 10 | Invite staff | `/organisation/users` or `/hr/users/invite-users` | Operations |

---

## 5. Phase 1 — Authentication and Session Management

### 5.1 What it is

Authentication establishes identity via JWT access token (60 min) + refresh token (7 days). Authorization uses role permissions and module activation middleware on the API.

### 5.2 Login methods

| Method | Route | API | Notes |
|--------|-------|-----|-------|
| Email/password | `/login` | `POST /api/auth/login` | Primary |
| Google | `/login` (Google button) | `POST /api/auth/google-login` | Requires Google client ID |
| WebAuthn / passkey | Login screen | `/api/auth/webauthn/*` | FIDO2 |
| 2FA completion | `/two-factor-verify` | `POST /api/auth/complete-2fa-login` | After password when 2FA enabled |

### 5.3 Login workflow

**Prerequisites:** Active user; confirmed email (if required); org not suspended.

**Steps:**

1. Open `/login`.
2. Enter email and password (or Google sign-in).
3. If 2FA required → `/two-factor-verify` → enter TOTP / email OTP / recovery code.
4. `AuthService` stores JWT + refresh in `localStorage`.
5. App calls `GET /api/users/me` and `GET /api/organisations/current`.
6. `PermissionService` loads permissions.
7. If POS user without till → `/organisation/location-and-till` (`tillSelectionPageGuard`).
8. Redirect to `returnUrl` or preferred dashboard.

**Session lifecycle (automatic):**

- `AuthInterceptor` attaches `Authorization: Bearer {token}`.
- Proactive refresh when < 5 minutes to expiry.
- 401 → refresh and retry once; failure → logout.
- `session-timeout.service` warns on idle; can force logout.

**Test — login:**

| Check | Expected |
|-------|----------|
| Valid credentials | Dashboard loads; menu populated |
| Wrong password | Error toast; no token |
| Expired JWT + valid refresh | Silent refresh; no re-login |
| Expired refresh | Redirect to `/login` |
| User without module permission | Menu item hidden; direct URL → `/not-authorized` |
| Inactive module API call | 403 from `ModuleActivationMiddleware` |

### 5.4 Two-factor authentication

| Flow | Route | Steps |
|------|-------|-------|
| Setup | `/two-factor-setup` | Scan QR → verify code → save recovery codes |
| Verify on login | `/two-factor-verify` | Enter 6-digit code after password |
| Disable | Account settings / admin policy | Organisation → Security |

**Errors:**

| Error | Fix |
|-------|-----|
| Invalid code | Check device time sync (TOTP) |
| Locked out | Use recovery code; admin reset |
| Email OTP not received | Check Organisation email templates |

### 5.5 Password reset

**Route:** `/password-reset`  
**API:** `POST /api/auth/forgot-password` → email link → reset form

**Test:** Request reset → email link → new password → login succeeds.

### 5.6 Permissions model

- Format: `module:controller:operation` (e.g. `sales:invoices:create`)
- Wildcards: `*:*:*` (super), `stock:*:*`, `stock:inventory:*`
- Default roles: **Administrator** (all), **Manager** (no deletes), **Viewer** (view only), **Student** (school self-service)
- Source of truth: `wwwroot/default-permissions.md` + database role assignments
- UI mapping: `operations.ui/src/app/config/permission-config.ts`

**Refresh permissions after role change:** Profile menu → Refresh permissions (or re-login).

---

## 6. Phase 2 — Core Platform (Always Active)

### 6.1 Dashboard

**What it is:** Role-based home screen with widget shortcuts to active modules.

| Route | Component | Guard | Purpose |
|-------|-----------|-------|---------|
| `/dashboard` | Overview dashboard | `overviewDashboardGuard` | Org-wide KPIs |
| `/dashboard/user` | User dashboard | — | Personal shortcuts |
| `/dashboard/financial` | Redirect | — | → `/accounting/dashboard` |
| `/dashboard/inventory` | Redirect | — | → `/stock/dashboard` |

**Used by:** All authenticated users.  
**Requires:** `core` module (always on).

**Test:** Login as admin → widgets match activated modules. Login as viewer → no create buttons on widgets.

### 6.2 Organisation settings

**What it is:** Tenant administration — profile, modules, users, roles, billing, security, branch sync.

| Tab / route | Function |
|-------------|----------|
| `/organisation/general` | Name, logo, contact, regional settings |
| `/organisation/modules` | Enable/disable SaaS modules |
| `/organisation/users` | Staff list, invites |
| `/organisation/roles` | Role & permission management |
| `/organisation/billing` | Subscription and payment |
| `/organisation/periods` | Academic/fiscal periods |
| `/organisation/security` | Password policy, 2FA |
| `/organisation/location-and-till` | POS till selection |
| `/organisation/branch-sync` | Offline multi-branch replication |
| `/organisation/pos-devices` | Hardware pairing |
| `/organisation/emails` | Email templates |

**Cross-module impact:** Module toggles immediately affect sidebar and API access. Periods drive School and financial reporting.

**Test — module activation:**

1. Deactivate Stock in `/organisation/modules`.
2. Refresh permissions.
3. Verify Stock menu hidden; `GET /api/products` returns 403.
4. Re-activate; menu returns.

**Errors:**

| Error | Fix |
|-------|-----|
| Cannot save module change | Check subscription tier (`SubscriptionPolicyMiddleware`) |
| User sees stale menu | Refresh permissions or hard refresh browser |
| 402 on navigation | `/organisation/billing` — subscription payment required |

### 6.3 Account settings (personal)

**Route:** `/account/settings`  
**What it is:** User profile, password, display preferences, theme.

**Test:** Change password → logout → login with new password.

### 6.4 Guides & Help

**Route:** `/guides`  
**What it is:** In-app documentation browser.

### 6.5 Notifications

**Route:** `/notifications`  
**API:** Notification endpoints  
**What it is:** System alerts (stock, approvals, subscriptions).

### 6.6 Global search

**Location:** Header  
**What it is:** Search routes and entities across modules.

---

## 7. Phase 3 — Module Activation and Dependencies

### 7.1 Activation order (recommended)

```
core (automatic)
  → inventory
    → accounting
      → procurement | pharmacy | clinic | insurance
  → hr | crm | school | projects | documents | forms | assets | logistics
  → agriculture (needs inventory)
```

### 7.2 Dependency enforcement

| If you need… | You must have… |
|--------------|----------------|
| POS sales | Stock (products, location, till) + Accounting |
| Pharmacy dispense | Stock + Accounting + Pharmacy module |
| Clinic billing | Clinic + Accounting (+ Stock for supplies) |
| Procurement 3-way match | Procurement + Stock + Accounting |
| Insurance premiums in GL | Insurance + Accounting |
| Feed formulas with real materials | Agriculture + Stock |

### 7.3 Till selection prerequisite

**Route:** `/organisation/location-and-till`  
**Guard:** `tillSelectionPageGuard`  
**When:** User has POS permissions but no till selected in session.  
**Test:** Clear till session → navigate to POS → redirected to till selection → select → POS loads.

---

## 8. Module SOPs

---

### 8.1 Inventory & Stock

#### What it is

Product catalogue, warehouses, suppliers, receiving, stock takes, transfers, pricing, tills, and movement audit. Foundation for retail, pharmacy, clinic supplies, and agriculture.

#### Where it is used

| Consumer | Usage |
|----------|-------|
| Accounting POS | Deducts stock on sale |
| Pharmacy | FEFO batch deduction on dispense |
| Clinic | Clinical consumables |
| Procurement | GRN receiving |
| Agriculture | Raw materials catalogue |
| Reports | Valuation, movement |

#### Routes reference

| Function | Route |
|----------|-------|
| Dashboard | `/stock/dashboard` |
| Product list | `/stock/inventory/list` |
| Add product | `/stock/add` |
| Bulk add | `/stock/inventory/new` |
| Categories | `/stock/categories` |
| Suppliers | `/stock/suppliers` |
| Locations | `/stock/locations` |
| Tills | `/stock/tills` |
| Orders / receipts | `/stock/orders` |
| Receive stock | `/stock/orders/receive` |
| Stock takes | `/stock/stocktakes` |
| Count | `/stock/stocktakes/:id/count` |
| Transfers | `/stock/transfers` |
| Markups | `/stock/inventory/markups` |
| Price approvals | `/stock/inventory/price-approvals` |
| Audit log | `/stock/audit` |
| Reports | `/stock/reports/*` |

**Permissions:** `stock:inventory:*`, `stock:receipts:*`, `stock:stocktake:*`, `stock:transfers:*`, `pos:tills:*`

#### Workflow A — New product setup

**Prerequisites:** Categories, locations, optionally suppliers.

1. `/stock/add` → enter name, SKU, category, cost, sell price, reorder level.
2. Set opening quantity per location.
3. Save → product appears in `/stock/inventory/list`.

**Test:** Search product in list; quantity matches opening stock; audit log shows opening entry.

#### Workflow B — Receive stock from supplier

**Prerequisites:** Supplier, location, products exist.

1. `/stock/orders/create` or receive against existing PO.
2. Select supplier, location, supplier invoice number.
3. Add lines with quantities and costs.
4. Post as **Received** → quantities increase.

**Test:** Product qty ↑; audit log type = Receipt; cost updated.

#### Workflow C — Stock take

**Prerequisites:** Location defined; products with stock.

1. `/stock/stocktakes` → New → scope location/category.
2. `/stock/stocktakes/:id/count` → enter counted qty (or scan barcodes).
3. Submit for approval → approver completes → system qty adjusts.

**Test:** Variance report matches manual calculation; status = Completed.

#### Workflow D — Stock transfer

**Prerequisites:** Two locations with tills; stock at source.

1. `/stock/transfers/create` → source/destination, receiver, lines.
2. Approver: **Approve & Dispatch** → status In Transit; source qty ↓.
3. Receiver: **Confirm Receipt** → destination qty ↑.

**Test:** Audit shows transfer out/in; balances correct at both locations.

#### Workflow E — Price change approval

**Prerequisites:** Markup rules or manual price change on receipt.

1. Change triggers request → `/stock/inventory/price-approvals`.
2. Manager approves or rejects.

**Test:** Approved price visible on product; rejected price unchanged.

#### Errors and fixes

| Situation | Diagnosis | Fix |
|-----------|-----------|-----|
| Product shows zero stock | Wrong location filter | Select correct location; receive stock |
| POS insufficient stock | No qty at till's location | Receive or transfer stock in |
| Transfer rejected | Insufficient source qty | Reduce qty or receive stock first |
| Price not updated after receipt | Pending approval | Approve in price approvals queue |
| Barcode not found | Missing on product | Edit product; add barcode |
| Large stock take variance | Unrecorded movements | Review `/stock/audit` |

---

### 8.2 Accounting & Sales

#### What it is

General ledger, POS, invoices, quotations, purchase orders (financial), customers, payments, expenses, bank import, financial statements.

#### Where it is used

| Consumer | Usage |
|----------|-------|
| All revenue modules | Posts to GL via journals |
| Stock | POS deducts inventory |
| Pharmacy | Dispense payment → sales |
| Clinic | Patient invoices |
| School | Fees (when configured) |
| Insurance | Premiums, commissions |
| Procurement | Supplier payments |

#### Routes reference

| Function | Route |
|----------|-------|
| Dashboard | `/accounting/dashboard` |
| Chart of accounts | `/accounting/charts-of-accounts/chart-of-accounts` |
| Journal entries | `/accounting/charts-of-accounts/journal-entries` |
| Trial balance | `/accounting/charts-of-accounts/trial-balance` |
| Income statement | `/accounting/charts-of-accounts/income-statement` |
| Balance sheet | `/accounting/charts-of-accounts/balance-sheet` |
| POS | `/accounting/sales/create` |
| Sales list | `/accounting/sales/list` |
| Invoices | `/accounting/invoices` |
| Create invoice | `/accounting/invoices/create` |
| Quotations | `/accounting/quotations` |
| Purchase orders | `/accounting/purchase-orders` |
| Customers | `/accounting/customers` |
| Payment methods | `/accounting/payment-methods` |
| Currencies | `/accounting/currencies` |
| Fees & taxes | `/accounting/fees-taxes` |
| Banks | `/accounting/banks` |

**Permissions:** `accounting:*`, `sales:*`, `pos:sales:*`

#### Workflow A — Initial accounting setup

**Prerequisites:** Admin access.

1. Create/import chart of accounts.
2. Map **System Cash** account.
3. Add currencies, payment methods, fees/taxes, banks.
4. Add customers.
5. Test: Record Payment → check journal entry → trial balance balances.

#### Workflow B — POS sale

**Prerequisites:** Till selected; products with stock at till location; payment methods.

1. `/accounting/sales/create`.
2. Select customer (or walk-in).
3. Add products (search/scan).
4. Apply taxes/discounts.
5. Select payment method; complete sale.
6. Receipt prints; stock deducts.

**Test:** Sale in `/accounting/sales/list`; stock qty ↓; run **Consolidate** on dashboard → journal entry created.

#### Workflow C — Invoice (credit sale)

**Prerequisites:** Customer; products/services.

1. `/accounting/invoices/create` → customer, dates, lines.
2. Save as Pending → real `INV-` number assigned at create.
3. Email PDF to customer.
4. When paid: **Make Payment** → status Paid/Partial.

**Test:** Balance column correct; receivables aging on dashboard updates.

#### Workflow D — Month-end consolidate

**Prerequisites:** POS sales in period.

1. `/accounting/dashboard` → **Consolidate** for date range.
2. Review new journal entries.
3. Run trial balance, income statement, balance sheet.

**Test:** Debits = credits; revenue matches sales register.

#### Errors and fixes

| Situation | Fix |
|-----------|-----|
| Record Payment fails | Map Cash system account |
| Trial balance imbalance | Find one-sided journal entries |
| Consolidate creates nothing | Sales already consolidated or none in range |
| POS product not found | Product/stock at till location |
| Invoice wrong tax | Check `/accounting/fees-taxes` |

---

### 8.3 Procurement

#### What it is

Source-to-pay: requisitions, approvals, RFQs, supplier invoices with 3-way match, supplier payments.

#### Dependencies

`inventory`, `accounting` — uses Stock suppliers/GRN and Accounting POs.

#### Routes reference

| Function | Route |
|----------|-------|
| Dashboard | `/procurement/dashboard` |
| Requisitions | `/procurement/requisitions` |
| RFQs | `/procurement/rfqs` |
| Supplier invoices | `/procurement/supplier-invoices` |
| Payments | `/procurement/payments` |
| Approvals | `/procurement/approvals` |
| Settings | `/procurement/settings` |

**Cross-routes:** PO → `/accounting/purchase-orders`; GRN → `/stock/orders/receive`; suppliers → `/stock/suppliers`.

#### Workflow — Full procure-to-pay

**Prerequisites:** Suppliers, products, approval rules, chart of accounts.

```
PR (draft) → submit → approve
  → convert to PO (/accounting/purchase-orders) OR RFQ (/procurement/rfqs)
  → award supplier
  → goods arrive → receive (/stock/orders/receive)
  → supplier invoice (/procurement/supplier-invoices) — 3-way match
  → approve → payment (/procurement/payments)
```

**Test each gate:**

| Stage | Verify |
|-------|--------|
| PR approved | Status Approved; appears in approvals |
| PO sent | Status Pending; supplier notified |
| GRN posted | Stock qty ↑; linked to PO |
| Invoice matched | PO qty = GRN qty = invoice qty |
| Payment recorded | Supplier balance ↓; journal entry |

**Errors:**

| Error | Fix |
|-------|-----|
| 3-way match fails | Correct GRN or PO quantities |
| Cannot submit PR | Complete required fields; check approval rules |
| Payment without approval | Complete approval workflow first |

---

### 8.4 Pharmacy

#### What it is

Dispensary operations: quick dispense, verification queue, refills, medical aid claims, labels, optional POS payment.

#### Dependencies

`inventory`, `accounting`, `pharmacy` module; patients from Clinic or Accounting customers.

#### Routes reference

| Function | Route |
|----------|-------|
| Dashboard | `/pharmacy/dashboard` |
| Quick dispense | `/pharmacy/dispense` |
| Dispensing records | `/pharmacy/dispensing-records` |
| Pending refills | `/pharmacy/pending-refills` |
| Medical aid claims | `/pharmacy/medical-aid-claims` |
| Mixtures | `/pharmacy/mixtures` |
| Dosage cyphers | `/pharmacy/settings/dosage-cyphers` |
| Reports | `/pharmacy/reports` |
| Patients (redirect) | → `/accounting/customers` |
| Products (redirect) | → `/stock/inventory` |

#### Workflow — Dispense to payment

**Prerequisites:** Location/till; products with batch stock; dosage cyphers; payment methods.

1. `/pharmacy/dispense` → select location, patient.
2. Review allergy alerts.
3. Add medicine lines (FEFO batch auto-selected).
4. Attach prescription script image (per policy).
5. **Save & Print** → real `RX-` number; stock deducts; status **Dispensed**.
6. Payment modal → collect or skip.
7. Queue: **Verify** → **Collected** → **Paid**.

**Test:**

| Check | Expected |
|-------|----------|
| RX number | `RX-` prefix at create, never changes |
| Stock | Qty ↓ at selected location |
| Queue status | Progresses Dispensed → Verified → Paid |
| Refill | Scheduled refill appears in pending refills on due date |
| Medical aid | Claim in `/pharmacy/medical-aid-claims`; remittance reduces "owed by schemes" |

**Errors:**

| Situation | Fix |
|-----------|-----|
| Product not in search | Stock at location; product active |
| Script warning on save | Attach script or override per policy |
| Wrong stock location | Change location before save |
| Payment skipped | Collect later from queue → Process Sale |

---

### 8.5 Hospital / Clinic (HMS)

#### What it is

Hospital Management System: patients, appointments, EMR encounters, lab, inpatient, billing claims, admin master data. Menu label: **Hospital**; route prefix `/clinic`.

#### Dependencies

`inventory`, `accounting`; facility type in `Organisation.custom_args.healthcare_facility_type` controls feature set (clinic, hospital, surgery_center, dental, multi_specialty).

#### Menu structure (6 areas)

| Area | Routes | Status |
|------|--------|--------|
| **Front Desk** | `/clinic/patients`, `/clinic/appointments`, `/clinic/appointments/waitlist` | Operational |
| **OPD** | `/clinic/outpatient/my-queue`, `/clinic/emr/encounters`, `/clinic/teleconsult`, `/clinic/consent-forms` | Operational |
| **IPD** | `/clinic/inpatient/admissions`, `/clinic/inpatient/wards-beds` | Partial — see gaps |
| **Lab** | `/clinic/lab/orders`, `/clinic/lab/sample-tracking` | Partial |
| **Billing** | `/clinic/billing/insurance-claims`; invoices → `/accounting/invoices` | Operational |
| **Admin** | `/clinic/admin/*`, `/clinic/schedule/doctors` | Operational |

#### Workflow — Outpatient visit (happy path)

**Prerequisites:** Medical aid societies (if applicable); staff users; products for dispensing.

1. **Register patient** `/clinic/patients` → New → demographics, allergies, medical aid.
2. **Book appointment** `/clinic/appointments`.
3. **Encounter** `/clinic/emr/encounters` → vitals, notes, diagnosis.
4. **Prescription** `/clinic/patients/:id/prescriptions/new`.
5. **Pharmacy** `/pharmacy/dispense` → same patient.
6. **Bill** `/accounting/invoices` or collect at dispense.
7. **Medical aid claim** `/pharmacy/medical-aid-claims` or `/clinic/billing/insurance-claims`.

**Test:** Patient chart shows linked encounter, prescription, invoice; balances correct.

#### Known gaps and workarounds (documented in `hms_frontend_workflows.md`)

| Feature | Issue | Workaround |
|---------|-------|------------|
| Triage | Route not implemented | Use waitlist + encounter notes |
| Nursing station | Not routed | Use encounter vitals |
| Drug interactions | Not routed | External reference; manual check |
| Lab results dedicated page | Missing | Use lab orders status |
| Surgery bookings / OR | Backend PATCH mismatches | Avoid in production demo; use appointments |
| Admit/discharge custom actions | May 404 | Use standard CRUD until backend extended |
| Appointment cancel | Custom endpoint missing | Edit status via full PUT if supported |

**When testing Clinic:** Prefer Patient Directory, Appointments, Encounters, Quick Dispense, Invoices, Medical Aid Claims. Avoid OR/Surgery unless verified on your API version.

**Errors:**

| Error | Fix |
|-------|-----|
| 404 on PATCH actions | Use documented workarounds; align API version |
| Patient not in pharmacy search | Same customer/patient register — create in clinic first |
| Insurance claim stuck | Check medical aid on patient profile; society master data |

---

### 8.6 School (Frame)

#### What it is

School management for Zimbabwe primary/secondary: students, stages/classrooms, assessments, marks, report cards, attendance, e-learning. Menu label: **Frame**.

#### Dependencies

Organisation periods; HR for staff; optional Accounting for fees.

#### Routes reference

| Function | Route |
|----------|-------|
| Dashboard | `/school/dashboard` |
| Students | `/school/students` |
| Create student | `/school/students/create` |
| Staff | `/school/staff` |
| Assessments | `/school/assessments` |
| Marks | `/school/assessments/marks` |
| Write assessment (student) | `/school/assessments/:id/write` |
| Reports / report cards | `/school/reports` |
| Attendance | `/school/attendance-registers` |
| Grading | `/school/grading/*` |
| E-learning | `/school/elearning` |
| Student profile | `/school/student-profile/*` |
| Student activation | `/activate-student/:token` |

#### Workflow — Term assessment to report card

**Prerequisites:** Periods (`/organisation/periods`); stages/classrooms; subjects; grading scales.

1. Create period (e.g. `2026 – Term 1`).
2. `/school/grading/stages` → define forms/grades and classrooms.
3. `/school/subjects` → subject catalogue.
4. `/school/students/create` → enrol; assign classroom.
5. `/school/assessments/create` → link subject, stage, period.
6. `/school/assessments/marks` → enter scores.
7. `/school/reports` → generate report cards → Sync Marks → Refresh Grades → PDF.

**Student login workflow:**

1. Admin creates student → activation email.
2. Student opens `/activate-student/:token` → set password.
3. Student logs in → `/school/student-profile/dashboard`.

**Test:**

| Check | Expected |
|-------|----------|
| Marks in assessment | Visible in mark sheet |
| Report card | Grades match grading scale |
| Student login | Only student-profile routes; no admin menus |
| Attendance | Register saves; report shows trends |

**Errors:**

| Error | Fix |
|-------|-----|
| Wrong period on marks | Select correct period filter |
| Report card empty | Run Sync Marks from assessments |
| Student cannot log in | Re-send activation; check Student role |

---

### 8.7 HR & Payroll

#### What it is

Employee records, invites, user groups, payroll, pay periods, performance scorecards, timetables, onboarding/offboarding, announcements.

#### Routes reference

| Function | Route |
|----------|-------|
| Dashboard | `/hr/dashboard` |
| Users | `/hr/users` |
| Invite | `/hr/users/invite-users` |
| Import | `/hr/users/import-users` |
| User groups | `/hr/user-groups` |
| Payroll | `/hr/payroll` |
| Pay periods | `/hr/payroll/pay-periods` |
| Performance | `/hr/performance/*` |
| Timetables | `/hr/timetables` |
| Onboarding | `/hr/onboarding` |
| Offboarding | `/hr/offboarding` |
| Announcements | `/hr/announcements` |

#### Workflow — Hire to payroll

1. `/hr/users/invite-users` → email invite.
2. User accepts → appears in `/hr/users`.
3. Assign role at `/organisation/roles`.
4. `/hr/payroll/pay-periods` → create period.
5. `/hr/payroll/add` → run payroll for period.

**Test:** Invite → accept → user in list → payroll line generated.

**Cross-module:** School `/school/staff` reuses `UsersListComponent`. Clinic clinical staff are users with clinic permissions.

---

### 8.8 CRM

#### What it is

Leads, opportunities, pipeline, accounts, contacts, cases, campaigns, quotes, forecasts, knowledge base, portals.

#### Routes reference (selected)

| Function | Route |
|----------|-------|
| Dashboard | `/crm/dashboard` |
| Leads | `/crm/leads` |
| Opportunities | `/crm/opportunities` |
| Pipeline | `/crm/pipeline` |
| Accounts | `/crm/accounts` |
| Contacts | `/crm/contacts` |
| Customer 360 | `/crm/customers/:id` |
| Cases | `/crm/cases` |
| Quotes | `/crm/quotes` |
| Campaigns | `/crm/campaigns` |
| Reports | `/crm/reports` |

#### Workflow — Lead to sale

1. `/crm/leads/create` → capture lead.
2. Qualify → convert to opportunity.
3. `/crm/opportunities` → move through pipeline stages.
4. `/crm/quotes/create` → send quote.
5. On win → create Accounting quotation/invoice with same customer.

**Test:** Lead count on dashboard ↑; opportunity stage changes persist; quote PDF generates.

---

### 8.9 Insurance

#### What it is

Full insurance lifecycle: submissions, quotes, placements, policies, endorsements, renewals, claims, reinsurance, regulatory, client money, commissions.

#### Dependencies

`core`, `accounting`; documents module recommended for policy files.

#### Entity creation order (mandatory)

```
Submission → Quote → Placement → Policy
  → Endorsement (on active policy)
  → FNOL → Claim → Reserves → Claim Payment
```

Policy versions are immutable; endorsements create new versions.

#### Routes reference (selected)

| Function | Route |
|----------|-------|
| Dashboard | `/insurance/dashboard` |
| Submissions | `/insurance/submissions/list-submissions` |
| Quotes | `/insurance/quotes/list-quotes` |
| Placements | `/insurance/placements/list-placements` |
| Policies | `/insurance/policies/list-policies` |
| Claims | `/insurance/claims/list-claims` |
| FNOL | `/insurance/claims/create-update-fnol` |
| Reinsurance | `/insurance/reinsurance/*` |
| Regulatory | `/insurance/regulatory/*` |
| Client money | `/insurance/finance/list-client-money-transactions` |
| Commissions | `/insurance/finance/list-commissions` |

#### Workflow — New business to policy

1. Create submission with risk items.
2. Generate/compare quotes.
3. Record placement when market binds.
4. Issue policy from placement.
5. Premium posts to accounting (per integration setup).

#### Workflow — Claim

1. FNOL → claim record.
2. Set reserves.
3. Adjudicate → claim payment.
4. Update loss ratio on dashboard.

**Test:** Cannot create policy without placement; endorsement blocked on cancelled policy; commission statement totals match payments.

**Errors:**

| Error | Fix |
|-------|-----|
| Placement invariant violation | Complete quote acceptance first |
| Regulatory validation fail | Review `/insurance/regulatory/list-regulatory-validation-results` |
| Client money mismatch | Reconcile transactions against policy ledger |

---

### 8.10 Logistics

#### What it is

Fleet management: drivers, vehicles, trips, live monitoring (Traccar GPS integration).

#### Routes reference

| Function | Route |
|----------|-------|
| Dashboard | `/logistics/dashboard` |
| Trips | `/logistics/trips` |
| Monitor | `/logistics/trips/monitor` |
| Drivers | `/logistics/drivers` |
| Vehicles | `/logistics/vehicles` |
| Clients | `/logistics/clients` |

#### Workflow — Delivery trip

1. Create vehicle and driver.
2. `/logistics/trips` → plan stops, assign driver/vehicle.
3. Start trip → monitor on `/logistics/trips/monitor`.
4. Complete trip → review summary.

**Test:** Trip appears on map; status transitions; links to CRM customers if used.

**Cross-module:** Pharmacy Home Delivery uses Logistics trips (when delivery module enabled).

---

### 8.11 Agriculture & Feed

#### What it is

Feed formulation: nutrients, raw materials (from stock), nutrient matrix, formulas, generation reports.

#### Dependencies

`inventory` for raw materials (`/agriculture/raw-materials` → stock products).

#### Routes reference

| Function | Route |
|----------|-------|
| Dashboard | `/agriculture/dashboard` |
| Nutrients | `/agriculture/nutrients` |
| Raw materials | `/agriculture/raw-materials` |
| Nutrient matrix | `/agriculture/nutrient-matrix` |
| Formulas | `/agriculture/formulas` |
| Reports | `/agriculture/reports` |
| Setup | `/agriculture/setup` |

#### Workflow — Create feed formula

1. Define nutrients.
2. Map raw materials (inventory products) in nutrient matrix.
3. `/agriculture/formulas/create` → ingredients + targets.
4. Generate report → review nutritional analysis.

**Test:** Formula saves; report calculates nutrient totals; raw material costs pull from stock.

---

### 8.12 Projects

#### What it is

Project tracking, issues (list + kanban), job cards, field job cards.

#### Routes reference

| Function | Route |
|----------|-------|
| Dashboard | `/projects/dashboard` |
| Projects | `/projects/list` |
| Issues | `/projects/issues` |
| Issues board | `/projects/issues/board` |
| Job cards | `/projects/job-cards` |
| Field job cards | `/projects/field-job-cards` |

#### Workflow — Job card

1. Create project.
2. `/projects/job-cards/create` → work order details.
3. Complete → issue closed.

**Test:** Job card PDF; linked issues update status.

---

### 8.13 Documents (DMS)

#### What it is

Document management: cabinets, folders, uploads, approvals, workflows, share links, OnlyOffice viewing.

#### Routes reference

| Function | Route |
|----------|-------|
| Dashboard | `/documents` |
| Browse | `/documents/browse` |
| Upload | `/documents/create` |
| View (Office) | `/documents/view/:id` |
| Approvals | `/documents/approvals` |
| Expiry | `/documents/expiry` |
| Admin workflows | `/documents/admin/workflows` |
| Share links | `/documents/admin/shared-links` |

**Public route (no auth):** `/share/:token` → view shared document.

#### Workflow — Document approval

1. Upload to folder.
2. Workflow triggers → appears in `/documents/approvals`.
3. Approver signs off → status approved.

**Test:** Share link works in incognito; expiry alerts on dashboard.

---

### 8.14 Forms & Surveys

#### What it is

Form builder, published public forms, response collection.

#### Routes reference

| Function | Route |
|----------|-------|
| List | `/forms` |
| Builder | `/forms/create`, `/forms/:id/edit` |
| Responses | `/forms/:id/responses` |

**Public route (no auth):** `/f/:publicId`

#### Workflow

1. `/forms/create` → design fields → publish → copy public link.
2. Respondent opens `/f/:publicId` → submits.
3. Review `/forms/:id/responses`.

**Test:** Public form submits without login; response count increments.

---

### 8.15 Asset Management

#### What it is

Fixed asset register: equipment, assignment, depreciation tracking.

#### Routes reference

| Function | Route |
|----------|-------|
| List | `/assets/list` |
| Detail | `/assets/list/:id` |

#### Workflow

1. `/assets/list` → Add asset (equipment, cost, location).
2. Assign to user/department.
3. Appears on balance sheet (when integrated with accounting).

**Test:** Asset visible in list; edit persists.

---

## 9. Cross-Module Business Workflows

### 9.1 Retail shop (complete)

```
Procurement PR → PO → Stock GRN → POS sale → Consolidate → Financial statements
```

| Step | Route | Module |
|------|-------|--------|
| 1 | `/procurement/requisitions/create` | Procurement |
| 2 | `/accounting/purchase-orders/create` | Accounting |
| 3 | `/stock/orders/receive` | Stock |
| 4 | `/accounting/sales/create` | Accounting |
| 5 | `/accounting/dashboard` → Consolidate | Accounting |

### 9.2 Pharmacy clinic (Caring Hands pattern)

```
Patient → Appointment → Encounter → Dispense → Invoice → Medical aid claim → Financial dashboard
```

| Step | Route |
|------|-------|
| Register patient | `/clinic/patients` |
| Book visit | `/clinic/appointments` |
| Consult | `/clinic/emr/encounters` |
| Dispense | `/pharmacy/dispense` |
| Debtors | `/accounting/invoices` (filter Pending/Overdue) |
| Scheme money | `/pharmacy/medical-aid-claims` |
| Finance | `/accounting/dashboard` |
| Assets | `/assets/list` |

### 9.3 School term

```
Periods → Enrol → Assess → Marks → Report cards → (optional) Fees invoice
```

### 9.4 Insurance broker

```
Submission → Quote → Placement → Policy → Premium in GL → Claim → Commission statement
```

---

## 10. Shared Tools

### 10.1 Chat / Analyst

| Tool | Route | Purpose |
|------|-------|---------|
| Chat | `/chat` | AI assistant (streaming) |
| Analyst | Header button | Natural language data queries |

**API:** Main API + `chatApiUrl` for chat service.

**Test:** Send message → streamed response; no 401 with valid JWT.

### 10.2 Mobile Scanner

| Route | Purpose |
|-------|---------|
| `/mobile-scanner` | Phone barcode/photo session |

**Requires:** SignalR connection to `/hubs/barcode-scan`; desktop session initiates scan.

**Test:** Open scanner on phone → scan barcode → appears on desktop stock take or POS.

### 10.3 Guides

**Route:** `/guides` — in-app help.

### 10.4 Nudge engine

Contextual tips via `NudgeHub` (`/hubs/nudge`). Background intelligence per module (finance, inventory, school, etc.).

### 10.5 Desktop print bridge

**URL:** `http://127.0.0.1:17890`  
**Purpose:** Silent printing for receipts and labels from desktop deployment.

**Test:** POS sale → receipt prints without browser dialog.

### 10.6 Branch sync (desktop / multi-branch)

**Route:** `/organisation/branch-sync`  
**Purpose:** Offline branch captures outbox events; sync push/pull when online.

**Test:** Create sale offline → sync → appears at head office.

---

## 11. Phase Final — Logout and Session End

### 11.1 What it is

Logout terminates the session: clears JWT, refresh token, and cached user/permission data from `localStorage`; disconnects SignalR hubs.

### 11.2 Manual logout workflow

1. Click profile menu (top-right).
2. Select **Logout**.
3. `AuthService.logout()` calls API if configured; clears local storage.
4. Redirect to `/login`.

**Test:**

| Check | Expected |
|-------|----------|
| After logout | `/login` shown |
| Back button to `/dashboard` | Redirected to login |
| `localStorage.auth_token` | Removed |
| API call with old token | 401 |

### 11.3 Automatic logout triggers

| Trigger | Behaviour |
|---------|-------------|
| 401 + refresh failure | Clear session → `/login` |
| Session timeout (idle) | Warning modal → logout |
| Subscription 402 | May block features; not always full logout |
| Admin deactivates user | Next API call → 401 |

### 11.4 End-of-day checklist (organisation)

| Role | Actions before logout |
|------|----------------------|
| Cashier | Close POS; reconcile till; run Consolidate if daily policy |
| Pharmacist | Clear On Hold dispenses with handover notes |
| Warehouse | Complete in-transit transfer receipts |
| Finance | Import bank statement; record expenses |
| Admin | Review notifications; check branch sync status |

### 11.5 Security notes

- Never share JWT or refresh tokens.
- Use 2FA for admin and finance roles.
- Use **Preview as Role** (profile menu) to verify access before granting production roles.
- Log out on shared workstations.

---

## 12. Global Testing Matrix

### 12.1 Smoke test (any deployment)

Run after deployment or major upgrade (~30 minutes):

| # | Test | Pass criteria |
|---|------|---------------|
| 1 | Register or login | Dashboard loads |
| 2 | Module menu matches activation | Expected menus only |
| 3 | Create product | Appears in stock list |
| 4 | Receive stock | Qty increases |
| 5 | POS sale | Stock ↓; sale in register |
| 6 | Create invoice | `INV-` number; PDF |
| 7 | Permission denial | Viewer cannot delete |
| 8 | Logout/login | Session clean |
| 9 | Public form `/f/:id` | Submits without auth |
| 10 | API health | Swagger or `/health` responds |

### 12.2 Module-specific regression

| Module | Critical path test |
|--------|-------------------|
| Stock | Receive → transfer → stock take |
| Accounting | POS → consolidate → trial balance |
| Procurement | PR → PO → GRN → invoice → payment |
| Pharmacy | Dispense → verify → pay |
| Clinic | Patient → encounter → prescription |
| School | Assessment → marks → report card |
| Insurance | Submission → policy |
| CRM | Lead → opportunity → quote |
| Logistics | Create trip → monitor |
| Documents | Upload → approve → share link |

### 12.3 API verification (optional technical test)

Use Swagger at API root or tools like curl:

```http
POST /api/auth/login
GET  /api/users/me          Authorization: Bearer {token}
GET  /api/organisations/current
GET  /api/products?take=5
```

Expect 200 for authorised endpoints; 401 without token; 403 without permission.

---

## 13. Global Error Catalogue and Fixes

### 13.1 HTTP status codes

| Code | Meaning | User action | Admin action |
|------|---------|-------------|--------------|
| 401 | Unauthenticated | Re-login | Check JWT config, user active |
| 403 | Forbidden | Request access | Assign permission; activate module |
| 402 | Payment required | Open billing | Update subscription |
| 404 | Not found | Check URL/ID | Verify route exists; API version |
| 409 | Conflict | Refresh and retry | Check duplicate business number |
| 500 | Server error | Retry; note time | Check API logs, SQL connection |

### 13.2 UI errors

| Symptom | Likely cause | Fix |
|---------|--------------|-----|
| Blank page after login | JS error; API down | Check browser console; verify API URL in environment |
| Menu item missing | Module off or no permission | Organisation → Modules; Roles |
| "Not authorized" | `permissionGuard` | Assign `module:controller:view` |
| Infinite login redirect | Corrupt localStorage | Clear site data; login again |
| Toast shows raw JSON error | API returned error object | Read `message` field; fix per text |
| Data from wrong org | Token from another tenant | Logout; login to correct org |
| Stale data after edit | Cache not refreshed | Use header refresh button |
| SignalR disconnected | Network/firewall | Allow WebSockets to API |

### 13.3 Data integrity rules

| Rule | Enforcement |
|------|-------------|
| Business numbers permanent | `RX-`, `INV-`, `PO-` at create; status for draft |
| Multi-tenant isolation | `FilteredDbContext` org filter |
| Soft delete | Global filter; no `Deleted == false` in queries needed |
| Creator/org stamping | Automatic on save |

### 13.4 When to escalate

- Repeated 500 errors on core endpoints (`/api/auth/login`, `/api/products`)
- Data visible across organisations (security incident)
- Branch sync ledger out of balance after sync
- Payment gateway (Paynow) failures affecting live billing

---

## 14. Appendices

### Appendix A — Full authenticated route index

See `operations.ui/src/app/app-routing.module.ts` and each `*-routing.module.ts` under `src/app/components/`. Module exploration agent documented 200+ routes — key prefixes:

| Prefix | Module |
|--------|--------|
| `/dashboard` | Dashboards |
| `/stock` | Inventory |
| `/accounting` | Accounting |
| `/procurement` | Procurement |
| `/pharmacy` | Pharmacy |
| `/clinic` | Hospital |
| `/school` | Frame |
| `/hr` | HR |
| `/crm` | CRM |
| `/insurance` | Insurance |
| `/logistics` | Logistics |
| `/agriculture` | Agriculture |
| `/projects` | Projects |
| `/documents` | Documents |
| `/forms` | Forms |
| `/assets` | Assets |
| `/organisation` | Organisation |
| `/account` | Personal account |
| `/chat` | Chat |
| `/notifications` | Notifications |

**Public routes (no auth):** `/login`, `/register`, `/f/:publicId`, `/share/:token`, `/mobile-scanner`, password reset, invite acceptance, student activation.

### Appendix B — Permission format examples

```
stock:inventory:view
stock:inventory:create
sales:invoices:create
pharmacy:dispense:create
school:students:view
crm:leads:create
organisation:roles:assign
*:*:*
```

### Appendix C — Related documentation files

| Path | Content |
|------|---------|
| `operations.ui/src/AGENTS.md` | Dev conventions |
| `Operations.API/wwwroot/data/module-config.json` | Module definitions |
| `Operations.API/wwwroot/default-permissions.md` | Default roles |
| `Operations.API/wwwroot/branding/product-documentation/` | Per-module user guides |
| `Operations.API/wwwroot/data/hms_frontend_workflows.md` | Clinic gap analysis |
| `Operations.API/wwwroot/data/procurement.md` | Procurement implementation |
| `Operations.API/wwwroot/architecture.md` | Architecture audit |
| `clinic-script.md` | Clinic demo script (workflow reference) |

### Appendix D — Default role summary

| Role | Capabilities |
|------|--------------|
| **Administrator** | All permissions; cannot be deleted |
| **Manager** | All except deletes; org view |
| **Viewer** | View only |
| **Student** | School self-service (`school:students:me`) |

### Appendix E — Integration endpoints (admin reference)

| Integration | Config section | Used by |
|-------------|----------------|---------|
| Paynow | `Paynow` in appsettings | Billing, subscriptions |
| WhatsApp | `WhatsApp` | Notifications, flows |
| DeepSeek AI | `DeepSeek` | Chat, analyst, insurance AI |
| Cloudflare R2 | `CloudflareR2` / `Storage` | Documents, attachments |
| Traccar | `traccarConn` | Logistics GPS |
| Google OAuth | `Google` | Login, maps |
| Email (ZeptoMail) | `Email` | Invites, notifications |
| FCM | FCM tokens API | Push notifications |

---

*End of SOP — Pindah Basa / Operations Platform*

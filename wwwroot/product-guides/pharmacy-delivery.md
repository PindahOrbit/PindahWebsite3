# Pharmacy Home Delivery

## Executive Summary

Pharmacy Home Delivery extends your dispensary so medicines can reach patients at home — not only at the counter. A patient order is prepared and verified in **Pharmacy**, then handed off to **Logistics** for routing, driver assignment, live tracking, and proof of delivery.

The system is designed for pharmacies that run online orders, repeat-medication refills, chronic-care programmes, or any model where patients expect medicines brought to their door. It keeps the same patient record, stock, and payment trail as walk-in dispensing, while adding the operational layer needed to run a delivery fleet reliably.

Organisations such as **Trinity Pharmacy** use this model at scale — online orders grouped into trips, drivers completing stops with verified handover, and operations monitored from a central dashboard.

Pharmacy Home Delivery is not a separate product. It is the combined use of **Pharmacy** (dispensing and verification) and **Logistics** (trips, drivers, tracking, proof of delivery), supported by the **Rider mobile app** for field execution.

---

## Who This Is For

| Role | How they use delivery |
|------|------------------------|
| **Dispensary staff** | Prepare and verify orders; decide which are for collection vs delivery; pass ready orders to dispatch |
| **Pharmacists** | Final clinical check before medicines leave the pharmacy |
| **Dispatch / logistics coordinators** | Plan trips, assign drivers, monitor delays, handle failed deliveries |
| **Delivery riders / drivers** | Receive routes on mobile, navigate to patients, confirm handover |
| **Pharmacy managers** | Review delivery volumes, on-time performance, and driver efficiency |
| **Customer service** | Answer "where is my order?" using live trip and stop status |
| **Patients** | Receive medicines at home; may provide an OTP or signature to confirm receipt |

---

## Key Capabilities

- End-to-end fulfilment from dispense to verified home handover
- Multi-stop trip planning with patient addresses and delivery instructions
- Special-handling tags (Fridge, Urgent, Fragile, Chemotherapy, and more)
- Smart driver allocation based on proximity, workload, and performance
- Route optimisation for efficient daily delivery rounds
- Live Command Center map for active trip monitoring
- Proof of delivery via OTP, signature, photo, or initials
- GPS-validated delivery confirmation and full audit trail
- Driver performance scorecards (on-time rate, success rate, completion time)
- Offline-capable rider app with sync when connectivity returns
- Recurring trips for scheduled delivery rounds
- Confidential delivery visibility for sensitive orders

---

## Important Concepts

Before running pharmacy deliveries day to day, it helps to understand these terms:

| Term | Meaning |
|------|---------|
| **Trip** | A driver's run for a period — one vehicle, one driver, one or more stops |
| **Stop** | One patient delivery point on a trip (address, time, instructions, status) |
| **Drop-off** | A stop where medicines are handed to the patient |
| **Proof of delivery (POD)** | Evidence the handover happened — OTP, signature, photo, or initials |
| **OTP** | One-time code the patient gives the driver to confirm receipt |
| **Delivery tag** | A label on a stop that flags special handling (e.g. Fridge, Urgent, Chemotherapy) |
| **Command Center** | Live map and list of active trips for dispatch |
| **On-time performance (OTP rate)** | Share of deliveries completed within the planned window |
| **Smart allocation** | System suggestion for which driver should take a delivery, based on distance, workload, and past performance |

---

## Table of Contents

1. [How It Fits Together](#how-it-fits-together)
2. [The End-to-End Delivery Journey](#the-end-to-end-delivery-journey)
3. [Special Handling for Medicines](#special-handling-for-medicines)
4. [Proof of Delivery](#proof-of-delivery)
5. [Monitoring and Management](#monitoring-and-management)
6. [Collection vs Delivery](#collection-vs-delivery)
7. [How Pharmacy Delivery Connects to Other Modules](#how-pharmacy-delivery-connects-to-other-modules)
8. [Daily Routines and Best Practices](#daily-routines-and-best-practices)
9. [Troubleshooting Common Situations](#troubleshooting-common-situations)

---

## How It Fits Together

Pharmacy and Logistics are two halves of one fulfilment process:

```
Patient order → Dispense & verify → Schedule for delivery → Plan trip → Driver delivers → Proof captured → Order closed
```

| Module | Responsibility |
|--------|----------------|
| **Pharmacy** | Patient identity, medicines, dosing labels, stock deduction, payment |
| **Logistics** | Addresses, trip planning, driver assignment, routing, tracking, proof of delivery |
| **Rider app** | What the driver sees and does in the field |

Walk-in collection and home delivery share the same dispense record. The difference is what happens after **Verified**: the patient either collects at the pharmacy, or the order becomes a delivery stop on a trip.

---

## The End-to-End Delivery Journey

### 1. Order received

An order may come from:

- A walk-in patient who asks for home delivery
- An online or phone order
- A scheduled refill that the patient prefers delivered

The patient must exist in the shared patient register (from Pharmacy or Clinic), with a valid phone number and delivery address.

### 2. Dispense and verify (Pharmacy)

1. Staff dispense medicines in **Quick Dispense** as usual.
2. Labels and dosing instructions are printed.
3. A pharmacist **verifies** the dispense in the queue.
4. Payment is collected or deferred, per your policy.

At this point the medicines are ready to leave the pharmacy. For delivery orders, they are set aside for the dispatch desk rather than the collection counter.

![Screenshot: Verified dispense ready for dispatch — placeholder]

### 3. Handoff to dispatch

Dispatch receives:

- Patient name and contact number
- Delivery address (with map pin where possible)
- Medicines and quantities
- Special instructions (gate code, leave with security, call on arrival)
- Handling flags (refrigerated, fragile, urgent, chemotherapy, etc.)

### 4. Trip planning (Logistics)

1. Open **Logistics → Trips** or the **Command Center**.
2. Create a new trip with:
   - **Origin** — usually the pharmacy branch
   - **Driver** and **vehicle**
   - **Departure time**
3. Add one **stop** per patient delivery:
   - Patient / customer
   - Address (confirmed on map where possible)
   - Planned arrival time
   - Delivery instructions
   - Tags for special handling

Stops can be reordered for an efficient route. Recurring trips can be set up for regular delivery rounds (e.g. daily chronic-care runs).

![Screenshot: Trip planning with multiple patient stops — placeholder]

### 5. Driver assignment

Drivers can be assigned manually or with **smart allocation**, which weighs:

- How far the driver is from the pickup point
- How many deliveries they already have
- Past performance and availability

Nearest available drivers can also be searched when creating a trip.

### 6. Execution in the field (Rider app)

The driver:

1. Goes **online** and receives assigned stops.
2. Sees the optimised route with patient name, address, phone, and instructions.
3. Navigates stop by stop.
4. Marks **Arrived** when at the location.
5. Hands over medicines to the patient or authorised person.
6. Captures **proof of delivery**:
   - Patient enters an **OTP**, or
   - **Signature** or **initials**, or
   - **Photo** of handover (where policy allows)
7. Marks the stop **Delivered** or **Failed** with a reason.

The app works offline where needed: confirmations are stored locally and sync when connectivity returns.

![Screenshot: Rider app stop detail with proof of delivery — placeholder]

### 7. Confirmation and closure

When proof is captured:

- The stop status updates in Logistics.
- Dispatch sees completion on the Command Center.
- Pharmacy can treat the order as **Collected** — the patient has received their medicines.
- Performance metrics (on-time rate, success rate) are recorded for reporting.

### 8. Failed delivery

If the patient is unavailable, the address is wrong, or access is blocked:

1. Driver marks the stop as **failed** and selects a reason.
2. Notes and optional photo are recorded.
3. Dispatch is notified.
4. Pharmacy is informed — medicines typically return to the pharmacy.
5. Customer service reschedules or arranges collection.

---

## Special Handling for Medicines

Pharmacy deliveries often need more care than general parcels. Use **tags** on stops to communicate this to drivers and dispatch:

| Tag | When to use |
|-----|-------------|
| **Fridge** | Medicines that must stay refrigerated |
| **Perishable** | Short shelf-life once out of controlled storage |
| **Fragile** | Glass bottles, delicate packaging |
| **Urgent** | Same-day or time-critical delivery |
| **Chemotherapy** | High-care oncology medicines |
| **Express** | Priority within the day's route |
| **High Value** | Expensive or controlled items needing extra care |

**Confidential deliveries** can be restricted so only authorised roles see trip details — useful for sensitive patient orders.

---

## Proof of Delivery

Every home delivery should leave an audit trail:

- **Who** received the medicines
- **When** and **where** (GPS at confirmation)
- **How** receipt was verified (OTP, signature, photo)

This reduces disputes ("I never received it"), supports regulatory expectations, and gives managers confidence that drivers follow procedure rather than marking deliveries complete without handover.

### Verification methods

| Method | Typical use |
|--------|-------------|
| **OTP** | Patient receives a code by SMS or phone; gives it to the driver at handover |
| **Signature / initials** | Patient signs on the driver's device |
| **Photo** | Driver captures an image of the handover or package at the door |

Your organisation's policy determines which methods are required, optional, or used as fallback.

---

## Monitoring and Management

### Logistics Command Center

Managers and dispatch use the Command Center to:

- See all active trips on a map
- Spot late or stuck deliveries
- Intervene before patients call to complain
- Track daily volume against targets

![Screenshot: Logistics Command Center with active trips — placeholder]

### Key metrics to watch

| Metric | What it tells you |
|--------|-------------------|
| **On-time rate** | Are patients getting medicines when expected? |
| **Completion rate** | How many stops finish successfully vs failed? |
| **Proof capture rate** | Are drivers confirming handover properly? |
| **Average time per stop** | Is the route plan realistic? |
| **Driver performance score** | Who is consistently reliable? |

---

## Collection vs Delivery

| | **Walk-in collection** | **Home delivery** |
|--|------------------------|-------------------|
| Patient comes to pharmacy | Yes | No |
| Logistics trip needed | No | Yes |
| Proof of delivery | Counter handover | OTP / signature / photo |
| Status path | Verified → Collected | Verified → Out for delivery → Collected |
| Best for | Immediate pickup, OTC, urgent counter cases | Chronic patients, online orders, mobility limits |

---

## How Pharmacy Delivery Connects to Other Modules

### Pharmacy

- Same patient, stock, and dispense records as walk-in.
- Dispense moves through **Dispensed → Verified** before delivery.
- After successful delivery, status can move to **Collected** and **Paid**.

### Clinic / Healthcare

- Shared patient register — allergies and conditions visible during dispensing.
- Clinic does not auto-create delivery trips today — your local process links clinical orders to pharmacy fulfilment.

### Inventory & Stock

- Stock is deducted when the dispense is saved (FEFO — earliest expiry first).
- Returned failed deliveries may need a stock return process per your policy.

### Accounting

- Payment can be taken at dispense or on delivery, depending on setup.
- Delivery operations are tracked in Logistics; revenue sits in Pharmacy/Accounting.

### HR & Performance

- Driver delivery metrics feed into performance scorecards.
- Useful for incentives, reviews, and identifying training needs.

---

## Daily Routines and Best Practices

### Pharmacy (start of day)

1. Review orders marked for delivery today.
2. Process and verify refills due for delivery.
3. Hand verified, packaged orders to dispatch with clear instructions.

### Dispatch (start of day)

1. Open the **Logistics Command Center**.
2. Group today's delivery orders into trips by area and urgency.
3. Assign drivers; prioritise **Urgent** and **Fridge** stops.
4. Confirm patient phone numbers for drivers.

### During the day

- Monitor active trips; call patients ahead of refrigerated deliveries if needed.
- Handle failed stops quickly — reschedule same day where possible.
- Keep pharmacy informed of returns.

### End of day

1. Confirm all trips are **completed** or **cancelled**.
2. Reconcile failed deliveries with pharmacy (stock returned, patient contacted).
3. Review on-time rate and failed-stop reasons for tomorrow's planning.

### What good looks like

A well-run pharmacy delivery operation:

- Verifies every order in Pharmacy **before** it leaves the building
- Groups stops into sensible geographic trips
- Tags special-handling orders clearly
- Requires proof on every successful handover
- Monitors live trips instead of waiting for complaints
- Closes the loop with Pharmacy when delivery succeeds or fails

---

## Troubleshooting Common Situations

| Situation | What to do |
|-----------|------------|
| Patient says order never arrived | Check stop status and proof record; confirm GPS and OTP timestamp |
| Driver cannot find address | Verify map pin; call patient; update stop instructions for future |
| Refrigerated medicine out too long | Treat as failed delivery; return to pharmacy; assess product viability per policy |
| Patient not home | Driver marks failed; dispatch reschedules or offers collection |
| Wrong medicines prepared | Do not dispatch — correct dispense in Pharmacy before adding to trip |
| Duplicate delivery created | Check if dispense already has a completed stop; avoid double dispatch |
| Driver app offline | Driver continues with cached route; proofs sync when back online |
| OTP not working | Regenerate code; fall back to signature or photo per policy |

---

## Related Documentation

- [Pharmacy](pharmacy.md) — dispensing, refills, patient register, queue workflow
- [Logistics](logistics.md) — trips, drivers, vehicles, Command Center
- [Healthcare (Clinic)](clinic-healthcare.md) — shared patient records and clinical context
- [Inventory & Stock](inventory-stock.md) — product catalogue, locations, batches
- [Accounting & Sales](accounting.md) — payment methods, sales register, reconciliation

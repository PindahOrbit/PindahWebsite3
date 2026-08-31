# Prompt: Restructure PindahWebsite3 to Follow the Infor.com Information Architecture

## Objective

Restructure the navigation, homepage layout, and module-page template of **PindahWebsite3** (ASP.NET Core 8 MVC monolith) so its *structure* mirrors Infor.com's proven ERP-marketing pattern — industry-first navigation, stacked social proof, and a single repeatable module template — while changing **nothing else**.

This is a structural refactor, not a redesign. Do not write new copy, do not introduce a new visual identity, and do not add new CSS classes or a new class taxonomy. Every instruction below is scoped to *reordering, consolidating, and re-skinning existing elements*.

---

## Non-Negotiable Constraints

1. **No new content.** Every heading, paragraph, feature bullet, and CTA label that exists today must be reused as-is. If a section needs a heading it doesn't currently have, reuse the nearest existing heading/label rather than inventing new copy.
2. **No color changes.** The existing palette (primary/secondary/accent/background/text colors, as currently defined in `site.css` / `:root` variables) stays exactly as-is. Do not introduce new hex values, new CSS variables for color, or new theme tokens.
3. **Bootstrap classes only — no new classes.** The project uses Bootstrap utility and component classes (`container`, `row`, `col-*`, `card`, `navbar`, `btn`, `btn-primary`, `hero`, etc. — whatever is already in use across the Razor views). You may:
   - Reorder existing markup blocks.
   - Reuse existing classes on different elements.
   - Override the **underlying CSS rule** of an existing class (e.g., change padding, spacing, grid gap, or breakpoint behavior *inside* the existing `.card` or `.navbar` selector in `site.css`) to achieve Infor's denser, grid-based visual rhythm.
   You may **not**:
   - Add a new class name anywhere (HTML or CSS).
   - Pull in a new Bootstrap component not already used elsewhere in the project (e.g., don't introduce `carousel` if it isn't already used; reuse `card`/`grid` patterns instead).
   - Add new JS-driven UI patterns (accordions, tabs, carousels) unless that pattern already exists somewhere in the codebase and is simply being reused.
4. **No new database entities, controllers, or routes** unless explicitly called out below as a template consolidation (see Deliverable 3) — and even then, this consolidates *existing* controllers/views, it doesn't add new ones.
5. **SEO instrumentation stays intact.** Canonical tags, OG/JSON-LD, `SeoLandingCatalog`, `SitemapController`, and the `{slug}` catch-all route must continue to function exactly as they do today. Reordering visual sections must not change URLs, route names, or meta output.

---

## Reference Pattern (from Infor.com structural study)

Infor's structure, which this refactor should mirror:

1. **Top utility bar** — low-weight secondary links (Resources, Blog, Careers, Customer Center).
2. **Main nav ordered by "who you are" before "what you buy"** — Industries → Products → Platform → Services → Partners → About.
3. **Persistent dual CTA** in the header — one primary ("Contact Us") + one secondary ("Watch Demo" / "Request Demo").
4. **Homepage flow, top to bottom:**
   - Hero with flagship positioning statement.
   - Category-defining message block (one big differentiating claim).
   - Third-party/analyst credibility block, placed *before* customer logos.
   - Customer logos / case-study strip.
   - Industry or module grid — icon + one-line value prop per tile, uniform card shape.
   - Product/solution pillars — clean equal-weight cards, each a doorway to a deeper page.
   - Repeated credibility signals (reports, quotes).
   - Resources/content teasers (top-of-funnel).
   - Footer mirrors main nav + legal links.
5. **One dominant conversion action repeated top and bottom**, not diluted by competing CTAs.
6. **Every card/tile follows an identical anatomy**: icon → headline → one-sentence value prop → link. This uniformity is what should replace PindahWebsite3's current per-module inline markup.

---

## Mapping: Current PindahWebsite3 Structure → Target Structure

| Current element | Target position under new structure |
|---|---|
| `HomeController/Index` hero section | Hero (position 1) — reuse existing hero copy/CTA, restyle via existing `.hero`/`btn` classes only |
| Module controllers (Erp, Crm, Hr, Hospital, Sms, Scm, etc.) | Become the **"Industries/Products grid"** (position 5) on the homepage — one uniform card per module, linking to each module's existing `Index.cshtml` |
| Any existing testimonial/case-study content (if present in views) | Move to **credibility/case-study strip** (position 4), before the module grid |
| Existing analyst/press/partner mentions (if present anywhere in views) | Move to **credibility block** (position 3), immediately after hero |
| News/blog listing | Becomes the **Resources teaser** section (position 8) — reuse existing `News` cards, same classes, just relocated and capped to a small featured count on the homepage |
| SOP, Downloads, Zimsec | Remain out of the public homepage flow exactly as today (Infor doesn't surface internal/auth-gated tools on its homepage either) — no structural change to these subsystems |
| Existing footer nav | Restructure the *order* of footer link groups to mirror main-nav order (Industries/Products/Platform/Services/Partners/About, then legal), using the same footer markup/classes already present |
| Existing top navbar links | Reorder only — group into: utility bar (secondary links) + main nav (module/industry links ordered first) + persistent CTA button(s) already used elsewhere in the site |

---

## Deliverables

### 1. Navigation restructure (`_Layout.cshtml`)
- Split the current navbar into two tiers if two tiers already exist in some form (header links vs. main links); if only one tier exists, keep one tier but reorder link sequence so module/industry links come first, informational links (About, Careers, Blog if present) come last.
- Ensure exactly one primary CTA button (`btn-primary` or whatever the existing primary button class is) and, if a secondary CTA already exists elsewhere in the codebase (e.g., "Watch Demo," "Learn More"), reuse that exact class combination in the header too.
- No new nav items — only reordering and, where multiple existing modules already imply a grouping, wrapping them under an existing dropdown component *if a dropdown component is already used elsewhere in the project*. If no dropdown pattern currently exists, do not introduce one — keep a flat reordered list instead.

### 2. Homepage section reorder (`Views/Home/Index.cshtml`)
Reassemble existing partials/sections (do not rewrite their internal content) into this order:
1. Hero
2. Category-defining statement block (reuse the strongest existing value-proposition copy already on the homepage)
3. Credibility block (reuse any existing trust signals — partner logos, stats, testimonials — if none exist in current content, omit this section entirely rather than inventing one)
4. Case studies / customer proof (reuse existing content if present; omit if none exists)
5. Module grid (this replaces scattered module links with one uniform grid — see Deliverable 3)
6. Product/solution pillars (if the site already separates "modules" from "solutions," this is a second, distinct grid; if not, skip and let the module grid in step 5 serve this role)
7. Resources/News teaser (reuse existing `News` component, same classes, limit to existing "featured/latest" logic already in the codebase)
8. Closing CTA band (reuse hero's CTA styling)

### 3. Module page/card template consolidation
- Introduce **one shared partial view or view component** (e.g., `_ModuleCard.cshtml`) that all 12+ module tiles render through on the homepage grid, using only classes already present across the existing module views (`card`, `card-body`, `card-title`, etc. — whatever is already standard in the project).
- Each module's own landing page (`ErpController/Index`, `CrmController/Index`, etc.) stays as-is in content; only the **homepage teaser card** pointing to it is consolidated into the shared partial. This directly addresses the "150+ near-identical views" duplication noted in the structural analysis, without touching the module pages' own content.
- Do not change the underlying controllers' actions or routes — only how they're *linked to* from the homepage grid.

### 4. CSS-only visual alignment (`site.css` or equivalent)
For each existing class listed below (adjust names to match what's actually defined in the project), override only the **declarations inside the existing rule** to achieve Infor's denser, evenly-spaced grid feel — do not rename, do not duplicate, do not add modifier classes:
- `.navbar` — tighten vertical padding, ensure consistent height across breakpoints.
- `.hero` — increase visual weight of the primary headline (font-size/line-height only, same font-family and same colors).
- `.card` (as used for modules/industries) — enforce equal height and consistent internal padding so the grid reads as uniform tiles, matching Infor's icon → headline → one-liner → link anatomy.
- `.btn-primary` / `.btn-secondary` — no color change; only ensure consistent sizing/padding so the same two CTA styles are used identically in the header and homepage closing band.
- Grid/row utilities (`.row`, `.col-*`) — no new breakpoints; only adjust `gap`/`gutter` values inside existing custom overrides if such overrides already exist in `site.css`.

### 5. Sitemap/SEO safety check
- Confirm `SitemapController` output and `SeoLandingCatalog` entries are unaffected by the homepage reorder (they reference URLs, not homepage position, so this should be a non-issue — but verify no module route was renamed in the process).
- Confirm canonical/OG tags in `_Layout.cshtml` are untouched by the nav restructure.

---

## Acceptance Criteria

- [ ] Homepage sections appear in the order specified above, using only existing content blocks.
- [ ] Navbar is reordered (industries/modules before informational links); no new nav items added.
- [ ] All module homepage teasers render through one shared partial/view component instead of duplicated inline markup.
- [ ] No new CSS class names exist anywhere in the diff.
- [ ] No new hex colors or color variables exist anywhere in the diff.
- [ ] No copy differs from what exists today (a text-only diff of visible strings should be empty).
- [ ] All existing routes, controller actions, and the `{slug}` SEO catch-all continue to resolve exactly as before.
- [ ] Sitemap.xml output is unchanged in URL set.
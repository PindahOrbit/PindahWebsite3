# Pindah Private Limited — SEO Agent Instructions
**Stack: ASP.NET Core MVC | Platform: pindah.org**  
**Goal: Full Google indexation of all public pages, including sitelinks and indented indexing**

---

## AGENT RULES — ALWAYS ENFORCE THESE

When working on any file, view, controller, layout, or configuration in this codebase, apply every applicable rule below without being asked.

---

## 1. HTTPS & URL Hygiene

- All URLs must be lowercase. Enforce `options.LowercaseUrls = true` in `AddRouting()`.
- No trailing slashes. Enforce `options.AppendTrailingSlash = false`.
- HTTPS redirect must be active (`UseHttpsRedirection()` + `UseHsts()`).
- www and non-www must 301 redirect to one consistent base URL (`https://pindah.org`).
- Every URL must be reachable within 3 clicks from the homepage.

---

## 2. HTTP Status Codes

- Public indexable pages: **200**
- Permanently moved pages: **301** (never 302 unless intentionally temporary)
- Non-existent pages: **404** via `UseStatusCodePagesWithReExecute("/errors/{0}")`
- Noindex pages still return **200** — not 404
- Maintenance: **503** with `Retry-After` header

---

## 3. robots.txt

File lives at `wwwroot/robots.txt`. Always include:

```
User-agent: *
Allow: /
Disallow: /admin/
Disallow: /api/
Disallow: /account/
Disallow: /errors/
Allow: /css/
Allow: /js/
Allow: /images/
Sitemap: https://pindah.org/sitemap.xml
```

Never disallow CSS, JS, or image folders — Googlebot must render pages fully.

---

## 4. XML Sitemaps

- Serve a **sitemap index** at `/sitemap.xml` pointing to child sitemaps.
- Child sitemaps: `/sitemap-pages.xml`, `/sitemap-blog.xml`, `/sitemap-products.xml` (or whatever sections exist).
- Every public, indexable page must appear in a sitemap.
- Include `<lastmod>` using actual last-modified dates from the database or file system.
- Include `<changefreq>` and `<priority>` as hints.
- Sitemaps must be generated dynamically via a controller — never static files, because content changes.
- Register all sitemap URLs in Google Search Console.

Priority scale:
- Homepage: 1.0
- Core product/service pages: 0.9
- Secondary content pages: 0.7–0.8
- Blog posts: 0.6–0.7
- Legal pages: 0.3

---

## 5. Canonical URLs

- Every page must emit one `<link rel="canonical" href="...">` tag in `<head>`.
- Canonical must match the page's exact intended URL — no query strings unless the query string defines unique content (e.g. `?page=2`).
- Set canonicals via `ViewData["CanonicalUrl"]` in each controller action or at the top of each view.
- Render in `_Layout.cshtml`:  
  `<link rel="canonical" href="@ViewData["CanonicalUrl"]" />`
- Paginated pages: page 1 canonical = base URL, page 2+ canonical = base URL + `?page=N`.

---

## 6. `<head>` Meta Tags

Every page must have all of the following in `_Layout.cshtml`, populated via `ViewData`:

```html
<title>@ViewData["Title"] | Pindah</title>
<meta name="description" content="@ViewData["MetaDescription"]" />
<meta name="robots" content="@(ViewData["Robots"] ?? "index, follow")" />
<link rel="canonical" href="@ViewData["CanonicalUrl"]" />
```

Rules:
- `Title`: 50–60 characters, unique per page, keyword first, brand last.
- `MetaDescription`: 140–160 characters, unique per page, includes primary keyword and a value proposition.
- `Robots`: default to `index, follow`. Use `noindex, nofollow` for account/admin pages. Use `noindex, follow` for thank-you and confirmation pages.
- Never leave `ViewData["Title"]` or `ViewData["MetaDescription"]` unset on any public page.

---

## 7. Open Graph & Twitter Card Tags

Render in `_Layout.cshtml` for every page:

```html
<meta property="og:title" content="@ViewData["Title"]" />
<meta property="og:description" content="@ViewData["MetaDescription"]" />
<meta property="og:url" content="@ViewData["CanonicalUrl"]" />
<meta property="og:image" content="@(ViewData["OgImage"] ?? "https://pindah.org/images/og-default.jpg")" />
<meta property="og:type" content="@(ViewData["OgType"] ?? "website")" />
<meta property="og:site_name" content="Pindah Private Limited" />
<meta property="og:locale" content="en_ZW" />
<meta name="twitter:card" content="summary_large_image" />
<meta name="twitter:title" content="@ViewData["Title"]" />
<meta name="twitter:description" content="@ViewData["MetaDescription"]" />
<meta name="twitter:image" content="@(ViewData["OgImage"] ?? "https://pindah.org/images/og-default.jpg")" />
```

OG image must be at least 1200×630px, under 1MB, hosted on the same domain.

---

## 8. Structured Data (JSON-LD)

Inject via `ViewData["StructuredData"]` rendered in `_Layout.cshtml`:

```html
@if (ViewData["StructuredData"] != null)
{
    <script type="application/ld+json">@Html.Raw(ViewData["StructuredData"])</script>
}
```

### Required schemas per page type:

**Homepage:**
- `Organization` — company name, URL, logo, address, phone, email, sameAs social links
- `WebSite` — with `SearchAction` potentialAction pointing to a working `/search?q=` endpoint

**Every product/service page:**
- `SoftwareApplication` — name, description, applicationCategory, offers, featureList, publisher
- `BreadcrumbList` — full path from homepage to current page
- `FAQPage` — if the page contains Q&A content

**Every blog post:**
- `Article` — headline, datePublished, dateModified, author, publisher, image
- `BreadcrumbList`

**Contact page:**
- `LocalBusiness` or `Organization` with full address, phone, email, openingHours

**All pages:**
- `BreadcrumbList` on any page that is Tier 2 or deeper

Never use Microdata or RDFa — JSON-LD only.  
Validate all schemas at https://search.google.com/test/rich-results before deploying.

---

## 9. Heading Hierarchy

- Every page has exactly **one `<h1>`** that contains the primary keyword for that page.
- `<h1>` must not be in the shared layout — it belongs in each individual view.
- Heading order must be sequential: `h1 → h2 → h3`. Never skip levels.
- Navigation elements, footer links, and sidebar items must not use heading tags.

---

## 10. Internal Linking

- Every page must have at least one inbound internal link from another page.
- Anchor text must be descriptive and keyword-relevant — never "click here" or "read more".
- Product pages must link to each other where relevant.
- Blog posts must link to the relevant product or solution page they reference.
- Every page must link back to the homepage via the logo or a breadcrumb.
- Breadcrumb navigation must be present on all Tier 2 and deeper pages, both visually and as `BreadcrumbList` schema.

---

## 11. Image SEO

- Every `<img>` tag must have a non-empty, descriptive `alt` attribute.
- `alt` text must describe the image content — not be a keyword dump.
- Decorative images: `alt=""` (empty, not missing).
- Image filenames must be lowercase, hyphenated, and descriptive (e.g. `pindah-basa-dashboard.jpg`).
- Serve images in WebP format with JPEG/PNG fallback.
- All images must have explicit `width` and `height` attributes to prevent layout shift (CLS).
- Use `loading="lazy"` on all images below the fold. Do not lazy-load above-the-fold images.

---

## 12. Page Speed & Core Web Vitals

Enforce all of the following:

- **LCP (Largest Contentful Paint):** < 2.5s. Hero image or heading must load fast. Preload the LCP image: `<link rel="preload" as="image" href="...">`.
- **CLS (Cumulative Layout Shift):** < 0.1. All images and embeds must have explicit dimensions.
- **INP (Interaction to Next Paint):** < 200ms. Minimize main thread blocking JS.
- Enable response compression: `app.UseResponseCompression()` with Brotli + Gzip.
- Enable response caching for static assets with long `Cache-Control` headers (1 year for versioned assets).
- Bundle and minify CSS and JS. Use ASP.NET Core's built-in bundling or a build pipeline.
- Remove unused CSS. Do not load full Bootstrap if only using a subset.
- Fonts: use `font-display: swap`. Preconnect to Google Fonts if used: `<link rel="preconnect" href="https://fonts.googleapis.com">`.

---

## 13. URL Structure

- URLs must be human-readable and keyword-rich.
- Use hyphens as word separators — never underscores or spaces.
- No query strings in URLs intended for indexing (except pagination).
- Route structure must mirror site architecture hierarchy.
- Avoid numeric IDs in URLs for content pages (use slugs).
- Example: `/blog/erp-software-zimbabwe-guide` not `/blog?id=42`.

In `Program.cs`:
```csharp
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
    options.AppendTrailingSlash = false;
});
```

---

## 14. Sitelinks & Indented Indexing — How to Earn Them

Sitelinks (the sub-links shown under the main result in Google) and indented indexing (where two results from the same domain are shown stacked) are awarded by Google — not configured. But the following signals are required to earn them:

1. **Strong brand signal:** The homepage `<title>` must start with or prominently feature the brand name.
2. **Clear site architecture:** Navigation must be consistent, logical, and present on every page.
3. **BreadcrumbList schema on all subpages:** This signals page hierarchy to Google.
4. **Internal links use consistent anchor text** for key pages across the site.
5. **Homepage links to all major sections** in the main navigation.
6. **XML sitemap submitted to Search Console** and all pages returning 200.
7. **Organization schema on the homepage** with correct `url`, `name`, and `logo`.
8. **WebSite schema with SearchAction** on the homepage.
9. **High click-through rate on brand queries** — title and description must be compelling.
10. **Zero crawl errors in Search Console** — fix all 404s and redirect chains.

---

## 15. Rendering — No SPA on Public Pages

- Public, indexable pages must be rendered **server-side** (Razor Views in ASP.NET Core MVC). 
- Do not render public content via Angular, React, or any JS framework that requires client-side execution for Googlebot to see content.
- If Angular is used for app sections (dashboards, portals), those sections must be behind login and marked `noindex`.
- Prerendering or SSR must be implemented if any JS framework is used on public-facing pages.
- Verify rendering by fetching pages with `curl` and confirming all content is present in the raw HTML — not injected by JS.

---

## 16. Pagination

- Use `?page=N` query parameter consistently.
- Page 1 must be accessible at the base URL (without `?page=1`) and canonical must point to the base URL.
- Do not use `rel="next"` / `rel="prev"` — Google deprecated these. Use sitemaps and internal links instead.
- Each paginated page must have a unique `<title>` (e.g., `Blog | Page 2 | Pindah`).
- Do not noindex paginated pages unless content is truly duplicate.

---

## 17. `noindex` Rules

Apply `noindex` to:
- `/account/*` (login, register, profile management)
- `/admin/*`
- `/api/*`
- `/errors/*`
- Thank-you / confirmation pages (e.g., `/contact/success`)
- Internal search results pages (unless they have unique, valuable content)
- Print versions of pages

Never apply `noindex` to:
- Any page linked from the sitemap
- Any page with unique, valuable content
- Any page you want Google to discover

---

## 18. Google Search Console

- Verify ownership via HTML meta tag method (inject in `_Layout.cshtml` under `<head>`).
- Submit all sitemap URLs.
- Monitor Coverage report weekly — fix all Errors and Warnings.
- Monitor Core Web Vitals report monthly.
- Use URL Inspection tool to force-crawl any newly published or updated page.
- Set the preferred domain (non-www) in Search Console settings.

---

## 19. What Never to Do

- Never block CSS, JS, or image folders in `robots.txt`.
- Never use `meta name="robots" content="noindex"` on pages in the sitemap.
- Never use 302 redirects for permanent moves.
- Never duplicate `<title>` or `<meta name="description">` across pages.
- Never render indexable content exclusively via JavaScript.
- Never use `<iframe>` to load critical page content.
- Never stuff keywords into `alt`, `title`, or meta tags.
- Never have orphan pages (pages with zero internal links pointing to them).
- Never let internal links point to redirected or broken URLs.
- Never use session IDs or tokens in public-facing URLs.
- Never place `<h1>` in `_Layout.cshtml` — it belongs in individual views only.
- Never omit `width` and `height` attributes on images.
- Never use generic anchor text like "click here", "read more", or "learn more".

---

## 20. Per-Page ViewData Checklist

Every controller action serving a public page must set all of the following before returning the view:

```csharp
ViewData["Title"]           = "...";   // 50–60 chars, unique, keyword-first
ViewData["MetaDescription"] = "...";   // 140–160 chars, unique, keyword + CTA
ViewData["CanonicalUrl"]    = "...";   // Full absolute URL, no trailing slash
ViewData["OgImage"]         = "...";   // Absolute URL, 1200×630px WebP/JPG
ViewData["StructuredData"]  = "...";   // JSON-LD string for this page type
// ViewData["Robots"] defaults to "index, follow" — only set if overriding
```

---

*End of SEO Agent Instructions — apply all rules to every relevant file in this project.*
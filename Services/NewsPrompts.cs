namespace PindahWebsite3.Services;

public static class NewsPrompts
{
    public const string Heading = """
        Generate one engaging, SEO-optimized heading for a business software blog. Focus on these enterprise modules: ERP (finance, inventory, procurement), CRM, Manufacturing, Insurance, Accounting, Logistics, HR, Hospital Management, DMS, Construction, SCM. Include real-world case studies, implementation lessons, ROI metrics, or industry trends. Return ONLY the heading text with no quotes, no numbering, and no extra text.
        """;

    public static string Content(string heading) => $"""
        Write a detailed, engaging 400-600 word blog post based on this heading: "{heading}".
        Include real-world examples, case studies, or implementation insights. Reference enterprise software modules like ERP, CRM, Manufacturing, Insurance, etc.
        Mention specific ROI metrics, efficiency gains, or business outcomes.
        Write for business leaders and IT managers.

        Return ONLY valid HTML body content with no wrapper tags (no html, head, or body). Use:
        - <h2> for main sections
        - <h3> for subsections
        - <p> for paragraphs
        - <ul>/<ol> with <li> for lists
        - <strong> and <em> for emphasis
        - <blockquote> for key insights or quotes

        Do not use markdown, code fences, or meta commentary. Return only the HTML article body.
        """;

    public static string ImageKeyword(string heading) => $"""
        Based on this article heading: "{heading}", suggest a single relevant search keyword (one or two words) for finding a cover image. Return ONLY the keyword with no extra text.
        """;

    public static string ReviseHeading(string currentHeading, string instruction) => $"""
        You are editing a business software blog heading.

        Current heading:
        "{currentHeading}"

        User instruction:
        "{instruction}"

        Rewrite the heading according to the instruction. Keep it clear, professional, and SEO-friendly.
        Return ONLY the revised heading text with no quotes, no numbering, and no extra text.
        """;

    public static string ReviseContent(string heading, string currentHtml, string instruction) => $"""
        You are editing an HTML business software blog article.

        Article heading:
        "{heading}"

        Current HTML body:
        {currentHtml}

        User instruction:
        "{instruction}"

        Revise the article according to the instruction while preserving useful structure and business tone.
        Return ONLY valid HTML body content with no wrapper tags (no html, head, or body). Use only sensible article tags such as <h2>, <h3>, <p>, <ul>, <ol>, <li>, <strong>, <em>, <blockquote>, and <a>.
        Do not use markdown, code fences, or meta commentary. Return only the revised HTML article body.
        """;
}

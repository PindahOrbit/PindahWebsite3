using Google.GenAI;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models;
using Quartz;
using System.Text.RegularExpressions;

namespace PindahWebsite3.Jobs;

public class NewsGenerationJob : IJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<NewsGenerationJob> _logger;

    public NewsGenerationJob(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<NewsGenerationJob> logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Starting news generation job at {Time}", DateTime.UtcNow);

        var apiKey = _configuration["Gemini:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("Gemini API key not configured");
            return;
        }

        var client = new Client(apiKey: apiKey);

        // Step 1: Generate heading
        var heading = await GenerateHeadingAsync(client);
        if (string.IsNullOrEmpty(heading))
        {
            _logger.LogError("Failed to generate heading");
            return;
        }

        // Step 2: Generate content for the heading
        var content = await GenerateContentAsync(client, heading);
        if (string.IsNullOrEmpty(content))
        {
            _logger.LogError("Failed to generate content for heading: {Heading}", heading);
            return;
        }

        // Step 3: Generate slug
        var slug = GenerateSlug(heading);

        // Step 4: Generate cover image keyword
        var imageKeyword = await GenerateImageKeywordAsync(client, heading);
        var coverImageUrl = $"https://loremflickr.com/800/600/{Uri.EscapeDataString(imageKeyword)}";

        // Step 5: Save to database
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PindahWebsite3Context>();

        // Ensure slug is unique
        var originalSlug = slug;
        int counter = 1;
        while (await dbContext.News.AnyAsync(n => n.Slug == slug, context.CancellationToken))
        {
            slug = $"{originalSlug}-{counter}";
            counter++;
        }

        var news = new News
        {
            Heading = heading,
            Content = content,
            Slug = slug,
            DateCreated = DateTime.UtcNow,
            CoverImageUrl = coverImageUrl
        };

        dbContext.News.Add(news);
        await dbContext.SaveChangesAsync(context.CancellationToken);

        _logger.LogInformation("News article generated and saved: {Slug}", slug);
    }

    private async Task<string> GenerateHeadingAsync(Client client)
    {
        var prompt = @"Generate one engaging, SEO-optimized heading for a business software blog. Focus on these enterprise modules: ERP (finance, inventory, procurement), CRM, Manufacturing, Insurance, Accounting, Logistics, HR, Hospital Management, DMS, Construction, SCM. Include real-world case studies, implementation lessons, ROI metrics, or industry trends. Return ONLY the heading text with no quotes, no numbering, and no extra text.";

        try
        {
            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3-flash-preview",
                contents: prompt
            );

            if (response?.Candidates?.Count > 0 && response.Candidates[0].Content?.Parts?.Count > 0)
            {
                var text = response.Candidates[0].Content.Parts[0].Text?.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    // Remove quotes if present
                    text = text.Trim('"', '\'', '`');
                    return text;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating heading");
        }

        return string.Empty;
    }

    private async Task<string> GenerateContentAsync(Client client, string heading)
    {
        var prompt = $"Write a detailed, engaging 400-600 word blog post based on this heading: \"{heading}\". " +
            "Include real-world examples, case studies, or implementation insights. Reference enterprise software modules like ERP, CRM, Manufacturing, Insurance, etc. " +
            "Mention specific ROI metrics, efficiency gains, or business outcomes. Structure with clear paragraphs. " +
            "Write for business leaders and IT managers. Return ONLY the article content with no meta commentary, no markdown headers, and no extra text.";

        try
        {
            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3-flash-preview",
                contents: prompt
            );

            if (response?.Candidates?.Count > 0 && response.Candidates[0].Content?.Parts?.Count > 0)
            {
                var text = response.Candidates[0].Content.Parts[0].Text?.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    return text;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating content for heading: {Heading}", heading);
        }

        return string.Empty;
    }

    private async Task<string> GenerateImageKeywordAsync(Client client, string heading)
    {
        var prompt = $"Based on this article heading: \"{heading}\", suggest a single relevant search keyword (one or two words) for finding a cover image. Return ONLY the keyword with no extra text.";

        try
        {
            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3-flash-preview",
                contents: prompt
            );

            if (response?.Candidates?.Count > 0 && response.Candidates[0].Content?.Parts?.Count > 0)
            {
                var text = response.Candidates[0].Content.Parts[0].Text?.Trim().ToLowerInvariant();
                if (!string.IsNullOrEmpty(text))
                {
                    // Clean up - remove spaces, keep only alphanumeric and hyphens
                    text = Regex.Replace(text, "[^a-z0-9-]", "", RegexOptions.IgnoreCase);
                    return text.Length > 0 ? text : "technology";
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating image keyword for heading: {Heading}", heading);
        }

        return "technology";
    }

    private static string GenerateSlug(string heading)
    {
        var slug = heading.ToLowerInvariant();
        // Remove non-alphanumeric characters (except spaces and hyphens)
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        // Replace spaces with hyphens
        slug = Regex.Replace(slug, @"\s+", "-");
        // Remove multiple hyphens
        slug = Regex.Replace(slug, @"-+", "-");
        // Trim hyphens
        slug = slug.Trim('-');
        // Limit length
        if (slug.Length > 80)
        {
            slug = slug.Substring(0, 80).TrimEnd('-');
        }
        // Append short hash for uniqueness
        var hash = Guid.NewGuid().ToString("N")[..6];
        slug = $"{slug}-{hash}";
        return slug;
    }
}

using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models;
using PindahWebsite3.Services;
using Quartz;
using System.Text.RegularExpressions;

namespace PindahWebsite3.Jobs;

public class NewsGenerationJob : IJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly OllamaChatService _ollamaChatService;
    private readonly ILogger<NewsGenerationJob> _logger;

    public NewsGenerationJob(
        IServiceProvider serviceProvider,
        OllamaChatService ollamaChatService,
        ILogger<NewsGenerationJob> logger)
    {
        _serviceProvider = serviceProvider;
        _ollamaChatService = ollamaChatService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Starting news generation job at {Time}", DateTime.UtcNow);

        var heading = await GenerateHeadingAsync(context.CancellationToken);
        if (string.IsNullOrEmpty(heading))
        {
            _logger.LogError("Failed to generate heading");
            return;
        }

        var content = await GenerateContentAsync(heading, context.CancellationToken);
        if (string.IsNullOrEmpty(content))
        {
            _logger.LogError("Failed to generate content for heading: {Heading}", heading);
            return;
        }

        var slug = GenerateSlug(heading);
        var imageKeyword = await GenerateImageKeywordAsync(heading, context.CancellationToken);
        var coverImageUrl = $"https://loremflickr.com/800/600/{Uri.EscapeDataString(imageKeyword)}";

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PindahWebsite3Context>();

        var originalSlug = slug;
        var counter = 1;
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

    private async Task<string> GenerateHeadingAsync(CancellationToken cancellationToken)
    {
        try
        {
            var text = await _ollamaChatService.GenerateAsync(NewsPrompts.Heading, cancellationToken);
            if (!string.IsNullOrEmpty(text))
            {
                return text.Trim('"', '\'', '`');
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating heading");
        }

        return string.Empty;
    }

    private async Task<string> GenerateContentAsync(string heading, CancellationToken cancellationToken)
    {
        try
        {
            return await _ollamaChatService.GenerateAsync(NewsPrompts.Content(heading), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating content for heading: {Heading}", heading);
        }

        return string.Empty;
    }

    private async Task<string> GenerateImageKeywordAsync(string heading, CancellationToken cancellationToken)
    {
        try
        {
            var text = await _ollamaChatService.GenerateAsync(NewsPrompts.ImageKeyword(heading), cancellationToken);
            if (!string.IsNullOrEmpty(text))
            {
                text = Regex.Replace(text.Trim().ToLowerInvariant(), "[^a-z0-9-]", "", RegexOptions.IgnoreCase);
                return text.Length > 0 ? text : "technology";
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
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"-+", "-");
        slug = slug.Trim('-');

        if (slug.Length > 80)
        {
            slug = slug[..80].TrimEnd('-');
        }

        var hash = Guid.NewGuid().ToString("N")[..6];
        return $"{slug}-{hash}";
    }
}

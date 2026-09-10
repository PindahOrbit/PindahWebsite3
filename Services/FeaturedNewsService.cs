using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models;

namespace PindahWebsite3.Services;

public class FeaturedNewsService
{
    private readonly PindahWebsite3Context _context;

    public FeaturedNewsService(PindahWebsite3Context context)
    {
        _context = context;
    }

    public async Task<(bool Ok, string Message)> SetFeaturedAsync(int newsId, bool featured, CancellationToken cancellationToken = default)
    {
        var article = await _context.News.FirstOrDefaultAsync(n => n.Id == newsId, cancellationToken);
        if (article == null)
        {
            return (false, "Article not found.");
        }

        if (!featured)
        {
            article.IsFeatured = false;
            article.FeaturedRank = null;
            await _context.SaveChangesAsync(cancellationToken);
            await CompactRanksAsync(cancellationToken);
            return (true, "Removed from featured.");
        }

        if (article.Status != NewsStatus.Published)
        {
            return (false, "Only published articles can be featured on the landing page.");
        }

        if (article.IsFeatured)
        {
            return (true, "Already featured.");
        }

        var featuredCount = await _context.News.CountAsync(n => n.IsFeatured, cancellationToken);
        if (featuredCount >= CmsConstants.MaxFeaturedSlots)
        {
            return (false, $"Featured slots are full ({CmsConstants.MaxFeaturedSlots}). Unfeature another article first.");
        }

        var nextRank = featuredCount + 1;
        article.IsFeatured = true;
        article.FeaturedRank = nextRank;
        await _context.SaveChangesAsync(cancellationToken);
        return (true, $"Featured in slot {nextRank} of {CmsConstants.MaxFeaturedSlots}.");
    }

    public async Task CompactRanksAsync(CancellationToken cancellationToken = default)
    {
        var featured = await _context.News
            .Where(n => n.IsFeatured)
            .OrderBy(n => n.FeaturedRank ?? int.MaxValue)
            .ThenByDescending(n => n.DatePublished ?? n.DateCreated)
            .ToListAsync(cancellationToken);

        for (var i = 0; i < featured.Count; i++)
        {
            featured[i].FeaturedRank = i + 1;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<News>> GetLandingFeaturedAsync(CancellationToken cancellationToken = default)
    {
        var featured = await _context.News
            .AsNoTracking()
            .Include(n => n.Author)
            .Where(n => n.IsFeatured && n.Status == NewsStatus.Published)
            .OrderBy(n => n.FeaturedRank ?? int.MaxValue)
            .ThenByDescending(n => n.DatePublished ?? n.DateCreated)
            .Take(CmsConstants.MaxFeaturedSlots)
            .ToListAsync(cancellationToken);

        if (featured.Count > 0)
        {
            return featured;
        }

        // Fallback: latest published when nothing is curated.
        return await _context.News
            .AsNoTracking()
            .Include(n => n.Author)
            .Where(n => n.Status == NewsStatus.Published)
            .OrderByDescending(n => n.DatePublished ?? n.DateCreated)
            .Take(3)
            .ToListAsync(cancellationToken);
    }
}

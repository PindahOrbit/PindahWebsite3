using System.Text.RegularExpressions;

namespace PindahWebsite3.Services;

public static partial class YouTubeUrlHelper
{
    [GeneratedRegex(
        @"(?:youtube\.com\/(?:watch\?v=|embed\/|shorts\/|live\/)|youtu\.be\/)([A-Za-z0-9_-]{11})",
        RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex VideoIdRegex();

    public static bool TryGetVideoId(string? url, out string videoId)
    {
        videoId = string.Empty;
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        var match = VideoIdRegex().Match(url.Trim());
        if (!match.Success)
        {
            return false;
        }

        videoId = match.Groups[1].Value;
        return true;
    }

    public static string? GetEmbedUrl(string? url)
    {
        return TryGetVideoId(url, out var id)
            ? $"https://www.youtube.com/embed/{id}"
            : null;
    }

    public static string? GetThumbnailUrl(string? url)
    {
        return TryGetVideoId(url, out var id)
            ? $"https://i.ytimg.com/vi/{id}/hqdefault.jpg"
            : null;
    }
}

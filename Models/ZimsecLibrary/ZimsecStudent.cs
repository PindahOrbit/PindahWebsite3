using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PindahWebsite3.Models.ZimsecLibrary;

public class ZimsecStudent
{
    public int Id { get; set; }

    [Required, MaxLength(16)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? LastLoginAtUtc { get; set; }
}

public static class ZimsecPhoneNumber
{
    private static readonly Regex ValidPhone = new(@"^\+263[0-9]{8,10}$", RegexOptions.Compiled);

    public static bool TryNormalize(string? input, out string normalized, out string? error)
    {
        normalized = string.Empty;
        error = null;

        if (string.IsNullOrWhiteSpace(input))
        {
            error = "Enter your phone number.";
            return false;
        }

        var compact = input.Trim().Replace(" ", "").Replace("-", "");
        if (compact.StartsWith("00"))
            compact = "+" + compact[2..];
        else if (compact.StartsWith('0'))
            compact = "+263" + compact[1..];
        else if (compact.StartsWith("263") && !compact.StartsWith("+263"))
            compact = "+" + compact;

        if (!compact.StartsWith("+263"))
        {
            error = "Phone number must start with +263 (e.g. +263771234567).";
            return false;
        }

        normalized = compact;
        if (!ValidPhone.IsMatch(normalized))
        {
            error = "Enter a valid Zimbabwe number starting with +263.";
            return false;
        }

        return true;
    }
}

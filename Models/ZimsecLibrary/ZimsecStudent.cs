using System.ComponentModel.DataAnnotations;

namespace PindahWebsite3.Models.ZimsecLibrary;

public class ZimsecStudent
{
    public int Id { get; set; }

    [Required, MaxLength(32)]
    public string StudentNumber { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? LastLoginAtUtc { get; set; }
}

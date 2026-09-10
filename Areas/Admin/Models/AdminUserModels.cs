using System.ComponentModel.DataAnnotations;

namespace PindahWebsite3.Areas.Admin.Models;

public class AdminUserCreateModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Role")]
    public string Role { get; set; } = CmsConstants.RoleContributor;
}

public class AdminUserListItem
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IList<string> Roles { get; set; } = new List<string>();
    public bool EmailConfirmed { get; set; }
    public int ArticleCount { get; set; }
}

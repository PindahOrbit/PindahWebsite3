using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Areas.Admin.Models;
using PindahWebsite3.Areas.Identity.Data;
using PindahWebsite3.Data;

namespace PindahWebsite3.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = CmsConstants.RoleAdmin)]
public class UsersController : Controller
{
    private readonly UserManager<PindahWebsite3User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly PindahWebsite3Context _context;

    public UsersController(
        UserManager<PindahWebsite3User> userManager,
        RoleManager<IdentityRole> roleManager,
        PindahWebsite3Context context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.OrderBy(u => u.Email).ToListAsync();
        var articleCounts = await _context.News
            .Where(n => n.AuthorId != null)
            .GroupBy(n => n.AuthorId!)
            .Select(g => new { AuthorId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.AuthorId, x => x.Count);

        var items = new List<AdminUserListItem>();
        foreach (var user in users)
        {
            items.Add(new AdminUserListItem
            {
                Id = user.Id,
                Email = user.Email ?? user.UserName ?? user.Id,
                Roles = await _userManager.GetRolesAsync(user),
                EmailConfirmed = user.EmailConfirmed,
                ArticleCount = articleCounts.GetValueOrDefault(user.Id)
            });
        }

        return View(items);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new AdminUserCreateModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminUserCreateModel model)
    {
        if (model.Role is not (CmsConstants.RoleAdmin or CmsConstants.RoleContributor))
        {
            ModelState.AddModelError(nameof(model.Role), "Choose Admin or Contributor.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!await _roleManager.RoleExistsAsync(model.Role))
        {
            await _roleManager.CreateAsync(new IdentityRole(model.Role));
        }

        var existing = await _userManager.FindByEmailAsync(model.Email);
        if (existing != null)
        {
            ModelState.AddModelError(nameof(model.Email), "A user with this email already exists.");
            return View(model);
        }

        var user = new PindahWebsite3User
        {
            UserName = model.Email.Trim(),
            Email = model.Email.Trim(),
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        await _userManager.AddToRoleAsync(user, model.Role);
        TempData["Success"] = $"Created user {user.Email} as {model.Role}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetRole(string id, string role)
    {
        if (role is not (CmsConstants.RoleAdmin or CmsConstants.RoleContributor))
        {
            TempData["Error"] = "Invalid role.";
            return RedirectToAction(nameof(Index));
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (await IsLastAdminAsync(user) && role != CmsConstants.RoleAdmin)
        {
            TempData["Error"] = "Cannot demote the last Admin.";
            return RedirectToAction(nameof(Index));
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role);
        TempData["Success"] = $"Updated role for {user.Email} to {role}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var currentUserId = _userManager.GetUserId(User);
        if (user.Id == currentUserId)
        {
            TempData["Error"] = "You cannot delete your own account while signed in.";
            return RedirectToAction(nameof(Index));
        }

        if (await IsLastAdminAsync(user))
        {
            TempData["Error"] = "Cannot delete the last Admin.";
            return RedirectToAction(nameof(Index));
        }

        // Keep articles; clear authorship (SetNull on FK).
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            TempData["Error"] = string.Join(" ", result.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = $"Deleted user {user.Email}.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> IsLastAdminAsync(PindahWebsite3User user)
    {
        if (!await _userManager.IsInRoleAsync(user, CmsConstants.RoleAdmin))
        {
            return false;
        }

        var admins = await _userManager.GetUsersInRoleAsync(CmsConstants.RoleAdmin);
        return admins.Count <= 1;
    }
}

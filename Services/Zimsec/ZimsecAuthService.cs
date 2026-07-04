using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models.ZimsecLibrary;

namespace PindahWebsite3.Services.Zimsec;

public class ZimsecAuthService
{
    private readonly ZimsecContext _context;
    private readonly PasswordHasher<ZimsecStudent> _hasher = new();

    public ZimsecAuthService(ZimsecContext context) => _context = context;

    public async Task<(bool Success, string? Error)> RegisterAsync(string phoneNumber, string password)
    {
        if (!ZimsecPhoneNumber.TryNormalize(phoneNumber, out var normalized, out var phoneError))
            return (false, phoneError);

        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            return (false, "Password must be at least 6 characters.");

        if (await _context.Students.AnyAsync(s => s.PhoneNumber == normalized))
            return (false, "This phone number is already registered.");

        var student = new ZimsecStudent { PhoneNumber = normalized };
        student.PasswordHash = _hasher.HashPassword(student, password);
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, ZimsecStudent? Student, string? Error)> ValidateAsync(string phoneNumber, string password)
    {
        if (!ZimsecPhoneNumber.TryNormalize(phoneNumber, out var normalized, out var phoneError))
            return (false, null, phoneError);

        var student = await _context.Students.FirstOrDefaultAsync(s => s.PhoneNumber == normalized);
        if (student == null)
            return (false, null, "Invalid phone number or password.");

        var result = _hasher.VerifyHashedPassword(student, student.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            return (false, null, "Invalid phone number or password.");

        student.LastLoginAtUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return (true, student, null);
    }

    public async Task SignInAsync(HttpContext httpContext, ZimsecStudent student)
    {
        var claims = new List<Claim>
        {
            new(ZimsecClaimTypes.StudentId, student.Id.ToString()),
            new(ZimsecClaimTypes.PhoneNumber, student.PhoneNumber),
            new(ClaimTypes.Name, student.PhoneNumber)
        };

        var identity = new ClaimsIdentity(claims, ZimsecAuthDefaults.Scheme);
        var principal = new ClaimsPrincipal(identity);
        await httpContext.SignInAsync(ZimsecAuthDefaults.Scheme, principal, new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
        });
    }

    public Task SignOutAsync(HttpContext httpContext) =>
        httpContext.SignOutAsync(ZimsecAuthDefaults.Scheme);

    public static int? GetStudentId(ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ZimsecClaimTypes.StudentId)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }
}

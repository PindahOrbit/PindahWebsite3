using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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

    public async Task<(bool Success, string? Error)> RegisterAsync(string studentNumber, string password)
    {
        studentNumber = NormalizeStudentNumber(studentNumber);
        if (string.IsNullOrWhiteSpace(studentNumber))
            return (false, "Enter your student number.");
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            return (false, "Password must be at least 6 characters.");

        if (await _context.Students.AnyAsync(s => s.StudentNumber == studentNumber))
            return (false, "This student number is already registered.");

        var student = new ZimsecStudent { StudentNumber = studentNumber };
        student.PasswordHash = _hasher.HashPassword(student, password);
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, ZimsecStudent? Student, string? Error)> ValidateAsync(string studentNumber, string password)
    {
        studentNumber = NormalizeStudentNumber(studentNumber);
        var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
        if (student == null)
            return (false, null, "Invalid student number or password.");

        var result = _hasher.VerifyHashedPassword(student, student.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            return (false, null, "Invalid student number or password.");

        student.LastLoginAtUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return (true, student, null);
    }

    public async Task SignInAsync(HttpContext httpContext, ZimsecStudent student)
    {
        var claims = new List<Claim>
        {
            new(ZimsecClaimTypes.StudentId, student.Id.ToString()),
            new(ZimsecClaimTypes.StudentNumber, student.StudentNumber),
            new(ClaimTypes.Name, student.StudentNumber)
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

    public static string NormalizeStudentNumber(string value) =>
        value.Trim().ToUpperInvariant();

    public static int? GetStudentId(ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ZimsecClaimTypes.StudentId)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }
}

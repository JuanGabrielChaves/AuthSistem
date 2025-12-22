using System.Security.Claims;
using CleanArchitecture.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Infrastructure.Authentication;

public sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Implementación obligatoria de UserEmail
    public string? UserEmail =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ??
        _httpContextAccessor.HttpContext?.User?.FindFirstValue("email");

    public Guid UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out var guid) ? guid : Guid.Empty;
        }
    }
}
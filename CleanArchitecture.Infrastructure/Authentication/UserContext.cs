using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Infrastructure.Authentication;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public string? UserEmail
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;

            // Al limpiar el mapeo arriba, el email ahora estará en "sub"
            return user?.FindFirstValue("sub")
                   ?? user?.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? user?.FindFirstValue(ClaimTypes.Email)
                   ?? "System";
        }
    }
}
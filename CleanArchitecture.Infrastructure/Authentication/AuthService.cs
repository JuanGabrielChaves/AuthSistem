using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Authentication;

internal sealed class AuthService : IAuthService
{
    private readonly UserManager<UserIdentity> _userManager;

    public AuthService(UserManager<UserIdentity> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Guid> RegisterAsync(string email, string password, string firstName, string lastName)
    {
        var user = new UserIdentity
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception("Error al crear usuario: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return Guid.Parse(user.Id);
    }
}
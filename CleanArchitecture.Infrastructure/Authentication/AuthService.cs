using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Authentication;

internal sealed class AuthService : IAuthService
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtProvider _jwtProvider;

    public AuthService(
        UserManager<UserIdentity> userManager,
        RoleManager<IdentityRole> roleManager,
        IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtProvider = jwtProvider;
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

        if (!await _roleManager.RoleExistsAsync("User"))
            await _roleManager.CreateAsync(new IdentityRole("User"));

        await _userManager.AddToRoleAsync(user, "User");
        return Guid.Parse(user.Id);
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            throw new Exception("Credenciales inválidas");
        }
        var roles = await _userManager.GetRolesAsync(user);
        Console.WriteLine($"LOGIN: Usuario {email} tiene {roles.Count} roles.");
        return await _jwtProvider.Generate(user);
    }

    public async Task AssignRoleAsync(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new Exception("Usuario no encontrado");

        // Asegurar que el rol existe en la tabla AspNetRoles
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        // Asignar el rol al usuario
        var result = await _userManager.AddToRoleAsync(user, roleName);

        if (!result.Succeeded)
        {
            throw new Exception("Error al asignar rol: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
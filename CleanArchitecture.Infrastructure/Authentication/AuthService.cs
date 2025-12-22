using System.Security.Cryptography;
using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Infrastructure.DbContexts;
using CleanArchitecture.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Authentication;

internal sealed class AuthService : IAuthService
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly ApplicationDbContext _context;

    public AuthService(
        UserManager<UserIdentity> userManager,
        RoleManager<IdentityRole> roleManager,
        IJwtProvider jwtProvider,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtProvider = jwtProvider;
        _context = context;
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

    public async Task<TokenResponse> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            throw new Exception("Credenciales inválidas");
        }

        var accessToken = await _jwtProvider.Generate(user);
        var refreshTokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        _context.Set<RefreshToken>().Add(refreshToken);
        await _context.SaveChangesAsync();

        return new TokenResponse(
            accessToken,
            refreshTokenValue,
            DateTime.UtcNow.AddMinutes(15));
    }
    public async Task AssignRoleAsync(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new Exception("Usuario no encontrado");
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);

        if (!result.Succeeded)
        {
            throw new Exception("Error al asignar rol: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
    public async Task<TokenResponse> RefreshAsync(string refreshTokenValue)
    {
        // 1. Buscar el token en la DB
        var savedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshTokenValue);

        if (savedToken == null || !savedToken.IsActive)
        {
            throw new Exception("Token inválido o expirado");
        }

        // 2. Obtener al usuario
        var user = await _userManager.FindByIdAsync(savedToken.UserId);
        if (user == null) throw new Exception("Usuario no encontrado");

        // 3. Rotación: Revocamos el token actual
        savedToken.IsRevoked = true;
        _context.RefreshTokens.Update(savedToken);

        // 4. Generar nuevo par de tokens
        var newAccessToken = await _jwtProvider.Generate(user);
        var newRefreshTokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        var newRefreshToken = new RefreshToken
        {
            Token = newRefreshTokenValue,
            UserId = user.Id,
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        return new TokenResponse(newAccessToken, newRefreshTokenValue, DateTime.UtcNow.AddMinutes(15));
    }
}
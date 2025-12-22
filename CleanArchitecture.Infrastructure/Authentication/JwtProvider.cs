using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Authentication;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<UserIdentity> _userManager;

    public JwtProvider(IConfiguration configuration, UserManager<UserIdentity> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<string> Generate(UserIdentity user)
    {
        var userFromDb = await _userManager.FindByIdAsync(user.Id);
        var targetUser = userFromDb ?? user;
        var roles = await _userManager.GetRolesAsync(targetUser);

        var claims = new List<Claim> {
        new(JwtRegisteredClaimNames.Sub, targetUser.Email!),
        new(JwtRegisteredClaimNames.Email, targetUser.Email!),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(ClaimTypes.NameIdentifier, targetUser.Id)
    };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var secretKey = _configuration["Jwt:SecretKey"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            null,
            DateTime.UtcNow.AddHours(8),
            creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
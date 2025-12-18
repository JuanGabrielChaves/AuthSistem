using System.IdentityModel.Tokens.Jwt; // Asegúrate de tener este using
using System.Security.Claims;
using System.Text;
using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Authentication;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _configuration;

    public JwtProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Generate(ApplicationUser user)
    {
        // Cambiado de JwtLibRegisteredClaimNames a JwtRegisteredClaimNames
        var claims = new Claim[] {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("firstName", user.FirstName),
            new("lastName", user.LastName)
        };

        var secretKey = _configuration["Jwt:SecretKey"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            null,
            DateTime.UtcNow.AddHours(1),
            creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Authentication;
using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Infrastructure.DbContexts;
using CleanArchitecture.Domain.Entities; // Asegúrate de que apunte a la nueva ubicación de UserIdentity
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Necesario para los esquemas
using Microsoft.IdentityModel.Tokens; // Necesario para TokenValidationParameters
using System.Text;
using CleanArchitecture.Infrastructure.Models;
using System.Security.Claims;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException(nameof(configuration));

        // 1. Base de Datos
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // 2. Identity
        services.AddIdentity<UserIdentity, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // 3. Autenticación JWT (CORREGIDO PARA EVITAR EL 404)
        services.AddAuthentication(options =>
        {
            // Indicamos que el esquema por defecto SIEMPRE sea Bearer
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                NameClaimType = "sub",
                RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!))
            };
        });

        services.AddHttpContextAccessor(); services.AddScoped<IUserContext, UserContext>();

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(configuration));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<UpdateAuditableEntitiesInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

            options.UseSqlServer(connectionString)
                   .AddInterceptors(interceptor);
        });

        return services;
    }
}
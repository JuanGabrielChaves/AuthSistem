using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using AutoMapper; // <--- AGREGAR ESTO
using CleanArchitecture.Application.Behaviors;

namespace CleanArchitecture.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // 1. Registro de AutoMapper (Busca todos los 'Profile' en este proyecto)
        services.AddAutoMapper(assembly);

        // 2. Registro de FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
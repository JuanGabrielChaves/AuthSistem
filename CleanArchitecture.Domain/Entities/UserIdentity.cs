using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Models;

// Aquí sí está permitido usar AspNetCore porque es la capa de Infraestructura
public class UserIdentity : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
namespace CleanArchitecture.Domain.Entities;

// Quitamos la herencia de IdentityUser para que el dominio sea puro
public class ApplicationUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
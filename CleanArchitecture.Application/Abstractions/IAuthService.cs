namespace CleanArchitecture.Application.Abstractions;

public interface IAuthService
{
    Task<Guid> RegisterAsync(string email, string password, string firstName, string lastName);
    Task<string> LoginAsync(string email, string password);
    Task AssignRoleAsync(string email, string roleName);
}
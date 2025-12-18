public interface IAuthService
{
    Task<Guid> RegisterAsync(string email, string password, string firstName, string lastName);
}
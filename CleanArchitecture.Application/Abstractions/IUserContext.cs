public interface IUserContext
{
    string? UserEmail { get; }
    Guid UserId { get; }
}
using CleanArchitecture.Domain.Entities.Common;

public class RefreshToken : BaseEntity
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresOnUtc { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresOnUtc;
    public bool IsRevoked { get; set; }
    public bool IsActive => !IsRevoked && !IsExpired;
    public string UserId { get; set; } = string.Empty;
}

namespace CleanArchitecture.Application.Abstractions;
public record TokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiration);
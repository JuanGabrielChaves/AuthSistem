using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Application.Commands.Users;
using MediatR;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
{
    private readonly IAuthService _authService;

    public RefreshTokenHandler(IAuthService authService) => _authService = authService;

    public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        return await _authService.RefreshAsync(request.RefreshToken);
    }
}
using CleanArchitecture.Application.Abstractions;
using MediatR;

namespace CleanArchitecture.Application.Commands.Users;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        return await _authService.RegisterAsync(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName);
    }
}
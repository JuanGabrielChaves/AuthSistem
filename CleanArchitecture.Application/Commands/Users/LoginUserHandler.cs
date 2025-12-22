using CleanArchitecture.Application.Abstractions;

using MediatR;

namespace CleanArchitecture.Application.Commands.Users
{
    // Cambiamos el retorno de string a TokenResponse
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, TokenResponse>
    {
        private readonly IAuthService _authService;

        public LoginUserHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<TokenResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // El servicio ahora debe encargarse de generar ambos tokens
            return await _authService.LoginAsync(request.Email, request.Password);
        }
    }
}
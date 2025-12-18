using CleanArchitecture.Application.Abstractions;
using MediatR;

namespace CleanArchitecture.Application.Commands.Users
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IAuthService _authService;

        public LoginUserHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // Delegamos toda la lógica al servicio que ya configuramos con roles
            // Este método internamente llama al JwtProvider que SÍ incluye los roles
            return await _authService.LoginAsync(request.Email, request.Password);
        }
    }
}
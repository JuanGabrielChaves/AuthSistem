using MediatR;

namespace CleanArchitecture.Application.Commands.Users
{
    public record LoginUserCommand(string Email, string Password) : IRequest<string>;
}
using CleanArchitecture.Application.Abstractions;
using MediatR;

namespace CleanArchitecture.Application.Commands.Users
{
    public record LoginUserCommand(string Email, string Password) : IRequest<TokenResponse>;
}
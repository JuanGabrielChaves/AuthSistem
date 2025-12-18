using MediatR;

namespace CleanArchitecture.Application.Commands.Users;

public record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<Guid>;
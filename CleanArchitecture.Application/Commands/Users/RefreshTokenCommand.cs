using CleanArchitecture.Application.Abstractions;
using MediatR;

namespace CleanArchitecture.Application.Commands.Users;

public record RefreshTokenCommand(string RefreshToken) : IRequest<TokenResponse>;
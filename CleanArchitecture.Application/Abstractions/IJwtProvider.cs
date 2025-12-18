using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Models;

namespace CleanArchitecture.Application.Abstractions;

public interface IJwtProvider
{
    Task<string> Generate(UserIdentity user);
}
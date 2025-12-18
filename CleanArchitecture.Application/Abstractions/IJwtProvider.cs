using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Abstractions;

public interface IJwtProvider
{
    string Generate(ApplicationUser user);
}
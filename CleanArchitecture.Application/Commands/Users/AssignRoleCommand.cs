

using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities.Common;
using MediatR;

namespace CleanArchitecture.Application.Commands.Users
{
    public record AssignRoleCommand(string Email, string RoleName) : IRequest<Result<bool>>;

    public class AssignRoleCommandHandler(IAuthService authService) : IRequestHandler<AssignRoleCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AssignRoleCommand request, CancellationToken ct)
        {
            try
            {
                await authService.AssignRoleAsync(request.Email, request.RoleName);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new Error("Auth.RoleAssignmentError", ex.Message));
            }
        }
    }
}


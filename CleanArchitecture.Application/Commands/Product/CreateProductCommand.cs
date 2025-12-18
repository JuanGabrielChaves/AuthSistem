using MediatR;
using CleanArchitecture.Domain.Entities.Common;

namespace CleanArchitecture.Application.Commands;


public class CreateProductCommand : IRequest<Result<int>>
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
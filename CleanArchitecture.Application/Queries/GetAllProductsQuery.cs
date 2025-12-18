using CleanArchitecture.Application.Products.Queries;
using CleanArchitecture.Domain.Entities.Common;
using MediatR;

namespace CleanArchitecture.Application.Queries;

public class GetAllProductsQuery : IRequest<Result<List<ProductDto>>>
{
}
using CleanArchitecture.Application.Products.Queries;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities.Common;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Queries;

public record GetProductByIdQuery(int Id) : IRequest<Result<ProductDto>>;
public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IProductRepository _repository;

    public GetProductByIdHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await _repository.GetByIdAsync(request.Id);

        if (product == null)
        {
            return Result<ProductDto>.Failure(new Error(
                "Product.NotFound",
                $"El producto con Id {request.Id} no fue encontrado."));
        }

        var dto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CreatedOnUtc = product.CreatedOnUtc,
            CreatedBy = product.CreatedBy
        };

        return Result<ProductDto>.Success(dto);
    }
}
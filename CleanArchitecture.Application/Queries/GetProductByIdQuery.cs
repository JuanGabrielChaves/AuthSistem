using CleanArchitecture.Application.Products.Queries;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities.Common;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Queries;

// 1. Cambiamos IRequest para que devuelva Result
public record GetProductByIdQuery(int Id) : IRequest<Result<ProductDto>>;

// 2. Cambiamos la interfaz del Handler
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

        // 3. En lugar de devolver null, devolvemos un Failure con un Error claro
        if (product == null)
        {
            return Result<ProductDto>.Failure(new Error(
                "Product.NotFound",
                $"El producto con Id {request.Id} no fue encontrado."));
        }

        // 4. Devolvemos Success envolviendo el DTO
        var dto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CreatedAt = product.CreatedAt,
            CreatedBy = product.CreatedBy
        };

        return Result<ProductDto>.Success(dto);
    }
}
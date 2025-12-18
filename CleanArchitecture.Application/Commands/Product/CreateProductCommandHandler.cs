using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Common;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<int>>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // 1. Crear la entidad
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        // 2. Persistir (EF se encargará de CreatedAt/By automáticamente)
        await _repository.AddAsync(product);

        // 3. Devolver éxito con el ID
        return Result<int>.Success(product.Id);
    }
}
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Products.Queries;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities.Common;
using Dapper;
using MediatR;

namespace CleanArchitecture.Application.Queries;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<List<ProductDto>>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAllProductsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<List<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            using var sqlConnection = _sqlConnectionFactory.CreateConnection();

            const string sql = "SELECT Id, Name, Price, CreatedAt, CreatedBy FROM Products";

            var products = await sqlConnection.QueryAsync<ProductDto>(sql);

            return Result<List<ProductDto>>.Success(products.ToList());
        }
        catch (Exception)
        {
            return Result<List<ProductDto>>.Failure(new Error("Query.Error", "Error al consultar los productos."));
        }
    }
}
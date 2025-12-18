using System.Data;
using CleanArchitecture.Application.Abstractions.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace CleanArchitecture.Infrastructure.Data;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;
    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection")
        );
    }
}
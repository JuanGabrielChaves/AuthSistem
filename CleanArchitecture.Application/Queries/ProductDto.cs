namespace CleanArchitecture.Application.Products.Queries;

public class ProductDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public string? CreatedBy { get; init; }
}
using CleanArchitecture.Domain.Entities.Common;

namespace CleanArchitecture.Domain.Entities;

public class Product : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; } = 0;
}
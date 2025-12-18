using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Application.Products.Queries;


namespace CleanArchitecture.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // De Entidad a DTO
        CreateMap<Product, ProductDto>();

        // Si quisieras un mapeo inverso (de DTO a Entidad)
        // CreateMap<ProductDto, Product>();
    }
}
using AutoMapper;
using BAMK.Application.DTOs.ProductDetail;
using BAMK.Domain.Entities;

namespace BAMK.Application.Mappings
{
    public class ProductDetailMappingProfile : Profile
    {
        public ProductDetailMappingProfile()
        {
            CreateMap<Domain.Entities.ProductDetail, ProductDetailDto>();
            CreateMap<CreateProductDetailDto, Domain.Entities.ProductDetail>();
            CreateMap<UpdateProductDetailDto, Domain.Entities.ProductDetail>();
        }
    }
}

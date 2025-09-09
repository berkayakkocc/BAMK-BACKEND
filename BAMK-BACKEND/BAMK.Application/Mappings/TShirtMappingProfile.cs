using AutoMapper;
using BAMK.Application.DTOs.TShirt;
using BAMK.Domain.Entities;

namespace BAMK.Application.Mappings
{
    public class TShirtMappingProfile : Profile
    {
        public TShirtMappingProfile()
        {
            CreateMap<TShirt, TShirtDto>();
            CreateMap<CreateTShirtDto, TShirt>();
            CreateMap<UpdateTShirtDto, TShirt>();
        }
    }
}

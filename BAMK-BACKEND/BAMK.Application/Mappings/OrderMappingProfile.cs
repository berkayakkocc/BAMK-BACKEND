using AutoMapper;
using BAMK.Application.DTOs.Order;
using BAMK.Domain.Entities;

namespace BAMK.Application.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : string.Empty))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.TShirtName, opt => opt.MapFrom(src => src.TShirt != null ? src.TShirt.Name : string.Empty))
                .ForMember(dest => dest.TShirtColor, opt => opt.MapFrom(src => src.TShirt != null ? src.TShirt.Color : string.Empty))
                .ForMember(dest => dest.TShirtSize, opt => opt.MapFrom(src => src.TShirt != null ? src.TShirt.Size : string.Empty));

            CreateMap<CreateOrderDto, Order>();
            CreateMap<CreateOrderItemDto, OrderItem>();
            CreateMap<UpdateOrderStatusDto, Order>();
            CreateMap<UpdatePaymentStatusDto, Order>();
        }
    }
}

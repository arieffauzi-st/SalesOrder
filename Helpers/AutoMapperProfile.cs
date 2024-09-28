using AutoMapper;
using SalesOrder.DTOs;
using SalesOrder.Entities;

namespace SalesOrder.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<OrderDto, SoOrder>()
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ReverseMap()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.CustomerName))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<ItemDto, SoItem>()
            .ForMember(dest => dest.SoOrderId, opt => opt.Ignore()) // Kita akan mengatur ini secara manual
            .ReverseMap();
        CreateMap<CustomerDto, ComCustomer>().ReverseMap();
    }
}
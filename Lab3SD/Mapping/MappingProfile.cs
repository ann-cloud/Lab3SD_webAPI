using AutoMapper;
using Lab3SD.Models;
using Lab3SD.ViewModels;

namespace Lab3SD.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductViewModel>();
        CreateMap<UserAccount, UserAccountViewModel>();
        CreateMap<OrderItem, OrderItemViewModel>();
        CreateMap<Order, OrderViewModel>();
        CreateMap<Customer, CustomerViewModel>();
    }
}
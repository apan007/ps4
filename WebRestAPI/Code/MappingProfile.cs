using System;
using AutoMapper;
using WebRest.EF.Models;

namespace WebRestAPI.Code
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, Customer>();
            CreateMap<Address, Address>();
            CreateMap<AddressType, AddressType>();
            CreateMap<CustomerAddress, CustomerAddress>();
            CreateMap<Gender, Gender>();
            CreateMap<Order, Order>();
            CreateMap<OrderState, OrderState>();
            CreateMap<OrderStatus, OrderStatus>();
            CreateMap<OrdersLine, OrdersLine>();
            CreateMap<Product, Product>();
            CreateMap<ProductPrice, ProductPrice>();
            CreateMap<ProductStatus, ProductStatus>();
        }
    }
}

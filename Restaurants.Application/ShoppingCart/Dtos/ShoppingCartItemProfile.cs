using AutoMapper;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.ShoppingCart.Dtos;

public class ShoppingCartItemProfile : Profile
{
    public ShoppingCartItemProfile()
    {
        CreateMap<ShoppingCartItem, ShoppingCartItemDto>()
            .ForMember(s => s.Number, ops =>
                ops.MapFrom(src => src.Number))
            .ForMember(s => s.Dish, ops =>
                ops.MapFrom(src => src.Dish));
    }
}
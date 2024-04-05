using AutoMapper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(d => d.City, ops =>
                ops.MapFrom(src => src.Address == null ? null : src.Address.City))
            .ForMember(d => d.Street, ops =>
                ops.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(d => d.Building, ops =>
                ops.MapFrom(src => src.Address == null ? null : src.Address.Building))
            .ForMember(d => d.Apartment, ops =>
                ops.MapFrom(src => src.Address == null ? null : src.Address.Apartment))
            .ForMember(d => d.Dishes, ops => ops.MapFrom(src => src.Dishes));

        CreateMap<CreateRestaurantCommand, Restaurant>()
            .ForMember(d => d.Address, ops =>
                ops.MapFrom(src => new Address
                {
                    City = src.City,
                    Street = src.Street,
                    Building = src.Building,
                    Apartment = src.Apartment
                }))
            .ForMember(d => d.ContactInformation, ops =>
                ops.MapFrom(src => new ContactInformation
                {
                    MainEmail = src.MainEmail,
                    MainPhoneNumber = src.MainPhoneNumber,
                    ExtraEmails = src.ExtraEmails,
                    ExtraPhoneNumbers = src.ExtraPhoneNumbers
                }));
    }
}
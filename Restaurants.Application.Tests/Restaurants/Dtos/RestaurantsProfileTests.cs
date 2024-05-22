using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Tests.Restaurants.Dtos;

[TestSubject(typeof(RestaurantsProfile))]
public class RestaurantsProfileTests
{
    private readonly IMapper _mapper;
    
    public RestaurantsProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        });

        _mapper = configuration.CreateMapper();
    }
    
    [Fact]
    public void Mapping_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // Arrange
        var restaurant = new Restaurant
        {
            Id = 1,
            Name = "Test name",
            Category = "Test category",
            Description =
                "Test description",
            ContactInformation = new()
            {
                MainEmail = "test@test.com",
                MainPhoneNumber = "0123456789"
            },
            HasDelivery = true,
            Address = new()
            {
                City = "Test city",
                Street = "Test street",
                Building = "1"
            }
        };
        
        // Act
        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        // Assert
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.Building.Should().Be(restaurant.Address.Building);
        restaurantDto.Apartment.Should().Be(restaurant.Address.Apartment);
    }
    
    [Fact]
    public void Mapping_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // Arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test name",
            Category = "Test category",
            Description = "Test description",
            MainEmail = "test@test.com",
            MainPhoneNumber = "0123456789",
            HasDelivery = true,
            City = "Test city",
            Street = "Test street",
            Building = "1"
        };
        
        // Act
        var restaurant = _mapper.Map<Restaurant>(command);

        // Assert
        restaurant.Should().NotBeNull();
        restaurant.Address.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.Address!.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.Building.Should().Be(command.Building);
        restaurant.Address.Apartment.Should().Be(command.Apartment);
    }
}
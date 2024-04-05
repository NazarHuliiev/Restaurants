using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

public class RestaurantsService(
    IRestaurantsRepository repository,
    ILogger<RestaurantsService> logger,
    IMapper mapper)
    : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");
        
        var allRestaurants = await repository.GetAllAsync();
        var restaurantDtos = mapper.Map<IEnumerable<RestaurantDto>>(allRestaurants);

        return restaurantDtos;
    }

    public async Task<RestaurantDto?> Get(int id)
    {
        logger.LogInformation($"Getting restaurant id {id}");

        var restaurant = await repository.GetById(id);
        var restaurantDto = mapper.Map<RestaurantDto?>(restaurant);

        return restaurantDto;
    }

    public async Task<int> CreateAsync(CreateRestaurantDto dto)
    {
        logger.LogInformation("Creating a new restaurant");
        
        var newRestaurant = mapper.Map<Restaurant>(dto);

        var newRestaurantId = await repository.Create(newRestaurant);

        return newRestaurantId;
    }
}
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantsService
{
    Task<IEnumerable<RestaurantDto>> GetAllRestaurants();

    Task<RestaurantDto?> Get(int id);
    Task<int> CreateAsync(CreateRestaurantDto dto);
}
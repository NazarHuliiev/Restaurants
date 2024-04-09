using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishesRepository
{
    Task<IEnumerable<Dish>> GetAllAsync(int restaurantId);

    Task<Dish?> GetByIdAsync(int id);
    
    Task<int> CreateAsync(Dish dish);

    Task<bool> DeleteDishAsync(int id);
}
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
{
    public async Task<int> CreateAsync(Dish dish)
    {
        await dbContext.Dishes.AddAsync(dish);

        await dbContext.SaveChangesAsync();

        return dish.Id;
    }

    public async Task<IEnumerable<Dish>> GetAllAsync(int restaurantId)
    {
        var dishes = await dbContext
            .Dishes
            .Where(d => d.RestaurantId == restaurantId)
            .AsNoTracking()
            .ToListAsync();

        return dishes;
    }
    
    public Task<Dish?> GetByIdAsync(int id) =>
        dbContext
            .Dishes
            .FirstOrDefaultAsync(d => d.Id == id);
}
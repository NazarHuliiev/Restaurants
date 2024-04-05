using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext
            .Restaurants
            .AsNoTracking()
            .ToListAsync();

        return restaurants;
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dbContext
            .Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id);

        return restaurant;
    }

    public async Task<int> CreateAsync(Restaurant restaurant)
    {
         await dbContext.Restaurants.AddAsync(restaurant);

         await dbContext.SaveChangesAsync();

         return restaurant.Id;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deletedRows = await dbContext.Restaurants.Where(r => r.Id == id).ExecuteDeleteAsync();

        return deletedRows > 0;
    }

    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}
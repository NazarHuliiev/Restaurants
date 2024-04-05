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

    public async Task<Restaurant?> GetById(int id)
    {
        var restaurant = await dbContext
            .Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id);

        return restaurant;
    }

    public async Task<int> Create(Restaurant restaurant)
    {
         await dbContext.Restaurants.AddAsync(restaurant);

         await dbContext.SaveChangesAsync();

         return restaurant.Id;
    }
}
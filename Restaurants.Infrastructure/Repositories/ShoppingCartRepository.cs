using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class ShoppingCartRepository(RestaurantsDbContext dbContext) : IShoppingCartRepository
{
    public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItemsForUserAsync(string userId)
    {
        var shoppingCartItems = await dbContext.ShoppingCartItems
            .Where(s => s.UserId == userId)
            .Include(s => s.Dish)
            .AsNoTracking()
            .ToListAsync();

        return shoppingCartItems;
    }
}
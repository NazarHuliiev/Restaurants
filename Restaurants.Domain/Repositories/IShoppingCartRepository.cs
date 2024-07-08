using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IShoppingCartRepository
{
    Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItemsForUserAsync(string userId);
}
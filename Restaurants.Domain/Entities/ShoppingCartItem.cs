using Restaurants.Domain.Entities.Identity;

namespace Restaurants.Domain.Entities;

public class ShoppingCartItem
{
    public int ShoppingCartItemId { get; set; }
    
    public int? DishId { get; set; }

    public Dish? Dish { get; set; }
    
    public int Number { get; set; }
    
    public User User { get; set; } = default!;
    
    public string UserId { get; set; } = default!;
}
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.ShoppingCart.Dtos;

public class ShoppingCartItemDto
{
    public DishDto Dish { get; set; } = default!;

    public int Number { get; set; }
}
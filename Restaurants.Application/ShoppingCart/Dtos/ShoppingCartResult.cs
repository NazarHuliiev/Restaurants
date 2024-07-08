namespace Restaurants.Application.ShoppingCart.Dtos;

public class ShoppingCartResult
{
    public IEnumerable<ShoppingCartItemDto> Items { get; set; } = [];
}
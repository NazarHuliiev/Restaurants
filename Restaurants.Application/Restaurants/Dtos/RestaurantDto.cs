using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public string Description { get; set; } = default!;
    
    public string Category { get; set; } = default!;
    
    public bool HasDelivery { get; set; }
    
    public string City { get; set; } = default!;

    public string Street { get; set; } = default!;
    
    public string? Building { get; set; }
    
    public string? Apartment { get; set; }
    
    public string? LogoSasUrl { get; set; }

    public IEnumerable<DishDto> Dishes { get; set; } = [];
}
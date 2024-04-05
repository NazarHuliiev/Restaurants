namespace Restaurants.Domain.Entities;

public class Address
{
    public string City { get; set; } = default!;

    public string Street { get; set; } = default!;
    
    public string? Building { get; set; }
    
    public string? Apartment { get; set; }
}
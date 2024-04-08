using MediatR;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommand : IRequest
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
    public string? MainEmail { get; set; }
    public string? MainPhoneNumber { get; set; }
    public List<string>? ExtraEmails { get; set; }
    public List<string>? ExtraPhoneNumbers { get; set; }
}
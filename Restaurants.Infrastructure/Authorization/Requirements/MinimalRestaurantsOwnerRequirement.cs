using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimalRestaurantsOwnerRequirement(int numberOfRestaurants) : IAuthorizationRequirement
{
    public int MinNumberOfRestaurants { get; } = numberOfRestaurants;
}
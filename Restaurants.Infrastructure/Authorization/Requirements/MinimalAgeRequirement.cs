using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimalAgeRequirement(int minimalAge) : IAuthorizationRequirement
{
    public int MinimalAge { get; } = minimalAge;
}
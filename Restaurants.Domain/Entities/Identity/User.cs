﻿using Microsoft.AspNetCore.Identity;

namespace Restaurants.Domain.Entities.Identity;

public class User : IdentityUser
{
    public DateOnly? DateOfBirth { get; set; }

    public string? CurrentCountry { get; set; }

    public List<Restaurant> OwnedRestaurants { get; set; } = [];
}
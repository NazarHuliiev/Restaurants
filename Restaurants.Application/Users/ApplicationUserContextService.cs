﻿using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Restaurants.Application.Users;

public class ApplicationUserContextService(
    IHttpContextAccessor httpContextAccessor)
    : IApplicationUserContextService
{
    public ApplicationUser? GetUser()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user == null)
        {
            throw new InvalidOperationException("User context is null");
        }

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return null;
        }

        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(r => r.Value);
        var currentCountry = user.FindFirst(c => c.Type == "CurrentCountry")?.Value;
        var dateOfBirthClaimValue = user.FindFirst(c => c.Type == "DateOfBirth")?.Value;
        var dateOfBirth = string.IsNullOrEmpty(dateOfBirthClaimValue) ? (DateOnly?)null : DateOnly.ParseExact(dateOfBirthClaimValue, "yyyy-MM-dd");
        
        return new ApplicationUser(userId, email, roles, CurrentCountry: currentCountry, DateOfBirth: dateOfBirth);
    }
}
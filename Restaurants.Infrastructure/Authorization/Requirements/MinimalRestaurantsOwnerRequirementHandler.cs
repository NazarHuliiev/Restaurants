using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

internal class MinimalRestaurantsOwnerRequirementHandler(
    ILogger<MinimalRestaurantsOwnerRequirementHandler> logger,
    IApplicationUserContextService applicationUserContextService,
    IRestaurantsRepository restaurantsRepository)
    : AuthorizationHandler<MinimalRestaurantsOwnerRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimalRestaurantsOwnerRequirement requirement)
    {
        var user = applicationUserContextService.GetUser();

        if (user == null)
        {
            logger.LogWarning(
                "User is unauthorized - MinimalRestaurantsOwnerRequirement failed");
            
            context.Fail();
            
            return;
        }
        
        logger.LogInformation(
            "User: {Email}, with Id {Id} - handling MinimalRestaurantsOwnerRequirement",
            user.Email,
            user.Id);

        var restaurants = await restaurantsRepository.GetRestaurantsForOwner(user.Id);

        if (restaurants.Count() < requirement.MinNumberOfRestaurants)
        {
            logger.LogWarning(
                "User {email} owns less then 2 restaurants - MinimalRestaurantsOwnerRequirement failed",
                user.Email);
            
            context.Fail();
            
            return;
        }
        
        logger.LogWarning(
            "User {email} owns at least 2 restaurants - MinimalRestaurantsOwnerRequirement success",
            user.Email);
        
        context.Succeed(requirement);
    }
}
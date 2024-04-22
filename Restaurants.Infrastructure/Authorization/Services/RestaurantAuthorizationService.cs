using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(
    ILogger<RestaurantAuthorizationService> logger,
    IApplicationUserContextService applicationUserContextService) 
    : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = applicationUserContextService.GetUser()!;
        
        logger.LogInformation(
            "Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantName}",
            user.Email,
            resourceOperation,
            restaurant.Name);

        if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
        {
            logger.LogInformation("Create/Read operation - successful authorization");

            return true;
        }

        if (resourceOperation == ResourceOperation.Delete && user.HasRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin user, delete operation - successful authorization");

            return true;
        }

        if (resourceOperation is ResourceOperation.Delete or ResourceOperation.Update &&
            user.Id == restaurant.OwnerId)
        {
            logger.LogInformation("Restaurant owner - successful authorization");

            return true;
        }
        
        logger.LogInformation("User doesn't have access - unauthorized");

        return false;
    }
}
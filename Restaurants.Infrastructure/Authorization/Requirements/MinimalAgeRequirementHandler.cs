using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimalAgeRequirementHandler(
    ILogger<MinimalAgeRequirementHandler> logger,
    IApplicationUserContextService applicationUserContextService)
    : AuthorizationHandler<MinimalAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimalAgeRequirement requirement)
    {
        var user = applicationUserContextService.GetUser()!;
        
        logger.LogInformation(
            "User: {email}, date of birth {dateOfBirth} - handling MinimalAgeRequirement",
            user.Email,
            user.DateOfBirth);

        if (user.DateOfBirth is null)
        {
            logger.LogWarning("User's {email} date of birth is null", user.Email);
            context.Fail();
            
            return Task.CompletedTask;
        }

        if (user.DateOfBirth.Value.AddYears(requirement.MinimalAge) < DateOnly.FromDateTime(DateTime.Today))
        {
            logger.LogInformation("User's {email} date of birth meets the MinimalAgeRequirement", user.Email);
            
            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        logger.LogWarning("User's {email} date of birth doesn't meet the MinimalAgeRequirement", user.Email);
        context.Fail();

        return Task.CompletedTask;
    }
}
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements;

namespace Restaurants.Infrastructure.Tests.Authorization.Requirements;

[TestSubject(typeof(MinimalRestaurantsOwnerRequirementHandler))]
public class MinimalRestaurantsOwnerRequirementHandlerTests
{

    [Fact]
    public void HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // Arrange
        const string currentUserId = "1";
        
        var currentUser = new ApplicationUser(
            currentUserId,
            "test@test.com",
            [],
            null,
            null);
        
        var userRestaurants = new List<Restaurant>
        {
            new() { Id = 1, OwnerId = currentUserId },
            new() { Id = 2, OwnerId = currentUserId }
        };

        var loggerMock = new Mock<ILogger<MinimalRestaurantsOwnerRequirementHandler>>();
        var applicationUserContextServiceMock = new Mock<IApplicationUserContextService>();
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();

        applicationUserContextServiceMock
            .Setup(a => a.GetUser())
            .Returns(currentUser);

        restaurantsRepositoryMock
            .Setup(r => r.GetRestaurantsForOwner(It.IsAny<string>()))
            .ReturnsAsync(userRestaurants);
        
        var handler = new MinimalRestaurantsOwnerRequirementHandler(
            loggerMock.Object,
            applicationUserContextServiceMock.Object,
            restaurantsRepositoryMock.Object);
        var requirement = new MinimalRestaurantsOwnerRequirement(2);
        var context = new AuthorizationHandlerContext([requirement], null, null);
        
        // Act
        handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }
    
    [Fact]
    public void HandleRequirementAsync_UserDoesntHaveCreatedMultipleRestaurants_ShouldFail()
    {
        // Arrange
        const string currentUserId = "1";
        
        var currentUser = new ApplicationUser(
            currentUserId,
            "test@test.com",
            [],
            null,
            null);
        
        var userRestaurants = new List<Restaurant>
        {
            new() { Id = 1, OwnerId = currentUserId }
        };

        var loggerMock = new Mock<ILogger<MinimalRestaurantsOwnerRequirementHandler>>();
        var applicationUserContextServiceMock = new Mock<IApplicationUserContextService>();
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();

        applicationUserContextServiceMock
            .Setup(a => a.GetUser())
            .Returns(currentUser);

        restaurantsRepositoryMock
            .Setup(r => r.GetRestaurantsForOwner(It.IsAny<string>()))
            .ReturnsAsync(userRestaurants);
        
        var handler = new MinimalRestaurantsOwnerRequirementHandler(
            loggerMock.Object,
            applicationUserContextServiceMock.Object,
            restaurantsRepositoryMock.Object);
        var requirement = new MinimalRestaurantsOwnerRequirement(2);
        var context = new AuthorizationHandlerContext([requirement], null, null);
        
        // Act
        handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
    }
}
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

[TestSubject(typeof(CreateRestaurantCommandHandler))]
public class CreateRestaurantCommandHandlerTests
{

    [Fact]
    public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
    {
        // Arrange
        const int createdRestaurantId = 1;
        const string restaurantOwnerId = "1";
        var currentUser = new ApplicationUser(restaurantOwnerId, "email@test.com", [], null, null);
        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();
        
        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        var userContextMock = new Mock<IApplicationUserContextService>();

        restaurantRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Restaurant>()))
            .ReturnsAsync(createdRestaurantId);
        userContextMock
            .Setup(u => u.GetUser())
            .Returns(currentUser);
        mapperMock
            .Setup(m => m.Map<Restaurant>(It.IsAny<CreateRestaurantCommand>()))
            .Returns(restaurant);
        
        var commandHandler = new CreateRestaurantCommandHandler(
            loggerMock.Object,
            mapperMock.Object,
            restaurantRepositoryMock.Object,
            userContextMock.Object);

        // Act
        var restaurantId = await commandHandler.Handle(command, CancellationToken.None);

        // Assert
        restaurantId.Should().Be(createdRestaurantId);
        restaurant.OwnerId.Should().Be(restaurantOwnerId);
        restaurantRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Restaurant>()), Times.Once);
    }
}
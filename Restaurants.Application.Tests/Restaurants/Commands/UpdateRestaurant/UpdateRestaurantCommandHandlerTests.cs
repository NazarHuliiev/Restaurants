using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

[TestSubject(typeof(UpdateRestaurantCommandHandler))]
public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _commandHandler;
    
    public UpdateRestaurantCommandHandlerTests()
    {
        var loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        
        _commandHandler = new UpdateRestaurantCommandHandler(
            loggerMock.Object,
            _restaurantRepositoryMock.Object,
            _restaurantAuthorizationServiceMock.Object);
    }
    
    [Fact]
    public async Task Handle_ForValidCommand_ShouldUpdateRestaurant()
    {
        // Arrange
        const int restaurantId = 1;
        
        var restaurant = new Restaurant { Id = restaurantId };
        var command = new UpdateRestaurantCommand
        {
            Name = "New name",
            Category = "New category",
            Description = "New description",
            MainEmail = "newtest@test.com",
            MainPhoneNumber = "new0123456789",
            HasDelivery = true,
            City = "New city",
            Street = "New street",
            Building = "new"
        };

        _restaurantRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(restaurant);
        _restaurantAuthorizationServiceMock
            .Setup(r => r.Authorize(restaurant, ResourceOperation.Update))
            .Returns(true);

        // Act
        var func = ()=> _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        await func.Should().NotThrowAsync();
        _restaurantRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        restaurant.Address.Should().NotBeNull();
        restaurant.ContactInformation.Should().NotBeNull();
        restaurant.Id.Should().Be(restaurantId);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.ContactInformation!.MainEmail.Should().Be(command.MainEmail);
        restaurant.ContactInformation.MainPhoneNumber.Should().Be(command.MainPhoneNumber);
        restaurant.ContactInformation.ExtraEmails.Should().BeSameAs(command.ExtraEmails);
        restaurant.ContactInformation.ExtraPhoneNumbers.Should().BeSameAs(command.ExtraPhoneNumbers);
        restaurant.Address!.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.Building.Should().Be(command.Building);
        restaurant.Address.Apartment.Should().Be(command.Apartment);
    }

    [Fact]
    public async Task Handle_ForRestaurantIsNull_ThrowsNotFoundException()
    {
        // Arrange
        var command = new UpdateRestaurantCommand();

        _restaurantRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Restaurant?)null);

        // Act
        var func = ()=> _commandHandler.Handle(command, CancellationToken.None);
        
        // Assert

        await func.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ForRestaurantExistsAndNotAllowedToUpdate_ThrowsForbiddenException()
    {
        // Arrange
        const int restaurantId = 1;
        var restaurant = new Restaurant { Id = restaurantId };
        var command = new UpdateRestaurantCommand();

        _restaurantRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(restaurant);
        _restaurantAuthorizationServiceMock
            .Setup(r => r.Authorize(restaurant, ResourceOperation.Update))
            .Returns(false);

        // Act
        var func = ()=> _commandHandler.Handle(command, CancellationToken.None);
        
        // Assert

        await func.Should().ThrowAsync<ForbiddenException>();
    }
}
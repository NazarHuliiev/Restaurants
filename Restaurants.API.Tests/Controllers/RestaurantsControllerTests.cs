using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Controllers;
using Restaurants.API.Tests.Fakes;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.API.Tests.Controllers;

[TestSubject(typeof(RestaurantsController))]
public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
    private readonly Mock<IRestaurantsSeeder> _restaurantsSeederMock = new();

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(
                    ServiceDescriptor.Scoped(
                        typeof(IRestaurantsRepository),
                        _ => _restaurantsRepositoryMock.Object));

                services.Replace(
                    ServiceDescriptor.Scoped(
                        typeof(IRestaurantsSeeder),
                        _ => _restaurantsSeederMock.Object));
            });
        });
    }

    [Fact]
    public async Task GetAll_WhenRequestIsValid_Returns200Ok()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync("/api/restaurants?PageSize=10&PageNumber=1");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetAll_WhenRequestIsInvalid_Returns400BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync("/api/restaurants");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_ForNonExistingId_Returns404NotFound()
    {
        // Arrange
        var id = 1;

        _restaurantsRepositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Restaurant?)null);

        var client = _factory.CreateClient();

        // Act 
        var result = await client.GetAsync($"/api/restaurants/{id}");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetById_ForExistingId_Returns200Ok()
    {
        // Arrange
        var id = 1;
        var restaurant = new Restaurant
        {
            Id = id,
            Name = "Test",
            Description = "Test description"
        };

        _restaurantsRepositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(restaurant);

        var client = _factory.CreateClient();

        // Act 
        var result = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto = await result.Content.ReadFromJsonAsync<RestaurantDto>();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto!.Name.Should().Be("Test");
        restaurantDto.Description.Should().Be("Test description");
    }
}
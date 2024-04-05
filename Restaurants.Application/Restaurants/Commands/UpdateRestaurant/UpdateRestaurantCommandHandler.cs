using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    ILogger<CreateRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updating restaurant id {request.Id}");

        var restaurantEntity = await restaurantsRepository.GetByIdAsync(request.Id);

        if (restaurantEntity is null)
        {
            return false;
        }
        
        // TODO: add mapper profile
        restaurantEntity.Name = request.Name;
        restaurantEntity.Description = request.Description;
        restaurantEntity.Category = request.Category;
        restaurantEntity.HasDelivery = request.HasDelivery;
        restaurantEntity.ContactInformation.MainEmail = request.MainEmail;
        restaurantEntity.ContactInformation.ExtraEmails = request.ExtraEmails;
        restaurantEntity.ContactInformation.MainPhoneNumber = request.MainPhoneNumber;
        restaurantEntity.ContactInformation.ExtraPhoneNumbers = request.ExtraPhoneNumbers;
        restaurantEntity.Address.City = request.City;
        restaurantEntity.Address.Street = request.Street;
        restaurantEntity.Address.Building = request.Building;
        restaurantEntity.Address.Apartment = request.Apartment;

        await restaurantsRepository.SaveChangesAsync();
        
        return true;
    }
}
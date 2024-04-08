using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    ILogger<UpdateRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updating restaurant id {request.Id}");

        var restaurantEntity = await restaurantsRepository.GetByIdAsync(request.Id);

        if (restaurantEntity is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
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
    }
}
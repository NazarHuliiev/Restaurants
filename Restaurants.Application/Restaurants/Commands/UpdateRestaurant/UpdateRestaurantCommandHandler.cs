using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    ILogger<UpdateRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService)
    : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updating restaurant id {request.Id}");

        var restaurantEntity = await restaurantsRepository.GetByIdAsync(request.Id);

        if (restaurantEntity is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        }
        
        if (!restaurantAuthorizationService.Authorize(restaurantEntity, ResourceOperation.Update))
        {
            throw new ForbiddenException();
        }
        
        // TODO: add mapper profile
        restaurantEntity.Name = request.Name;
        restaurantEntity.Description = request.Description;
        restaurantEntity.Category = request.Category;
        restaurantEntity.HasDelivery = request.HasDelivery;
        restaurantEntity.ContactInformation = new ContactInformation
        {
            MainEmail = request.MainEmail,
            ExtraEmails = request.ExtraEmails,
            MainPhoneNumber = request.MainPhoneNumber,
            ExtraPhoneNumbers = request.ExtraPhoneNumbers
        };
        restaurantEntity.Address = new Address
        {
            City = request.City,
            Street = request.Street,
            Building = request.Building,
            Apartment = request.Apartment
        };

        await restaurantsRepository.SaveChangesAsync();
    }
}
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(
    ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IApplicationUserContextService applicationUserContextService)
    : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var user = applicationUserContextService.GetUser()!;
        
        logger.LogInformation(
            "{UserEmail} [{UserId}] is creating a new restaurant {@restaurant}",
            user.Email,
            user.Id,
            request);
        
        var newRestaurant = mapper.Map<Restaurant>(request);
        newRestaurant.OwnerId = user.Id;
        var newRestaurantId = await restaurantsRepository.CreateAsync(newRestaurant);

        return newRestaurantId;
    }
}
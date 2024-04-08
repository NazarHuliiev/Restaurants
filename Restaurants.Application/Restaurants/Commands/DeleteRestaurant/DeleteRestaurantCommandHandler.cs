using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(
    ILogger<DeleteRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository)
    : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Deleting restaurant. Id {request.Id}");

        var isDeleted = await restaurantsRepository.DeleteAsync(request.Id);

        if (!isDeleted)
        {
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        }
    }
}
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishById;

public class GetDishByIdQueryHandler(
    ILogger<GetDishByIdQueryHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository)
    : IRequestHandler<GetDishByIdQuery, DishDto?>
{
    public async Task<DishDto?> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogError("Retrieving dish id {@dish} for restaurant id {@restaurant}", request.DishId, request.RestaurantId);
        
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId);
        
        if (dish is null)
        {
            throw new NotFoundException(nameof(Dish), request.DishId.ToString());
        }

        var dishDto = mapper.Map<DishDto>(dish);

        return dishDto;
    }
}
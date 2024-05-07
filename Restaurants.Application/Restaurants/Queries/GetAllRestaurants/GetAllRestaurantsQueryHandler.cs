using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(
    ILogger<GetAllRestaurantsQueryHandler> logger,
    IRestaurantsRepository repository,
    IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
{
    public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");
        
        var matchingResult = await repository.GetAllMatchingAsync(
            request.SearchPhrase,
            request.PageSize,
            request.PageNumber,
            request.SortBy,
            request.SortingDirection);
        var restaurantDtos = mapper.Map<IEnumerable<RestaurantDto>>(matchingResult.restaurants);
        var result = new PagedResult<RestaurantDto>(restaurantDtos, matchingResult.totalCount, request.PageSize, request.PageNumber);
        
        return result;
    }
}
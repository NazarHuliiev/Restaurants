using MediatR;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery : IRequest<PagedResult<RestaurantDto>>
{
    public string SearchPhrase { get; set; } = string.Empty;

    public int PageSize { get; set; } = 5;

    public int PageNumber { get; set; } = 1;

    public string? SortBy { get; set; }

    public SortingDirection SortingDirection { get; set; }
}
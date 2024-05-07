using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private readonly int[] _allowedPageSizes = [5, 10, 15, 30];

    private readonly string[] _allowedSortByPropertyNames =
    {
        nameof(RestaurantDto.Name),
        nameof(RestaurantDto.Description),
        nameof(RestaurantDto.Category)
    };
    
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(query => query.PageSize)
            .Must(pageSize => _allowedPageSizes.Contains(pageSize))
            .WithMessage($"Page size  must be in {string.Join(", ", _allowedPageSizes)} range");

        RuleFor(query => query.SortBy)
            .Must(sortingBy => _allowedSortByPropertyNames.Contains(sortingBy))
            .When(sortingBy => !string.IsNullOrEmpty(sortingBy.SortBy))
            .WithMessage($"SortBy  must be one of these {string.Join(", ", _allowedSortByPropertyNames)} values");
    }
}
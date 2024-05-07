using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext
            .Restaurants
            .AsNoTracking()
            .ToListAsync();

        return restaurants;
    }

    public async Task<(IEnumerable<Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(
        string? searchPhrase,
        int pageSize,
        int pageNumber,
        string? sortBy,
        SortingDirection sortingDirection)
    {
        var searchPhraseNormalized = searchPhrase?.ToUpper();

        var baseQuery = dbContext
            .Restaurants
            .Where(r => r.Name.ToUpper().Contains(searchPhraseNormalized) ||
                        r.Description.ToUpper().Contains(searchPhraseNormalized));

        if (!string.IsNullOrEmpty(sortBy))
        {
            Expression<Func<Restaurant, object>> getSortingKey() => sortBy switch
                {
                    nameof(RestaurantDto.Description) => r => r.Description,
                    nameof(RestaurantDto.Category) => r => r.Category,
                    _ => r => r.Name
                };
            
            baseQuery = sortingDirection == SortingDirection.Ascending
                ? baseQuery.OrderBy(getSortingKey())
                : baseQuery.OrderByDescending(getSortingKey());
        }

        var totalCount = await baseQuery.CountAsync();
        
        var matchingRestaurants = await baseQuery
            .Skip(pageSize * (pageNumber -1))
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return (matchingRestaurants, totalCount);
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dbContext
            .Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id);

        return restaurant;
    }

    public async Task<IEnumerable<Restaurant>> GetRestaurantsForOwner(string ownerId)
    {
        var restaurants = await dbContext
            .Restaurants
            .Where(r => r.OwnerId == ownerId)
            .AsNoTracking()
            .ToListAsync();

        return restaurants;
    }

    public async Task<int> CreateAsync(Restaurant restaurant)
    {
         await dbContext.Restaurants.AddAsync(restaurant);

         await dbContext.SaveChangesAsync();

         return restaurant.Id;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deletedRows = await dbContext.Restaurants.Where(r => r.Id == id).ExecuteDeleteAsync();

        return deletedRows > 0;
    }

    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}
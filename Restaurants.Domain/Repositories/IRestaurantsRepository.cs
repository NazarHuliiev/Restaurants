using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    
    Task<(IEnumerable<Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(
        string? searchPhrase,
        int pageSize,
        int pageNumber,
        string? sortBy,
        SortingDirection sortingDirection);

    Task<Restaurant?> GetByIdAsync(int id);

    Task<IEnumerable<Restaurant>> GetRestaurantsForOwner(string ownerId);
    
    Task<int> CreateAsync(Restaurant restaurant);
    
    Task<bool> DeleteAsync(int id);
    
    Task SaveChangesAsync();
}
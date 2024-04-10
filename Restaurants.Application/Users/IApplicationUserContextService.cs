namespace Restaurants.Application.Users;

public interface IApplicationUserContextService
{
    ApplicationUser? GetUser();
}
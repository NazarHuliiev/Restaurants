namespace Restaurants.Application.User;

public interface IApplicationUserContextService
{
    ApplicationUser? GetUser();
}
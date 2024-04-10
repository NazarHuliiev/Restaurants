namespace Restaurants.Application.User;

public record ApplicationUser(string Id, string Email, IEnumerable<string> Roles)
{
    public bool HasRole(string role) => Roles.Contains(role);
}
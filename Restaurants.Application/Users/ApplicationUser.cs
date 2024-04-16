namespace Restaurants.Application.Users;

public record ApplicationUser(
    string Id, string Email,
    IEnumerable<string> Roles,
    string? CurrentCountry,
    DateOnly? DateOfBirth)
{
    public bool HasRole(string role) => Roles.Contains(role);
}
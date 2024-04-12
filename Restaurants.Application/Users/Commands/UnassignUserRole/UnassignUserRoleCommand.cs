using MediatR;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;

public class UnassignUserRoleCommand(string userEmail, string role) : IRequest
{
    public string UserEmail { get; set; } = userEmail;

    public string Role { get; set; } = role;
}
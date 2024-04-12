using MediatR;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommand(string userEmail, string role) : IRequest
{
    public string UserEmail { get; } = userEmail;

    public string Role { get; } = role;
}
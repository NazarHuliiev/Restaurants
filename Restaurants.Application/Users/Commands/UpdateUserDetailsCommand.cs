using MediatR;

namespace Restaurants.Application.Users.Commands;

public class UpdateUserDetailsCommand : IRequest
{
    public DateOnly? DateOfBirth { get; set; }

    public string? CurrentCountry { get; set; }
}
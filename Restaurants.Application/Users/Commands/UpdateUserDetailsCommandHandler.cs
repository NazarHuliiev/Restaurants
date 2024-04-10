using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands;

public class UpdateUserDetailsCommandHandler(
    ILogger<UpdateUserDetailsCommandHandler> logger,
    IApplicationUserContextService applicationUserContextService,
    IUserStore<User> userStore)
    : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = applicationUserContextService.GetUser()!;
        
        logger.LogInformation("Updating user {userId} with {@newUserData}", request, user.Id);

        var dbUser = await userStore.FindByIdAsync(user.Id, cancellationToken);

        if (dbUser == null)
        {
            throw new NotFoundException(nameof(User), user.Id.ToString());
        }

        dbUser.DateOfBirth = request.DateOfBirth;
        dbUser.CurrentCountry = request.CurrentCountry;

        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}
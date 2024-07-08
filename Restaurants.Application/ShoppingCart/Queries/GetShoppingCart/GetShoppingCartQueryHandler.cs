using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.ShoppingCart.Dtos;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.ShoppingCart.Queries.GetShoppingCart;

public class GetShoppingCartQueryHandler(
    ILogger<GetShoppingCartQueryHandler> logger,
    IMapper mapper,
    IShoppingCartRepository shoppingCartRepository,
    IApplicationUserContextService applicationUserContextService)
    : IRequestHandler<GetShoppingCartQuery, ShoppingCartResult>
{
    public async Task<ShoppingCartResult> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
    {
        var user = applicationUserContextService.GetUser()!;
        
        logger.LogInformation(
            "Retrieving shopping cart for user {UserId}",
            user.Id);

        var shoppingCartItems = await shoppingCartRepository.GetShoppingCartItemsForUserAsync(user.Id);
        var shoppingCartItemsDtos = mapper.Map<IEnumerable<ShoppingCartItemDto>>(shoppingCartItems);
        
        return new ShoppingCartResult {Items = shoppingCartItemsDtos };
    }
}
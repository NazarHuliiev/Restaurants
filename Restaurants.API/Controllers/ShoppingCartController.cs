using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.ShoppingCart.Dtos;
using Restaurants.Application.ShoppingCart.Queries.GetShoppingCart;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShoppingCartController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ShoppingCartResult>> GetAsync()
    {
        var shippingCart = await mediator.Send(new GetShoppingCartQuery());

        return Ok(shippingCart);
    }
}
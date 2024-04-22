using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllRestaurantsQuery());

        return Ok(result);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestaurantDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    [AllowAnonymous]
    //[Authorize(Policy = PolicyNames.AlLeast2RestaurantsOwner)]
    //[Authorize(Policy = PolicyNames.HasCurrentCountry)]
    //[Authorize(Policy = PolicyNames.AlLeast20)]
    public async Task<ActionResult<RestaurantDto>> Get(int id)
    {
        var result = await mediator.Send(new GetRestaurantByIdQuery(id));

        return Ok(result);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(CreateRestaurantCommand command)
    {
        var id = await mediator.Send(command);

        return CreatedAtAction(nameof(Get), new { id }, null);
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteRestaurantCommand(id));

        return NoContent();
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateRestaurantCommand command)
    {
        command.Id = id;
        
        await mediator.Send(command);

        return NoContent();
    }
}
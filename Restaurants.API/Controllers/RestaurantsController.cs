using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllRestaurantsQuery());

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestaurantDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RestaurantDto>> Get(int id)
    {
        var result = await mediator.Send(new GetRestaurantByIdQuery(id));

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    
    [HttpPost]
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
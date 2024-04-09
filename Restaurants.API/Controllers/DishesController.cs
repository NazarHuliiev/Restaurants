using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteDish;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetAllDishes;
using Restaurants.Application.Dishes.Queries.GetDishById;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/[controller]")]
[ApiController]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAll(int restaurantId)
    {
        var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));

        return Ok(dishes);
    }
    
    [HttpGet("{dishId}")]
    public async Task<ActionResult<DishDto?>> Get(int restaurantId, int dishId)
    {
        var dish = await mediator.Send(new GetDishByIdQuery(restaurantId, dishId));
        
        return Ok(dish);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateDish(int restaurantId, CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        
        var newDishId = await mediator.Send(command);

        return CreatedAtAction(nameof(Get), new { restaurantId = restaurantId, dishId = newDishId }, null);
    }
    
    [HttpDelete("{dishId}")]
    public async Task<IActionResult> DeleteDish(int restaurantId, int dishId)
    {
        await mediator.Send(new DeleteDishCommand(restaurantId, dishId));

        return NoContent();
    }
}
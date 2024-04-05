using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await restaurantsService.GetAllRestaurants();

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await restaurantsService.Get(id);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateRestaurantDto dto)
    {
        int id = await restaurantsService.CreateAsync(dto);

        return CreatedAtAction(nameof(Get), new { id }, null);
    }
}
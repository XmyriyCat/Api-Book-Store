using BLL.DTO.Order;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderCatalogService _orderService;

    public OrderController(IOrderCatalogService orderService)
    {
        _orderService = orderService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsyncTask()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsyncTask(int id)
    {
        var order = await _orderService.FindAsync(id);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAsyncTask([FromBody] CreateOrderDto order)
    {
        if (order is null)
        {
            return BadRequest();
        }

        var createdOrder = await _orderService.AddAsync(order);

        return new ObjectResult(createdOrder) { StatusCode = StatusCodes.Status201Created };
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsyncTask([FromBody] UpdateOrderDto order)
    {
        if (order is null)
        {
            return BadRequest();
        }

        var updatedOrder = await _orderService.UpdateAsync(order);

        if (updatedOrder is null)
        {
            return NotFound();
        }

        return Ok(updatedOrder);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsyncTask(int id)
    {
        await _orderService.DeleteAsync(id);
        return Ok();
    }
}
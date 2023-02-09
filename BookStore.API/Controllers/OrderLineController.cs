using BLL.DTO.OrderLine;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderLineController : ControllerBase
{
    private readonly IOrderLineCatalogService _orderLineService;

    public OrderLineController(IOrderLineCatalogService orderLineService)
    {
        _orderLineService = orderLineService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsyncTask()
    {
        var orderLines = await _orderLineService.GetAllAsync();
        return Ok(orderLines);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsyncTask(int id)
    {
        var orderLine = await _orderLineService.FindAsync(id);
        return Ok(orderLine);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAsyncTask([FromBody] CreateOrderLineDto orderLine)
    {
        if (orderLine is null)
        {
            return BadRequest();
        }

        var createdOrderLine = await _orderLineService.AddAsync(orderLine);

        return new ObjectResult(createdOrderLine) { StatusCode = StatusCodes.Status201Created };
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsyncTask([FromBody] UpdateOrderLineDto orderLine)
    {
        if (orderLine is null)
        {
            return BadRequest();
        }

        var updatedOrderLine = await _orderLineService.UpdateAsync(orderLine);
        return Ok(updatedOrderLine);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsyncTask(int id)
    {
        await _orderLineService.DeleteAsync(id);
        return Ok();
    }
}
using BLL.DTO.Delivery;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryController : ControllerBase
{
    private readonly IDeliveryCatalogService _deliveryService;

    public DeliveryController(IDeliveryCatalogService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsyncTask()
    {
        var deliveries = await _deliveryService.GetAllAsync();
        return Ok(deliveries);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsyncTask(int id)
    {
        var delivery = await _deliveryService.FindAsync(id);
        return Ok(delivery);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAsyncTask([FromBody] CreateDeliveryDto delivery)
    {
        if (delivery is null)
        {
            return BadRequest();
        }

        var createdDelivery = await _deliveryService.AddAsync(delivery);

        return new ObjectResult(createdDelivery) { StatusCode = StatusCodes.Status201Created };
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsyncTask([FromBody] UpdateDeliveryDto delivery)
    {
        if (delivery is null)
        {
            return BadRequest();
        }

        var updatedDelivery = await _deliveryService.UpdateAsync(delivery);
        return Ok(updatedDelivery);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsyncTask(int id)
    {
        await _deliveryService.DeleteAsync(id);
        return Ok();
    }
}
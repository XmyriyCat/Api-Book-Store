using BLL.DTO.Shipment;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentController : ControllerBase
{
    private readonly IShipmentCatalogService _shipmentService;

    public ShipmentController(IShipmentCatalogService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsyncTask()
    {
        var shipments = await _shipmentService.GetAllAsync();
        return Ok(shipments);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsyncTask(int id)
    {
        var shipment = await _shipmentService.FindAsync(id);

        if (shipment is null)
        {
            return NotFound();
        }

        return Ok(shipment);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAsyncTask([FromBody] CreateShipmentDto shipment)
    {
        if (shipment is null)
        {
            return BadRequest();
        }

        var createdShipment = await _shipmentService.AddAsync(shipment);

        return new ObjectResult(createdShipment) { StatusCode = StatusCodes.Status201Created };
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsyncTask([FromBody] UpdateShipmentDto shipment)
    {
        if (shipment is null)
        {
            return BadRequest();
        }

        var updatedShipment = await _shipmentService.UpdateAsync(shipment);
        
        return Ok(updatedShipment);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsyncTask(int id)
    {
        await _shipmentService.DeleteAsync(id);
        return Ok();
    }
}
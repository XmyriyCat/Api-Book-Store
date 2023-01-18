using BLL.DTO.Warehouse;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseCatalogService _warehouseService;

    public WarehouseController(IWarehouseCatalogService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsyncTask()
    {
        var deliveries = await _warehouseService.GetAllAsync();
        return Ok(deliveries);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsyncTask(int id)
    {
        var warehouse = await _warehouseService.FindAsync(id);

        if (warehouse is null)
        {
            return NotFound();
        }

        return Ok(warehouse);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAsyncTask([FromBody] CreateWarehouseDto warehouse)
    {
        if (warehouse is null)
        {
            return BadRequest();
        }

        var createdWarehouse = await _warehouseService.AddAsync(warehouse);

        return new ObjectResult(createdWarehouse) { StatusCode = StatusCodes.Status201Created };
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsyncTask([FromBody] UpdateWarehouseDto warehouse)
    {
        if (warehouse is null)
        {
            return BadRequest();
        }

        var updatedWarehouse = await _warehouseService.UpdateAsync(warehouse);

        if (updatedWarehouse is null)
        {
            return NotFound();
        }

        return Ok(updatedWarehouse);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsyncTask(int id)
    {
        await _warehouseService.DeleteAsync(id);
        return Ok();
    }
}
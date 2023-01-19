using BLL.DTO.WarehouseBook;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseBookController : ControllerBase
{
    private readonly IWarehouseBookCatalogService _warehouseBookService;

    public WarehouseBookController(IWarehouseBookCatalogService warehouseBookService)
    {
        _warehouseBookService = warehouseBookService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsyncTask()
    {
        var warehouses = await _warehouseBookService.GetAllAsync();
        return Ok(warehouses);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsyncTask(int id)
    {
        var warehouseBook = await _warehouseBookService.FindAsync(id);

        if (warehouseBook is null)
        {
            return NotFound();
        }

        return Ok(warehouseBook);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAsyncTask([FromBody] CreateWarehouseBookDto warehouseBook)
    {
        if (warehouseBook is null)
        {
            return BadRequest();
        }

        var createdWarehouseBook = await _warehouseBookService.AddAsync(warehouseBook);

        return new ObjectResult(createdWarehouseBook) { StatusCode = StatusCodes.Status201Created };
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsyncTask([FromBody] UpdateWarehouseBookDto warehouseBook)
    {
        if (warehouseBook is null)
        {
            return BadRequest();
        }

        var updatedWarehouseBook = await _warehouseBookService.UpdateAsync(warehouseBook);

        if (updatedWarehouseBook is null)
        {
            return NotFound();
        }

        return Ok(updatedWarehouseBook);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsyncTask(int id)
    {
        await _warehouseBookService.DeleteAsync(id);
        return Ok();
    }
}
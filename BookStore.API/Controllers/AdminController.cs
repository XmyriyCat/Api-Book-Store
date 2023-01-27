using BLL.DTO.User;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminCatalogService _adminService;

    public AdminController(IAdminCatalogService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsyncTask()
    {
        var users = await _adminService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        var usersAmount = await _adminService.CountAsync();
        return Ok(usersAmount);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsyncTask(int id)
    {
        var user = await _adminService.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto createUser)
    {
        if (createUser is null)
        {
            return BadRequest();
        }

        var user = await _adminService.AddAsync(createUser);

        if (user is null)
        {
            return NotFound();
        }

        return new ObjectResult(user) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto updateUser)
    {
        if (updateUser is null)
        {
            return BadRequest();
        }

        var user = await _adminService.UpdateAsync(updateUser);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteByIdAsyncTask(int id)
    {
        await _adminService.DeleteAsync(id);
        return Ok();
    }
}
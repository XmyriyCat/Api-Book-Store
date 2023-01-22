using BLL.DTO.Role;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleCatalogService _roleService;

    public RoleController(IRoleCatalogService roleService)
    {
        _roleService = roleService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsyncTask()
    {
        var roles = await _roleService.GetAllAsync();
        return Ok(roles);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsyncTask(int id)
    {
        var role = await _roleService.FindAsync(id);

        if (role is null)
        {
            return NotFound();
        }

        return Ok(role);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAsyncTask([FromBody] CreateRoleDto role)
    {
        if (role is null)
        {
            return BadRequest();
        }

        var createdRole = await _roleService.AddAsync(role);

        return new ObjectResult(createdRole) { StatusCode = StatusCodes.Status201Created };
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsyncTask([FromBody] UpdateRoleDto role)
    {
        if (role is null)
        {
            return BadRequest();
        }

        var updatedRole = await _roleService.UpdateAsync(role);

        if (updatedRole is null)
        {
            return NotFound();
        }

        return Ok(updatedRole);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsyncTask(int id)
    {
        await _roleService.DeleteAsync(id);
        return Ok();
    }
}
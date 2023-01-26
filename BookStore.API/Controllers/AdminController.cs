using BLL.DTO.User;
using BLL.Services.Contract;
using DLL.Models;
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



    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsyncTask()
    {
        var users = await _adminService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet]
    public async Task<IActionResult> GetCount()
    {
        var usersAmount = await _adminService.CountAsync();
        return Ok(usersAmount);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetByIdAsyncTask(int id)
    {
        var user = await _adminService.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> DeleteByIdAsyncTask(int id)
    {
        await _adminService.DeleteAsync(id);

        

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync(UpdateUserDto updateUser)
    {
        var user = await _adminService.UpdateAsync(updateUser);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync(CreateUserDto createUser)
    {
        var user = await _adminService.AddAsync(createUser);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    
}
using BLL.DTO.Author;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorCatalogService _authorService;

    public AuthorController(IAuthorCatalogService authorService)
    {
        _authorService = authorService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsyncTask()
    {
        var authors = await _authorService.GetAllAsync();
        return Ok(authors);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsyncTask(int id)
    {
        var author = await _authorService.FindAsync(id);

        if (author is null)
        {
            return NotFound();
        }

        return Ok(author);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAsyncTask([FromBody] CreateAuthorDto author)
    {
        if (author is null)
        {
            return BadRequest();
        }

        var createdAuthor = await _authorService.AddAsync(author);

        return new ObjectResult(createdAuthor) { StatusCode = StatusCodes.Status201Created };
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsyncTask([FromBody] UpdateAuthorDto author)
    {
        if (author is null)
        {
            return BadRequest();
        }

        var updatedAuthor = await _authorService.UpdateAsync(author);

        if (updatedAuthor is null)
        {
            return NotFound();
        }

        return Ok(updatedAuthor);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsyncTask(int id)
    {
        await _authorService.DeleteAsync(id);
        return Ok();
    }
}
using BLL.DTO.Genre;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenreController : ControllerBase
{
    private readonly IGenreCatalogService _genreService;

    public GenreController(IGenreCatalogService genreService)
    {
        _genreService = genreService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsyncTask()
    {
        var genres = await _genreService.GetAllAsync();
        return Ok(genres);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsyncTask(int id)
    {
        var genre = await _genreService.FindAsync(id);

        if (genre is null)
        {
            return NotFound();
        }

        return Ok(genre);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAsyncTask([FromBody] CreateGenreDto genre)
    {
        if (genre is null)
        {
            return BadRequest();
        }

        var createdGenre = await _genreService.AddAsync(genre);

        return new ObjectResult(createdGenre) { StatusCode = StatusCodes.Status201Created };
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsyncTask([FromBody] UpdateGenreDto genre)
    {
        if (genre is null)
        {
            return BadRequest();
        }

        var updatedGenre = await _genreService.UpdateAsync(genre);

        if (updatedGenre is null)
        {
            return NotFound();
        }

        return Ok(updatedGenre);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsyncTask(int id)
    {
        await _genreService.DeleteAsync(id);
        return Ok();
    }
}
using BLL.DTO.Book;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookCatalogService _bookService;

    public BookController(IBookCatalogService bookService)
    {
        _bookService = bookService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsyncTask()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsyncTask(int id)
    {
        var book = await _bookService.FindAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAsyncTask([FromBody] CreateBookDto book)
    {
        if (book is null)
        {
            return BadRequest();
        }

        var createdBook = await _bookService.AddAsync(book);

        return new ObjectResult(createdBook) { StatusCode = StatusCodes.Status201Created };
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsyncTask([FromBody] UpdateBookDto book)
    {
        if (book is null)
        {
            return BadRequest();
        }

        var updatedBook = await _bookService.UpdateAsync(book);

        if (updatedBook is null)
        {
            return NotFound();
        }

        return Ok(updatedBook);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsyncTask(int id)
    {
        await _bookService.DeleteAsync(id);
        return Ok();
    }
}
using BLL.DTO.Book;
using BLL.Services.Contract;
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

    [HttpGet]
    public async Task<IActionResult> BookGetAllAsyncTask()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> BookGetByIdAsyncTask(int id)
    {
        var book = await _bookService.FindAsync(id);
                
        if (book is null)
        {
            return NotFound();
        }
    
        return Ok(book);
    }
    
    [HttpPost]
    public async Task<IActionResult> BookCreateAsyncTask([FromBody] CreateBookDto book)
    {
        if (book is null) 
        {
            return BadRequest();
        }
    
        var createdBook = await _bookService.AddAsync(book);
    
        return new ObjectResult(createdBook) { StatusCode = StatusCodes.Status201Created };
    }
    
    [HttpPut]
    public async Task<IActionResult> BookUpdateAsyncTask([FromBody] UpdateBookDto book)
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
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> BookDeleteAsyncTask(int id)
    {
        await _bookService.DeleteAsync(id);
        return Ok();
    }
}
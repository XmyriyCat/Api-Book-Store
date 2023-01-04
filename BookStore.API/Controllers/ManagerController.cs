using BLL.DTO.Author;
using BLL.DTO.Book;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ManagerController : ControllerBase
{
    private readonly IBookCatalogService _bookService;
    private readonly IAuthorCatalogService _authorService;
    
    public ManagerController(IBookCatalogService bookService, IAuthorCatalogService authorService) 
    { 
        _bookService = bookService;
        _authorService = authorService;
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
    
    [HttpGet]
    public async Task<IActionResult> AuthorGetAll()
    {
        var authors = await _authorService.GetAllAsync();
        return Ok(authors);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> AuthorGetByIdAsyncTask(int id)
    {
        var author = await _authorService.FindAsync(id);

        if (author is null)
        { 
            return NotFound();
        }

        return Ok(author);
    }

    [HttpPost]
    public async Task<IActionResult> AuthorCreateAsyncTask([FromBody] CreateAuthorDto author)
    {
        if (author is null)
        {
            return BadRequest();
        }

        var createdAuthor = await _authorService.AddAsync(author);

        return CreatedAtAction(nameof(AuthorGetByIdAsyncTask), new { id = createdAuthor.Id }, createdAuthor);
    }

    [HttpPut]
    public async Task<IActionResult> AuthorUpdateAsyncTask([FromBody] UpdateAuthorDto author)
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

    [HttpDelete("{id}")] 
    public async Task<IActionResult> AuthorDeleteAsyncTask(int id)
    {
        await _authorService.DeleteAsync(id);
        return Ok();
    }
}
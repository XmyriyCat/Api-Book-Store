using BLL.DTO.Author;
using BLL.DTO.Book;
using BLL.DTO.Genre;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ManagerController : ControllerBase
{
    private readonly IBookCatalogService _bookService;
    private readonly IAuthorCatalogService _authorService;
    private readonly IGenreCatalogService _genreService;
    
    public ManagerController(IBookCatalogService bookService, IAuthorCatalogService authorService, IGenreCatalogService genreService) 
    { 
        _bookService = bookService;
        _authorService = authorService;
        _genreService = genreService;
    }
    
    [HttpGet]
    public async Task<IActionResult> BookGetAllAsyncTask()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }
    
    [HttpGet]
    public async Task<IActionResult> AuthorGetAllAsyncTask()
    {
        var authors = await _authorService.GetAllAsync();
        return Ok(authors);
    }

    [HttpGet]
    public async Task<IActionResult> GenreGetAllAsyncTask()
    {
        var genres = await _genreService.GetAllAsync();
        return Ok(genres);
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GenreGetByIdAsyncTask(int id)
    {
        var genre = await _genreService.FindAsync(id);

        if (genre is null)
        {
            return NotFound();
        }

        return Ok(genre);
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

    [HttpPost]
    public async Task<IActionResult> GenreCreateAsyncTask([FromBody] CreateGenreDto genre)
    {
        if (genre is null)
        {
            return BadRequest();
        }

        var createGenre = await _genreService.AddAsync(genre);

        return CreatedAtAction(nameof(GenreGetByIdAsyncTask), new {id = createGenre.Id}, createGenre);
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

    [HttpPut]
    public async Task<IActionResult> GenreUpdateAsyncTask([FromBody] UpdateGenreDto genre)
    {
        if (genre is null)
        {
            return BadRequest();
        }

        var updateGenre = await _genreService.UpdateAsync(genre);

        if (updateGenre is null)
        {
            return NotFound();
        }

        return Ok(updateGenre);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> BookDeleteAsyncTask(int id)
    {
        await _bookService.DeleteAsync(id);
        return Ok();
    }
    
    [HttpDelete("{id}")] 
    public async Task<IActionResult> AuthorDeleteAsyncTask(int id)
    {
        await _authorService.DeleteAsync(id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> GenreDeleteAsyncTask(int id)
    {
        await _genreService.DeleteAsync(id);
        return Ok();
    }
}
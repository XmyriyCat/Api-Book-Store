using BLL.DTO.Author;
using BLL.DTO.Book;
using BLL.DTO.Genre;
using BLL.DTO.Publisher;
using BLL.Services.Contract;
using DLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ManagerController : ControllerBase
{
    private readonly IAuthorCatalogService _authorService;
    private readonly IBookCatalogService _bookService;
    private readonly IGenreCatalogService _genreService;
    private readonly IPublisherCatalogService _publisherService;
    private readonly ITokenService _tokenService;
    private readonly IUserCatalogService _userService;


    public ManagerController(
        IAuthorCatalogService authorService,
        IBookCatalogService bookService,
        IGenreCatalogService genreService,
        IPublisherCatalogService publisherService,
        ITokenService tokensService,
        IUserCatalogService userService

        )
    {
        _authorService = authorService;
        _bookService = bookService;
        _genreService = genreService;
        _publisherService = publisherService;
        _tokenService = tokensService;
        _userService = userService;

    }

    //[AllowAnonymous]
    //[HttpGet("author")]
    //public async Task<IActionResult> AuthorGetAllAsync(CreateBookDto createBookDto)
    //{
    //    _bookService.AddAsync(createBookDto);
    //    return Ok();
    //}

    [AllowAnonymous]
    [HttpGet("author")]
    public async Task<IActionResult> AuthorGetAllAsync()
    {
        var authors = await _authorService.GetAllAsync();

        return Ok(authors);
    }

    [AllowAnonymous]
    [HttpGet("author")]
    public async Task<IActionResult> AuthorFindAsync(int id)
    {
        var author = await _authorService.FindAsync(id);

        return Ok(author);
    }

    [AllowAnonymous]
    [HttpGet("author/{id}")]
    public async Task<IActionResult> AuthorGetByIdAsync(int id)
    {
        var author = await _authorService.FindAsync(id);

        if (author is null)
        {
            return NotFound();
        }

        return Ok(author);
    }

    [AllowAnonymous]
    [HttpPost("author")]
    public async Task<IActionResult> AddAsync(CreateAuthorDto item)
    {
        var author = await _authorService.AddAsync(item);

        return Ok(author);
    }

    [AllowAnonymous]
    [HttpPut("author")]
    public async Task<IActionResult> UpdateAsync(UpdateAuthorDto item)
    {
        var author = await _authorService.UpdateAsync(item);

        return Ok(author);
    }

    [AllowAnonymous]
    [HttpDelete("author")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _authorService.DeleteAsync(id);

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("author")]
    public async Task<IActionResult> CountAsync(int id)
    {
        var count = await _authorService.CountAsync();

        return Ok(count);
    }













    [AllowAnonymous]
    [HttpPost("token")]
    public IActionResult CreateToken(User user)
    {
        var token = _tokenService.CreateToken(user);
        return Ok(token);
    }

    [AllowAnonymous]
    [HttpGet("genre")]
    public async Task<IActionResult> GenreGetAllAsync()
    {
        var genres = await _genreService.GetAllAsync();
        return Ok(genres);
    }

    [AllowAnonymous]
    [HttpGet("publisher")]
    public async Task<IActionResult> PublisherGetAllAsync()
    {
        var publishers = await _publisherService.GetAllAsync();
        return Ok(publishers);
    }




    [AllowAnonymous]
    [HttpGet("genre/{id}")]
    public async Task<IActionResult> GenreGetByIdAsync(int id)
    {
        var genre = await _genreService.FindAsync(id);

        if (genre is null)
        {
            return NotFound();
        }

        return Ok(genre);
    }

    [AllowAnonymous]
    [HttpGet("publisher/{id}")]
    public async Task<IActionResult> PublisherGetByIdAsync(int id)
    {
        var publisher = await _publisherService.FindAsync(id);

        if (publisher is null)
        {
            return NotFound();
        }

        return Ok(publisher);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("author")]
    public async Task<IActionResult> AuthorCreateAsync([FromBody] CreateAuthorDto author)
    {
        if (author is null)
        {
            return BadRequest();
        }

        var createdAuthor = await _authorService.AddAsync(author);

        return CreatedAtAction(nameof(AuthorGetByIdAsync), new { id = createdAuthor.Id }, createdAuthor);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("genre")]
    public async Task<IActionResult> GenreCreateAsync([FromBody] CreateGenreDto genre)
    {
        if (genre is null)
        {
            return BadRequest();
        }

        var createGenre = await _genreService.AddAsync(genre);

        return CreatedAtAction(nameof(GenreGetByIdAsync), new { id = createGenre.Id }, createGenre);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("publisher")]
    public async Task<IActionResult> PublisherCreateAsync([FromBody] CreatePublisherDto publisher)
    {
        if (publisher is null)
        {
            return BadRequest();
        }

        var createPublisher = await _publisherService.AddAsync(publisher);

        return CreatedAtAction(nameof(PublisherCreateAsync), new { id = createPublisher.Id }, createPublisher);
    }

    [Authorize(Roles = "Manager")]
    [HttpPut("author")]
    public async Task<IActionResult> AuthorUpdateAsync([FromBody] UpdateAuthorDto author)
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
    [HttpPut("genre")]
    public async Task<IActionResult> GenreUpdateAsync([FromBody] UpdateGenreDto genre)
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

    [Authorize(Roles = "Manager")]
    [HttpPut("publisher")]
    public async Task<IActionResult> PublisherUpdateAsync([FromBody] UpdatePublisherDto publisher)
    {
        if (publisher is null)
        {
            return BadRequest();
        }

        var updatePublisher = await _publisherService.UpdateAsync(publisher);

        return Ok(updatePublisher);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("author/{id}")]
    public async Task<IActionResult> AuthorDeleteAsync(int id)
    {
        await _authorService.DeleteAsync(id);
        return Ok();
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("genre/{id}")]
    public async Task<IActionResult> GenreDeleteAsync(int id)
    {
        await _genreService.DeleteAsync(id);
        return Ok();
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("publisher/{id}")]
    public async Task<IActionResult> PublisherDeleteAsync(int id)
    {
        await _publisherService.DeleteAsync(id);
        return Ok();
    }
}
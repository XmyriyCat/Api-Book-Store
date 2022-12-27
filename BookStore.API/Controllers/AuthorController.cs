using BLL.DTO.Author;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {   
        private readonly IAuthorCatalogService _authorService;

        public AuthorController(IAuthorCatalogService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var authors = _authorService.GetAll();
            return Ok(authors);
        }


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

        [HttpPost]
        public async Task<IActionResult> CreateAsyncTask([FromBody] CreateAuthorDto author)
        {
            if (author is null)
            {
                return BadRequest();
            }

            var createdAuthor = await _authorService.AddAsync(author);

            return CreatedAtAction(nameof(GetByIdAsyncTask), new { id = createdAuthor.Id }, createdAuthor);
        }

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsyncTask(int id)
        {
            await _authorService.DeleteAsync(id);
            return Ok();
        }
    }
}

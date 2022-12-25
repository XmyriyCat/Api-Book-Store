using DLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AuthorService _authorService;
        public AuthorController(AuthorService AuthorService)
        {
            _authorService = AuthorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var Authors = await _authorService.GetAllAsync();
            return Ok(Authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var Author = await _authorService.GetAuthorByIdAsync(id);
            if (Author == null)
            {
                return NotFound();
            }
            return Ok(Author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] Author Author)
        {
            if (Author == null)
            {
                return BadRequest();
            }

            var createdAuthor = await _authorService.CreateAuthorAsync(Author);
            return CreatedAtAction("GetAuthorById", new { id = createdAuthor.Id }, createdAuthor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author Author)
        {
            if (Author == null || Author.Id != id)
            {
                return BadRequest();
            }

            var updatedAuthor = await _authorService.UpdateAuthorAsync(Author);
            if (updatedAuthor == null)
            {
                return NotFound();
            }

            return Ok(updatedAuthor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var Author = await _authorService.GetAuthorByIdAsync(id);
            if (Author == null)
            {
                return NotFound();
            }

            await _authorService.DeleteAuthorAsync(Author);
            return Ok();
        }
    }
}

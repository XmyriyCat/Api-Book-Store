using BLL.DTO.Author;
using BLL.Services.Interfaces;
using DLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers
{
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {   
        private readonly IAuthorCatalogService _authorService;
        public AuthorController(IAuthorCatalogService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authors = _authorService.GetAll();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var Author = await _authorService.FindAsync(id);
            if (Author == null)
            {
                return NotFound();
            }
            return Ok(Author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            var createdAuthor = await _authorService.AddAsync(author);
            return CreatedAtAction("GetAuthorById", new { id = createdAuthor.Id }, createdAuthor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor([FromBody] UpdateAuthorDto author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            var updatedAuthor = await _authorService.Update(author);
            if (updatedAuthor == null)
            {
                return NotFound();
            }

            return Ok(updatedAuthor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorService.DeleteAsync(id);
            return Ok();
        }
    }
}

using DLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IBookCatalogService _bookService;

        public BookController(IBookCatalogService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/book
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAll();
            return Ok(books);
        }

        // GET: api/book/5
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

        // POST: api/book
        [HttpPost]
        public async Task<IActionResult> CreateAsyncTask([FromBody] Book book)
        {
            if (book is null)
            {
                return BadRequest();
            }

            await _bookService.CreateAsync(book);

            return CreatedAtAction(nameof(GetByIdAsyncTask), new { id = book.Id }, book);
        }

        // PUT: api/book/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Book book)
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

        // DELETE: api/book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedBook = await _bookService.DeleteAsync(id);

            if (deletedBook is null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}

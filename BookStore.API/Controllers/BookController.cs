using BLL.DTO.Book;
using BLL.Services.Contract;
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
            var books = await _bookService.GetAllAsync();
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
        public async Task<IActionResult> CreateAsyncTask([FromBody] CreateBookDto book)
        {
            if (book is null)
            {
                return BadRequest();
            }

            var createdBook = await _bookService.AddAsync(book);

            return CreatedAtAction(nameof(GetByIdAsyncTask), new { id = createdBook.Id }, createdBook);
        }

        // PUT: api/book/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateBookDto book)
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
            await _bookService.DeleteAsync(id);
            return Ok();
        }
    }
}

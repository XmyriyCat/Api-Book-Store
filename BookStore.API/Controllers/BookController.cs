using DLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers
{
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
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
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookService.GetById(id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        // POST: api/book
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            if (book == null)
                return BadRequest();

            await _bookService.Create(book);

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        // PUT: api/book/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Book book)
        {
            if (book is null)
                return BadRequest();

            var updatedBook = await _bookService.Update(book);
            if (updatedBook == null)
                return NotFound();

            return Ok(updatedBook);
        }

        // DELETE: api/book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedBook = await _bookService.Delete(id);
            if (deletedBook == null)
                return NotFound();

            return Ok();
        }
    }
}

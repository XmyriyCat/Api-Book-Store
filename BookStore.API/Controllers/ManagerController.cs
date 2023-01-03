using BLL.DTO.Author;
using BLL.Services.Interfaces;
using DLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;

namespace ManagerController
{
    [Route("api/[controller]")]
    public class ManagerController : Controller
    {
        private readonly IBookService _bookService;

        private readonly IAuthorCatalogService _authorService;
        public ManagerController(IAuthorCatalogService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authors = _authorService.GetAll();
            return Ok(authors);
        }

        [HttpGet("author/{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _authorService.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
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

        [HttpPut("author/{id}")]
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

        [HttpDelete("author/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorService.DeleteAsync(id);
            return Ok();
        }

        // GET: api/Manager/book
        [HttpGet("book")]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAll();
            return Ok(books);
        }

        // GET: api/Manager/book/5
        [HttpGet("book/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookService.GetById(id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        // POST: api/Manager/book
        [HttpPost("book")]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            if (book == null)
                return BadRequest();

            await _bookService.Create(book);

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        // PUT: api/Manager/book/5
        [HttpPut("book/{id}")]
        public async Task<IActionResult> Update([FromBody] Book book)
        {
            if (book is null)
                return BadRequest();

            var updatedBook = await _bookService.Update(book);
            if (updatedBook == null)
                return NotFound();

            return Ok(updatedBook);
        }

        // DELETE: api/Manager/book/5
        [HttpDelete("book/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedBook = await _bookService.Delete(id);
            if (deletedBook == null)
                return NotFound();

            return Ok();
        }
    }
}

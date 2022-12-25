using DLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AuthorService _authorService;
        public AuthorController(AuthorService userService)
        {
            _authorService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _authorService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _authorService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var createdUser = await _authorService.CreateUserAsync(user);
            return CreatedAtAction("GetUserById", new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null || user.Id != id)
            {
                return BadRequest();
            }

            var updatedUser = await _authorService.UpdateUserAsync(user);
            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _authorService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _authorService.DeleteUserAsync(user);
            return Ok();
        }
    }
}

using BLL.DTO.User;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserCatalogService _userService;

        public UserController(IUserCatalogService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync(RegistrationUserDto user)
        {
            var registeredUser = await _userService.RegisterAsync(user);
            return Ok(registeredUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync(LoginUserDto user)
        {
            var registeredUser = await _userService.LoginAsync(user);
            return Ok(registeredUser);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticationTestGet()
        {
            return Ok(new[] { "Hello", "authenticated", "user" });
        }
    }
}

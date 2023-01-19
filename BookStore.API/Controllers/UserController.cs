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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync(RegistrationUserDto user)
        {
            var registeredUser = await _userService.RegisterAsync(user);
            return Ok(registeredUser);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync(LoginUserDto user)
        {
            var registeredUser = await _userService.LoginAsync(user);
            return Ok(registeredUser);
        }

        [AllowAnonymous]
        [HttpPost("register-google")]
        public async Task<IActionResult> RegisterGoogleAsync(string googleToken, string password)
        {
            var registeredUser = await _userService.RegisterGoogleAsync(googleToken, password);
            return Ok(registeredUser);
        }

        [AllowAnonymous]
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogleAsync(string googleToken)
        {
            var registeredUser = await _userService.LoginGoogleAsync(googleToken);
            return Ok(registeredUser);
        }
    }
}

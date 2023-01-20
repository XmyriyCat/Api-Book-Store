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
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegistrationUserDto user)
        {
            var registeredUser = await _userService.RegisterAsync(user);
            return Ok(registeredUser);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserDto user)
        {
            var registeredUser = await _userService.LoginAsync(user);
            return Ok(registeredUser);
        }

        [AllowAnonymous]
        [HttpPost("register-google")]
        public async Task<IActionResult> RegisterGoogleAsync([FromBody] RegistrationGoogleUserDto registrationGoogleUserDto)
        {
            var registeredUser = await _userService.RegisterGoogleAsync(registrationGoogleUserDto);
            return Ok(registeredUser);
        }

        [AllowAnonymous]
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogleAsync([FromBody] LoginGoogleUserDto loginGoogleUserDto)
        {
            var registeredUser = await _userService.LoginGoogleAsync(loginGoogleUserDto);
            return Ok(registeredUser);
        }
    }
}

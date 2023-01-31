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
        private readonly IUserGoogleCatalogService _userGoogleService;

        public UserController(IUserCatalogService userService, IUserGoogleCatalogService userGoogleService)
        {
            _userService = userService;
            _userGoogleService = userGoogleService;
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
            var registeredUser = await _userGoogleService.RegisterGoogleAsync(registrationGoogleUserDto);
            return Ok(registeredUser);
        }

        [AllowAnonymous]
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogleAsync([FromBody] LoginGoogleUserDto loginGoogleUserDto)
        {
            var registeredUser = await _userGoogleService.LoginGoogleAsync(loginGoogleUserDto);
            return Ok(registeredUser);
        }
    }
}

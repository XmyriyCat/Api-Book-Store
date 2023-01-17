using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using BLL.DTO.User;
using BLL.Services.Contract;
using DLL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using static System.Net.WebRequestMethods;

namespace ApiBookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserCatalogService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUserCatalogService userService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
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
        [HttpGet("login-google")]
        public IActionResult LoginGoogleAsync()
        {
            var redirectUrl = Url.Action("GoogleResponse", "User");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [HttpGet("GoogleResponse")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return NotFound("Info is null!");
            }
 
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result.Succeeded)
            {
                return Ok(userInfo);
            }
            else
            {
                User user = new User
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };
 
                var identResult = await _userManager.CreateAsync(user);

                if (identResult.Succeeded)
                {
                    identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return Ok(userInfo);
                    }
                }
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("logout-google")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync();
        }

        [HttpPost("Microsoft-Post")]
        [AllowAnonymous]
        public IActionResult MicrosoftLogin()
        {
            var Googleurl = "https://accounts.google.com/o/oauth2/auth?response_type=code&redirect_uri=" + "https://localhost:8000" + "&scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile&client_id=" + "263574892200-knp9l4edttla8kh1u2ukjma2qed2dq4s.apps.googleusercontent.com";
            Response.Redirect(Googleurl);
            return Ok(Googleurl);
        }
    }
}

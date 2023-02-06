using System.Net;
using System.Net.Http.Json;
using ApiBookStore;
using BLL.DTO.User;
using Newtonsoft.Json;
using Web_Api.Tests.Startup;
using Xunit;

namespace Web_Api.Tests.Controllers
{
    public class UserControllerTest : IClassFixture<WebApplicationFactoryTest<Program>>
    {
        private readonly WebApplicationFactoryTest<Program> _appFactory;

        public UserControllerTest(WebApplicationFactoryTest<Program> appFactory)
        {
            _appFactory = appFactory;
        }

        [Theory]
        [InlineData("/api/user/register")]
        public async Task RegisterUserAsync_Return_Ok(string url)
        {
            // Arranges
            var client = _appFactory.CreateClient();

            var registrationUserDto = new RegistrationUserDto
            {
                Login = "some-login",
                Password = "some-password",
                Username = "Test_username"
            };

            // Act
            var response = await client.PostAsJsonAsync(url, registrationUserDto);

            var userString = await response.Content.ReadAsStringAsync();
            var userCreated = JsonConvert.DeserializeObject<AuthorizedUserDto>(userString)!;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(userCreated);
            Assert.NotEmpty(userCreated.Username);
            Assert.NotEmpty(userCreated.JwtToken);
            Assert.Equal(registrationUserDto.Username, userCreated.Username);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/user/register")]
        public async Task RegisterUserAsync_Return_BadRequest(string url)
        {
            // Arranges
            var client = _appFactory.CreateClient();

            // Invalid data
            var registrationUserDto = new RegistrationUserDto
            {
                Login = string.Empty,
                Password = string.Empty,
                Username = string.Empty
            };

            // Act
            var response = await client.PostAsJsonAsync(url, registrationUserDto);
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/user/register")]
        public async Task RegisterUserAsync_Return_BadRequest_Used_Login(string url)
        {
            // Arranges
            var client = _appFactory.CreateClient();

            // Already created Login from DbUtilities class
            var registrationUserDto = new RegistrationUserDto
            {
                Login = "test-login",
                Password = "some-password",
                Username = "some-username"
            };

            // Act
            var response = await client.PostAsJsonAsync(url, registrationUserDto);
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/user/login")]
        public async Task LoginUserAsync_Return_Ok(string url)
        {
            // Arranges
            var client = _appFactory.CreateClient();

            // Certain password and login from DbUtilities class
            var registrationUserDto = new LoginUserDto
            {
                Login = "test-login",
                Password = "123asd456",
            };

            // Act
            var response = await client.PostAsJsonAsync(url, registrationUserDto);

            var userString = await response.Content.ReadAsStringAsync();
            var authorizedUser = JsonConvert.DeserializeObject<AuthorizedUserDto>(userString)!;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(authorizedUser);
            Assert.NotEmpty(authorizedUser.Username);
            Assert.NotEmpty(authorizedUser.JwtToken);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/user/login")]
        public async Task LoginUserAsync_Return_Unauthorized(string url)
        {
            // Arranges
            var client = _appFactory.CreateClient();

            // Non-existent Login and Password
            var registrationUserDto = new LoginUserDto
            {
                Login = "some-login",
                Password = "123456",
            };

            // Act
            var response = await client.PostAsJsonAsync(url, registrationUserDto);
            
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/user/login")]
        public async Task LoginUserAsync_Return_BadRequest(string url)
        {
            // Arranges
            var client = _appFactory.CreateClient();

            // Invalid data
            var registrationUserDto = new LoginUserDto
            {
                Login = string.Empty,
                Password = string.Empty,
            };

            // Act
            var response = await client.PostAsJsonAsync(url, registrationUserDto);
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/user/register-google")]
        public async Task RegisterGoogleAsync_Return_Unauthorized(string url)
        {
            // Arranges
            var client = _appFactory.CreateClient();

            // Invalid GoogleToken
            var registrationGoogleUserDto = new RegistrationGoogleUserDto
            {
                GoogleToken = "some-token",
                Password = "some-password"
            };

            // Act
            var response = await client.PostAsJsonAsync(url, registrationGoogleUserDto);
            
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/user/login-google")]
        public async Task LoginGoogleAsync_Return_Unauthorized(string url)
        {
            // Arranges
            var client = _appFactory.CreateClient();

            // Invalid GoogleToken
            var registrationGoogleUserDto = new LoginGoogleUserDto
            {
                GoogleToken = "some-token",
            };

            // Act
            var response = await client.PostAsJsonAsync(url, registrationGoogleUserDto);
            
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.ToString());
        }
    }
}

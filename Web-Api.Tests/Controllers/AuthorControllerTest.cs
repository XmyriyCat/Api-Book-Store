using ApiBookStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using BLL.DTO.Author;
using DLL.Models;
using Newtonsoft.Json;
using Web_Api.Tests.Extensions;
using Web_Api.Tests.Startup;
using Web_Api.Tests.Startup.JwtHandler;
using Xunit;

namespace Web_Api.Tests.Controllers
{
    public class AuthorControllerTest : IClassFixture<WebApplicationFactoryTest<Program>>
    {
        private readonly WebApplicationFactoryTest<Program> _appFactory;
        private readonly TokenServiceTest _tokenJwtService;

        public AuthorControllerTest(WebApplicationFactoryTest<Program> appFactory)
        {
            _appFactory = appFactory;

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            _tokenJwtService = new TokenServiceTest(config);
        }

        [Theory]
        [InlineData("/api/author")]
        public async Task AuthorGetAllAsyncTask_Return_Ok(string url)
        {
            // Arranges
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/author/1")]
        [InlineData("/api/author/2")]
        public async Task AuthorGetByIdAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/author")]
        public async Task AuthorCreateAsyncTask_Return_Created_201(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var createAuthorDto = new CreateAuthorDto
            {
                FirstName = "Test-first-name",
                LastName = "Test-last-name"
            };

            // Act
            var response = await client.PostAsJsonAsync(url, createAuthorDto);

            var authorString = await response.Content.ReadAsStringAsync();
            var authorCreated = JsonConvert.DeserializeObject<Author>(authorString)!;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(createAuthorDto.FirstName, authorCreated.FirstName);
            Assert.Equal(createAuthorDto.LastName, authorCreated.LastName);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/author")]
        public async Task AuthorCreateAsyncTask_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            CreateAuthorDto authorDtoNull = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var response = await client.PostAsJsonAsync(url, authorDtoNull);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/author")]
        public async Task AuthorCreateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateAuthorDto
            {
                FirstName = "Test-first-name",
                LastName = "Test-last-name"
            });

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/author")]
        public async Task AuthorCreateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateAuthorDto
            {
                FirstName = string.Empty,
                LastName = string.Empty
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/author")]
        public async Task AuthorCreateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateAuthorDto
            {
                FirstName = "Test-first-name",
                LastName = "Test-last-name"
            });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/author")]
        public async Task AuthorUpdateAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateAuthorDto = new UpdateAuthorDto()
            {
                Id = 1,
                FirstName = "Test-first-name-EDIT",
                LastName = "Test-last-name-EDIT"
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateAuthorDto);

            var authorString = await response.Content.ReadAsStringAsync();
            var authorUpdated = JsonConvert.DeserializeObject<Author>(authorString);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updateAuthorDto.Id, authorUpdated!.Id);
            Assert.Equal(updateAuthorDto.FirstName, authorUpdated.FirstName);
            Assert.Equal(updateAuthorDto.LastName, authorUpdated.LastName);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/author")]
        public async Task AuthorUpdateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateAuthorDto = new UpdateAuthorDto()
            {
                Id = 1,
                FirstName = "Test-first-name-EDIT",
                LastName = "Test-last-name-EDIT"
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateAuthorDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/author")]
        public async Task AuthorUpdateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var updateAuthorDto = new UpdateAuthorDto()
            {
                Id = 1,
                FirstName = "Test-first-name-EDIT",
                LastName = "Test-last-name-EDIT"
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateAuthorDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/author")]
        public async Task AuthorUpdateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateAuthorDto = new UpdateAuthorDto()
            {
                Id = 1,
                FirstName = string.Empty,
                LastName = "Test-last-name-EDIT"
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateAuthorDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/author")]
        public async Task AuthorUpdateAsyncTask_Return_NotFound_404(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateAuthorDto = new UpdateAuthorDto()
            {
                Id = 999999999,
                FirstName = "Test-first-name",
                LastName = "Test-last-name"
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateAuthorDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/author/3")]
        public async Task AuthorDeleteAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("api/author/3")]
        public async Task AuthorDeleteAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/author/2")]
        public async Task AuthorDeleteAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}

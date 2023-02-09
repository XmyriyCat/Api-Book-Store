using System.Net;
using System.Net.Http.Json;
using ApiBookStore;
using BLL.DTO.Book;
using DLL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Web_Api.Tests.Extensions;
using Web_Api.Tests.Startup;
using Web_Api.Tests.Startup.JwtHandler;
using Xunit;

namespace Web_Api.Tests.Controllers
{
    public class BookControllerTest : IClassFixture<WebApplicationFactoryTest<Program>>
    {
        private readonly WebApplicationFactoryTest<Program> _appFactory;
        private readonly TokenServiceTest _tokenJwtService;

        public BookControllerTest(WebApplicationFactoryTest<Program> appFactory)
        {
            _appFactory = appFactory;

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();
            
            _tokenJwtService = new TokenServiceTest(config);
        }

        [Theory]
        [InlineData("/api/book")]
        public async Task BookGetAllAsyncTask_Return_Ok(string url)
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
        [InlineData("/api/book/1")]
        public async Task BookGetByIdAsyncTask_Return_Ok(string url)
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
        [InlineData("api/book")]
        public async Task BookCreateAsyncTask_Return_Created_201(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();
            
            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var createBookDto = new CreateBookDto
            {
                Name = "Test-book",
                Price = 99.99m,
                IdPublisher = 1,
                GenresId = new List<int> { 1 },
                AuthorsId = new List<int> { 1 }
            };

            // Act
            var response = await client.PostAsJsonAsync(url, createBookDto);

            var bookString = await response.Content.ReadAsStringAsync();
            var bookCreated = JsonConvert.DeserializeObject<Book>(bookString)!;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(createBookDto.Name, bookCreated.Name);
            Assert.Equal(createBookDto.Price, bookCreated.Price);
            Assert.Equal(createBookDto.IdPublisher, bookCreated.Publisher.Id);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/book")]
        public async Task BookCreateAsyncTask_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            CreateBookDto bookDtoNull = null;
            // ReSharper disable once ExpressionIsAlwaysNull    
            var response = await client.PostAsJsonAsync(url, bookDtoNull);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/book")]
        public async Task BookCreateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateBookDto
            {
                Name = "Test-book",
                Price = 99.99m,
                IdPublisher = 1,
                GenresId = new List<int> { 1 },
                AuthorsId = new List<int> { 1 }
            });

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/book")]
        public async Task BookCreateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateBookDto
            {
                Name = string.Empty,
                Price = -99.99m,
                IdPublisher = 0,
                GenresId = new List<int> { 0 },
                AuthorsId = new List<int> { 0 }
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/book")]
        public async Task BookCreateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateBookDto
            {
                Price = 99.99m,
                IdPublisher = 1,
                GenresId = new List<int> { 1 },
                AuthorsId = new List<int> { 1 }
            });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/book")]
        public async Task BookUpdateAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateBookDto = new UpdateBookDto()
            {
                Id = 2,
                Name = "Test-book-EDITED",
                Price = 11.11m,
                IdPublisher = 1,
                GenresId = new List<int> { 1 },
                AuthorsId = new List<int> { 1 }
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateBookDto);

            var bookString = await response.Content.ReadAsStringAsync();
            var bookUpdated = JsonConvert.DeserializeObject<Book>(bookString);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updateBookDto.Id, bookUpdated!.Id);
            Assert.Equal(updateBookDto.Name, bookUpdated.Name);
            Assert.Equal(updateBookDto.Price, bookUpdated.Price);
            Assert.Equal(updateBookDto.IdPublisher, bookUpdated.Publisher.Id);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/book")]
        public async Task BookUpdateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateBookDto = new UpdateBookDto()
            {
                Id = 1,
                Name = "Test-book-EDITED",
                Price = 11.11m,
                IdPublisher = 1,
                GenresId = new List<int> { 1 },
                AuthorsId = new List<int> { 1 }
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateBookDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/book")]
        public async Task BookUpdateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var updateBookDto = new UpdateBookDto()
            {
                Id = 1,
                Name = "Test-book-EDITED",
                Price = 11.11m,
                IdPublisher = 1,
                GenresId = new List<int> { 1 },
                AuthorsId = new List<int> { 1 }
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateBookDto);
            
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/book")]
        public async Task BookUpdateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateBookDto = new UpdateBookDto()
            {
                Id = 1,
                Name = string.Empty,
                Price = -99.99m,
                IdPublisher = 0,
                GenresId = new List<int> { 0 },
                AuthorsId = new List<int> { 0 }
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateBookDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/book")]
        public async Task BookUpdateAsyncTask_Return_NotFound_404(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateBookDto = new UpdateBookDto()
            {
                Id = 99999,
                Name = "Test-book-EDITED",
                Price = 11.11m,
                IdPublisher = 1,
                GenresId = new List<int> { 1 },
                AuthorsId = new List<int> { 1 }
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateBookDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/book/2")]
        public async Task BookDeleteAsyncTask_ReturnOk(string url)
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
        [InlineData("api/book/3")]
        public async Task BookDeleteAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/book/2")]
        public async Task BookDeleteAsyncTask_Return_Forbidden_403(string url)
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
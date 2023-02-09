using ApiBookStore;
using BLL.DTO.WarehouseBook;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using DLL.Models;
using Web_Api.Tests.Extensions;
using Web_Api.Tests.Startup;
using Web_Api.Tests.Startup.JwtHandler;
using Xunit;

namespace Web_Api.Tests.Controllers
{
    public class WarehouseBookControllerTest : IClassFixture<WebApplicationFactoryTest<Program>>
    {
        private readonly WebApplicationFactoryTest<Program> _appFactory;
        private readonly TokenServiceTest _tokenJwtService;

        public WarehouseBookControllerTest(WebApplicationFactoryTest<Program> appFactory)
        {
            _appFactory = appFactory;

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            _tokenJwtService = new TokenServiceTest(config);
        }

        [Theory]
        [InlineData("/api/warehouseBook")]
        public async Task WarehouseBookGetAllAsyncTask_Return_Ok(string url)
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
        [InlineData("/api/warehouseBook/9")]
        public async Task WarehouseBookGetByIdAsyncTask_Return_Ok(string url)
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
        [InlineData("api/warehouseBook")]
        public async Task WarehouseBookCreateAsyncTask_Return_Created_201(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var createWarehouseBookDto = new CreateWarehouseBookDto
            {
                Quantity = 1,
                WarehouseId = 3,
                BookId = 4,
            };

            // Act
            var response = await client.PostAsJsonAsync(url, createWarehouseBookDto);

            var warehouseBookString = await response.Content.ReadAsStringAsync();
            var warehouseBookCreated = JsonConvert.DeserializeObject<WarehouseBook>(warehouseBookString)!;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(createWarehouseBookDto.Quantity, warehouseBookCreated.Quantity);
            Assert.Equal(createWarehouseBookDto.WarehouseId, warehouseBookCreated.WarehouseId);
            Assert.Equal(createWarehouseBookDto.BookId, warehouseBookCreated.BookId);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/warehouseBook")]
        public async Task WarehouseBookCreateAsyncTask_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            CreateWarehouseBookDto warehouseBookDtoNull = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var response = await client.PostAsJsonAsync(url, warehouseBookDtoNull);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouseBook")]
        public async Task WarehouseBookCreateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateWarehouseBookDto
            {
                Quantity = 1,
                WarehouseId = 1,
                BookId = 1
            });

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouseBook")]
        public async Task WarehouseBookCreateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateWarehouseBookDto
            {
                Quantity = 0,
                WarehouseId = 0,
                BookId = 0
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouseBook")]
        public async Task WarehouseBookCreateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateWarehouseBookDto
            {
                Quantity = 1,
                WarehouseId = 1,
                BookId = 1
            });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouseBook")]
        public async Task WarehouseBookUpdateAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateWarehouseBookDto = new UpdateWarehouseBookDto()
            {
                Id = 2,
                Quantity = 1,
                WarehouseId = 1,
                BookId = 1
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateWarehouseBookDto);

            var warehouseBookString = await response.Content.ReadAsStringAsync();
            var warehouseBookUpdated = JsonConvert.DeserializeObject<WarehouseBook>(warehouseBookString);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updateWarehouseBookDto.Id, warehouseBookUpdated!.Id);
            Assert.Equal(updateWarehouseBookDto.Quantity, warehouseBookUpdated.Quantity);
            Assert.Equal(updateWarehouseBookDto.WarehouseId, warehouseBookUpdated.WarehouseId);
            Assert.Equal(updateWarehouseBookDto.BookId, warehouseBookUpdated.BookId);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/warehouseBook")]
        public async Task WarehouseBookUpdateAsyncTask_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateWarehouseBookDto = new UpdateWarehouseBookDto()
            {
                Id = 3,
                Quantity = 1,
                WarehouseId = 1,
                BookId = 1
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateWarehouseBookDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouseBook")]
        public async Task WarehouseBookUpdateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var updateWarehouseBookDto = new UpdateWarehouseBookDto()
            {
                Id = 1,
                Quantity = 1,
                WarehouseId = 1,
                BookId = 1
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateWarehouseBookDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouseBook")]
        public async Task WarehouseBookUpdateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateWarehouseBookDto = new UpdateWarehouseBookDto()
            {
                Id = 0,
                Quantity = 0,
                WarehouseId = 0,
                BookId = 0
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateWarehouseBookDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouseBook")]
        public async Task WarehouseBookUpdateAsyncTask_Return_NotFound_404(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateWarehouseBookDto = new UpdateWarehouseBookDto()
            {
                Id = 999999999,
                Quantity = 1,
                WarehouseId = 1,
                BookId = 1
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateWarehouseBookDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouseBook/1")]
        public async Task WarehouseBookDeleteAsyncTask_Return_Ok(string url)
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
        [InlineData("api/warehouseBook/2")]
        public async Task WarehouseBookDeleteAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouseBook/3")]
        public async Task WarehouseBookDeleteAsyncTask_Return_Forbidden_403(string url)
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


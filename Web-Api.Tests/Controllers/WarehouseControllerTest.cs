using ApiBookStore;
using BLL.DTO.Warehouse;
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
using System.ComponentModel.DataAnnotations;

namespace Web_Api.Tests.Controllers
{
    public class WarehouseControllerTest : IClassFixture<WebApplicationFactoryTest<Program>>
    {
        private readonly WebApplicationFactoryTest<Program> _appFactory;
        private readonly TokenServiceTest _tokenJwtService;

        public WarehouseControllerTest(WebApplicationFactoryTest<Program> appFactory)
        {
            _appFactory = appFactory;

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            _tokenJwtService = new TokenServiceTest(config);
        }

        [Theory]
        [InlineData("/api/warehouse")]
        public async Task WarehouseGetAllAsyncTask_Return_Ok(string url)
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
        [InlineData("/api/warehouse/1")]
        [InlineData("/api/warehouse/2")]
        public async Task WarehouseGetByIdAsyncTask_Return_Ok(string url)
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
        [InlineData("api/warehouse")]
        public async Task WarehouseCreateAsyncTask_Return_Created_201(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var createWarehouseDto = new CreateWarehouseDto
            {
                Name = "warehouse-test-name",
                Address = "Test Addres 22, 11",
                City = "Minsk",
                Country = "Belarus",
                PhoneNumber = "+375291111111"
            };

            // Act
            var response = await client.PostAsJsonAsync(url, createWarehouseDto);

            var warehouseString = await response.Content.ReadAsStringAsync();
            var warehouseCreated = JsonConvert.DeserializeObject<Warehouse>(warehouseString)!;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(createWarehouseDto.Name, warehouseCreated.Name);
            Assert.Equal(createWarehouseDto.Address, warehouseCreated.Address);
            Assert.Equal(createWarehouseDto.Country, warehouseCreated.Country);
            Assert.Equal(createWarehouseDto.City, warehouseCreated.City);
            Assert.Equal(createWarehouseDto.PhoneNumber, warehouseCreated.PhoneNumber);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/warehouse")]
        public async Task WarehouseCreateAsyncTask_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            CreateWarehouseDto warehouseDtoNull = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var response = await client.PostAsJsonAsync(url, warehouseDtoNull);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouse")]
        public async Task WarehouseCreateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateWarehouseDto
            {
                Name = "warehouse-test-name",
                Address = "Test Addres 22, 11",
                City = "Minsk",
                Country = "Belarus",
                PhoneNumber = "+375291111111"
            });

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouse")]
        public async Task WarehouseCreateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateWarehouseDto
            {
                Name = string.Empty,
                Address = "Test Addres 22, 11",
                City = string.Empty,
                Country = "Belarus",
                PhoneNumber = "+375291111111"
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouse")]
        public async Task WarehouseCreateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateWarehouseDto
            {
                Name = "warehouse-test-name",
                Address = "Test Addres 22, 11",
                City = "Minsk",
                Country = "Belarus",
                PhoneNumber = "+375291111111"
            });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouse")]
        public async Task WarehouseUpdateAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateWarehouseDto = new UpdateWarehouseDto()
            {
                Id = 1,
                Name = "warehouse-test-name-EDIT",
                Address = "Test Addres 22, 11",
                City = "Minsk",
                Country = "Belarus",
                PhoneNumber = "+375291111111"
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateWarehouseDto);

            var warehouseString = await response.Content.ReadAsStringAsync();
            var warehouseUpdated = JsonConvert.DeserializeObject<Warehouse>(warehouseString);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updateWarehouseDto.Id, warehouseUpdated!.Id);
            Assert.Equal(updateWarehouseDto.Name, warehouseUpdated.Name);
            Assert.Equal(updateWarehouseDto.Address, warehouseUpdated.Address);
            Assert.Equal(updateWarehouseDto.City, warehouseUpdated.City);
            Assert.Equal(updateWarehouseDto.Country, warehouseUpdated.Country);
            Assert.Equal(updateWarehouseDto.PhoneNumber, warehouseUpdated.PhoneNumber);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/warehouse")]
        public async Task WarehouseUpdateAsyncTask_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateWarehouseDto = new UpdateWarehouseDto()
            {
                Id = 1,
                Name = "warehouse-test-name-EDIT",
                Address = "Test Addres 22, 11",
                City = "Minsk",
                Country = "Belarus",
                PhoneNumber = "+375291111111"
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateWarehouseDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouse")]
        public async Task WarehouseUpdateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var updateWarehouseDto = new UpdateWarehouseDto()
            {
                Id = 1,
                Name = "warehouse-test-name-EDIT",
                Address = "Test Addres 22, 11",
                City = "Minsk",
                Country = "Belarus",
                PhoneNumber = "+375291111111"
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateWarehouseDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouse")]
        public async Task WarehouseUpdateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateWarehouseDto = new UpdateWarehouseDto()
            {
                Id = 0,
                Name = string.Empty,
                Address = "Test Addres 22, 11",
                City = "Minsk",
                Country = "Belarus",
                PhoneNumber = "+375291111111"
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateWarehouseDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouse")]
        public async Task WarehouseUpdateAsyncTask_Return_NotFound_404(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateWarehouseDto = new UpdateWarehouseDto()
            {
                Id = 999999999,
                Name = "warehouse-test-name-EDIT",
                Address = "Test Addres 22, 11",
                City = "Minsk",
                Country = "Belarus",
                PhoneNumber = "+375291111111"
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateWarehouseDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouse/1")]
        public async Task WarehouseDeleteAsyncTask_Return_Ok(string url)
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
        [InlineData("api/warehouse/2")]
        public async Task WarehouseDeleteAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/warehouse/3")]
        public async Task WarehouseDeleteAsyncTask_Return_Forbidden_403(string url)
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


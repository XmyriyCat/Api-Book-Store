using ApiBookStore;
using BLL.DTO.Order;
using DLL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using Web_Api.Tests.Extensions;
using Web_Api.Tests.Startup;
using Web_Api.Tests.Startup.JwtHandler;
using Xunit;

namespace Web_Api.Tests.Controllers
{
    public class OrderControllerTest : IClassFixture<WebApplicationFactoryTest<Program>>
    {
        private readonly WebApplicationFactoryTest<Program> _appFactory;
        private readonly TokenServiceTest _tokenJwtService;

        public OrderControllerTest(WebApplicationFactoryTest<Program> appFactory)
        {
            _appFactory = appFactory;

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            _tokenJwtService = new TokenServiceTest(config);
        }

        [Theory]
        [InlineData("/api/order")]
        public async Task OrderGetAllAsyncTask_Return_Ok(string url)
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
        [InlineData("/api/order/4")]
        [InlineData("/api/order/6")]
        public async Task OrderGetByIdAsyncTask_Return_Ok(string url)
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
        [InlineData("api/order")]
        public async Task OrderCreateAsyncTask_Return_Created_201(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var createOrderDto = new CreateOrderDto
            {
                TotalPrice = 100,
                OrderDate = new DateTime(2023, 12, 31, 5, 10, 20),
                ShipmentId = 5,
                CustomerId = 8
            };

            // Act
            var response = await client.PostAsJsonAsync(url, createOrderDto);

            var orderString = await response.Content.ReadAsStringAsync();
            var orderCreated = JsonConvert.DeserializeObject<Order>(orderString)!;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(createOrderDto.TotalPrice, orderCreated.TotalPrice);
            Assert.Equal(createOrderDto.OrderDate, orderCreated.OrderDate);
            Assert.Equal(createOrderDto.ShipmentId, orderCreated.ShipmentId);
            Assert.Equal(createOrderDto.CustomerId, orderCreated.CustomerId);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/order")]
        public async Task OrderCreateAsyncTask_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            CreateOrderDto orderDtoNull = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var response = await client.PostAsJsonAsync(url, orderDtoNull);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/order")]
        public async Task OrderCreateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateOrderDto
            {
                TotalPrice = 100,
                OrderDate = new DateTime(2023, 12, 31, 5, 10, 20),
                ShipmentId = 1,
                CustomerId = 1
            });

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/order")]
        public async Task OrderCreateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateOrderDto
            {
                TotalPrice = 100,
                OrderDate = new DateTime(2023, 12, 31, 5, 10, 20),
                ShipmentId = 0,
                CustomerId = 0
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/order")]
        public async Task OrderCreateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateOrderDto {
                TotalPrice = 100,
                OrderDate = new DateTime(2023, 12, 31, 5, 10, 20),
                ShipmentId = 1,
                CustomerId = 1
            });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/order")]
        public async Task OrderUpdateAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateOrderDto = new UpdateOrderDto()
            {
                Id = 6,
                TotalPrice = 100,
                OrderDate = new DateTime(2023, 12, 31, 5, 10, 20),
                ShipmentId = 5,
                CustomerId = 9
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateOrderDto);

            var orderString = await response.Content.ReadAsStringAsync();
            var orderUpdated = JsonConvert.DeserializeObject<Order>(orderString);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updateOrderDto.Id, orderUpdated!.Id);
            Assert.Equal(updateOrderDto.TotalPrice, orderUpdated.TotalPrice);
            Assert.Equal(updateOrderDto.OrderDate, orderUpdated.OrderDate);
            Assert.Equal(updateOrderDto.ShipmentId, orderUpdated.ShipmentId);
            Assert.Equal(updateOrderDto.CustomerId, orderUpdated.CustomerId);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/order")]
        public async Task OrderUpdateAsyncTask_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateOrderDto = new UpdateOrderDto()
            {
                Id = 3,
                TotalPrice = 100,
                OrderDate = new DateTime(2023, 12, 31, 5, 10, 20),
                ShipmentId = 1,
                CustomerId = 1
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateOrderDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/order")]
        public async Task OrderUpdateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var updateOrderDto = new UpdateOrderDto()
            {
                Id = 3,
                TotalPrice = 100,
                OrderDate = new DateTime(2023, 12, 31, 5, 10, 20),
                ShipmentId = 1,
                CustomerId = 1
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateOrderDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/order")]
        public async Task OrderUpdateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateOrderDto = new UpdateOrderDto()
            {
                Id = 3,
                TotalPrice = 100,
                OrderDate = new DateTime(2023, 12, 31, 5, 10, 20),
                ShipmentId = 0,
                CustomerId = 0
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateOrderDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/order")]
        public async Task OrderUpdateAsyncTask_Return_NotFound_404(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateOrderDto = new UpdateOrderDto()
            {
                Id = 999999999,
                TotalPrice = 100,
                OrderDate = new DateTime(2023, 12, 31, 5, 10, 20),
                ShipmentId = 1,
                CustomerId = 1
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateOrderDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/order/3")]
        public async Task OrderDeleteAsyncTask_Return_Ok(string url)
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
        [InlineData("api/order/2")]
        public async Task OrderDeleteAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/order/4")]
        public async Task OrderDeleteAsyncTask_Return_Forbidden_403(string url)
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

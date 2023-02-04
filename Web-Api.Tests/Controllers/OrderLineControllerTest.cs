using ApiBookStore;
using BLL.DTO.OrderLine;
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
    public class OrderLineControllerTest : IClassFixture<WebApplicationFactoryTest<Program>>
    {
        private readonly WebApplicationFactoryTest<Program> _appFactory;
        private readonly TokenServiceTest _tokenJwtService;

        public OrderLineControllerTest(WebApplicationFactoryTest<Program> appFactory)
        {
            _appFactory = appFactory;

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            _tokenJwtService = new TokenServiceTest(config);
        }

        [Theory]
        [InlineData("/api/orderLine")]
        public async Task OrderLineGetAllAsyncTask_Return_Ok(string url)
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
        [InlineData("/api/orderLine/1")]
        [InlineData("/api/orderLine/2")]
        public async Task OrderLineGetByIdAsyncTask_Return_Ok(string url)
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
        [InlineData("api/orderLine")]
        public async Task OrderLineCreateAsyncTask_Return_Created_201(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var createOrderLineDto = new CreateOrderLineDto
            {
                Quantity= 1,
                OrderId = 1,
                WarehouseBookId = 1
            };

            // Act
            var response = await client.PostAsJsonAsync(url, createOrderLineDto);

            var orderLineString = await response.Content.ReadAsStringAsync();
            var orderLineCreated = JsonConvert.DeserializeObject<OrderLine>(orderLineString)!;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(createOrderLineDto.Quantity, orderLineCreated.Quantity);
            Assert.Equal(createOrderLineDto.OrderId, orderLineCreated.OrderId);
            Assert.Equal(createOrderLineDto.WarehouseBookId, orderLineCreated.WarehouseBookId);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/orderLine")]
        public async Task OrderLineCreateAsyncTask_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            CreateOrderLineDto orderLineDtoNull = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var response = await client.PostAsJsonAsync(url, orderLineDtoNull);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/orderLine")]
        public async Task OrderLineCreateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateOrderLineDto
            {
                Quantity = 1,
                OrderId = 1,
                WarehouseBookId = 1
            });

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/orderLine")]
        public async Task OrderLineCreateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateOrderLineDto
            {
                Quantity = 0,
                OrderId = 0,
                WarehouseBookId = 0
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/orderLine")]
        public async Task OrderLineCreateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateOrderLineDto
            {
                Quantity = 1,
                OrderId = 1,
                WarehouseBookId = 1
            });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/orderLine")]
        public async Task OrderLineUpdateAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateOrderLineDto = new UpdateOrderLineDto()
            {
                Id = 3,
                Quantity = 1,
                OrderId = 1,
                WarehouseBookId = 1
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateOrderLineDto);

            var orderLineString = await response.Content.ReadAsStringAsync();
            var orderLineUpdated = JsonConvert.DeserializeObject<OrderLine>(orderLineString);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updateOrderLineDto.Id, orderLineUpdated!.Id);
            Assert.Equal(updateOrderLineDto.Quantity, orderLineUpdated.Quantity);
            Assert.Equal(updateOrderLineDto.OrderId, orderLineUpdated.OrderId);
            Assert.Equal(updateOrderLineDto.WarehouseBookId, orderLineUpdated.WarehouseBookId);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/orderLine")]
        public async Task OrderLineUpdateAsyncTask_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateOrderLineDto = new UpdateOrderLineDto()
            {
                Id = 3,
                Quantity = 1,
                OrderId = 1,
                WarehouseBookId = 1
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateOrderLineDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/orderLine")]
        public async Task OrderLineUpdateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var updateOrderLineDto = new UpdateOrderLineDto()
            {
                Id = 3,
                Quantity = 1,
                OrderId = 1,
                WarehouseBookId = 1
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateOrderLineDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/orderLine")]
        public async Task OrderLineUpdateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateOrderLineDto = new UpdateOrderLineDto()
            {
                Id = 0,
                Quantity = 0,
                OrderId = 0,
                WarehouseBookId = 0
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateOrderLineDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/orderLine")]
        public async Task OrderLineUpdateAsyncTask_Return_NotFound_404(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateOrderLineDto = new UpdateOrderLineDto()
            {
                Id = 999999999,
                Quantity = 1,
                OrderId = 1,
                WarehouseBookId = 1
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateOrderLineDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/orderLine/3")]
        public async Task OrderLineDeleteAsyncTask_Return_Ok(string url)
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
        [InlineData("api/orderLine/2")]
        public async Task OrderLineDeleteAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/orderLine/4")]
        public async Task OrderLineDeleteAsyncTask_Return_Forbidden_403(string url)
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

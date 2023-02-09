using ApiBookStore;
using BLL.DTO.Delivery;
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
    public class DeliveryControllerTest : IClassFixture<WebApplicationFactoryTest<Program>>
    {
        private readonly WebApplicationFactoryTest<Program> _appFactory;
        private readonly TokenServiceTest _tokenJwtService;

        public DeliveryControllerTest(WebApplicationFactoryTest<Program> appFactory)
        {
            _appFactory = appFactory;

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            _tokenJwtService = new TokenServiceTest(config);
        }

        [Theory]
        [InlineData("/api/delivery")]
        public async Task DeliveryGetAllAsyncTask_Return_Ok(string url)
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
        [InlineData("/api/delivery/1")]
        [InlineData("/api/delivery/2")]
        public async Task DeliveryGetByIdAsyncTask_Return_Ok(string url)
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
        [InlineData("api/delivery")]
        public async Task DeliveryCreateAsyncTask_Return_Created_201(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var createDeliveryDto = new CreateDeliveryDto
            {
                Price = 100,
                Name = "Test-Delivery-name"
            };

            // Act
            var response = await client.PostAsJsonAsync(url, createDeliveryDto);

            var deliveryString = await response.Content.ReadAsStringAsync();
            var deliveryCreated = JsonConvert.DeserializeObject<Delivery>(deliveryString)!;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(createDeliveryDto.Price, deliveryCreated.Price);
            Assert.Equal(createDeliveryDto.Name, deliveryCreated.Name);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/delivery")]
        public async Task DeliveryCreateAsyncTask_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            CreateDeliveryDto deliveryDtoNull = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var response = await client.PostAsJsonAsync(url, deliveryDtoNull);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/delivery")]
        public async Task DeliveryCreateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateDeliveryDto
            {
                Price = 100,
                Name = "Test-delivery-name",
            });

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/delivery")]
        public async Task DeliveryCreateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateDeliveryDto
            {
                Price = 100,
                Name = string.Empty,
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/delivery")]
        public async Task DeliveryCreateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateDeliveryDto
            {
                Price = 100,
                Name = "Test-delivery-name",
            });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/delivery")]
        public async Task DeliveryUpdateAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateDeliveryDto = new UpdateDeliveryDto()
            {
                Id = 3,
                Price = 100,
                Name = "Test-delivery-name",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateDeliveryDto);

            var deliveryString = await response.Content.ReadAsStringAsync();
            var deliveryUpdated = JsonConvert.DeserializeObject<Delivery>(deliveryString);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updateDeliveryDto.Id, deliveryUpdated!.Id);
            Assert.Equal(updateDeliveryDto.Price, deliveryUpdated.Price);
            Assert.Equal(updateDeliveryDto.Name, deliveryUpdated.Name);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/delivery")]
        public async Task DeliveryUpdateAsyncTask_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateDeliveryDto = new UpdateDeliveryDto()
            {
                Id = 1,
                Price = 100,
                Name = "Test-delivery-name-EDIT",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateDeliveryDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/delivery")]
        public async Task DeliveryUpdateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var updateDeliveryDto = new UpdateDeliveryDto()
            {
                Id = 1,
                Price = 100,
                Name = "Test-delivery-name-EDIT",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateDeliveryDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/delivery")]
        public async Task DeliveryUpdateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateDeliveryDto = new UpdateDeliveryDto()
            {
                Id = 1,
                Price = 100,
                Name = string.Empty,
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateDeliveryDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/delivery")]
        public async Task DeliveryUpdateAsyncTask_Return_NotFound_404(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateDeliveryDto = new UpdateDeliveryDto()
            {
                Id = 999999999,
                Price = 100,
                Name = "Test-delivery-name",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateDeliveryDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/delivery/3")]
        public async Task DeliveryDeleteAsyncTask_Return_Ok(string url)
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
        [InlineData("api/delivery/2")]
        public async Task DeliveryDeleteAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/delivery/4")]
        public async Task DeliveryDeleteAsyncTask_Return_Forbidden_403(string url)
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

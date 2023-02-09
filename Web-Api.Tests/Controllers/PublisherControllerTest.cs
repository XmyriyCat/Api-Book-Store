using ApiBookStore;
using BLL.DTO.Publisher;
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
    public class PublisherControllerTest : IClassFixture<WebApplicationFactoryTest<Program>>
    {
        private readonly WebApplicationFactoryTest<Program> _appFactory;
        private readonly TokenServiceTest _tokenJwtService;

        public PublisherControllerTest(WebApplicationFactoryTest<Program> appFactory)
        {
            _appFactory = appFactory;

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            _tokenJwtService = new TokenServiceTest(config);
        }

        [Theory]
        [InlineData("/api/publisher")]
        public async Task PublisherGetAllAsyncTask_Return_Ok(string url)
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
        [InlineData("/api/publisher/1")]
        [InlineData("/api/publisher/2")]
        public async Task PublisherGetByIdAsyncTask_Return_Ok(string url)
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
        [InlineData("api/publisher")]
        public async Task PublisherCreateAsyncTask_Return_Created_201(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var createPublisherDto = new CreatePublisherDto
            {
                Name = "Test-publisher-name"
            };

            // Act
            var response = await client.PostAsJsonAsync(url, createPublisherDto);

            var publisherString = await response.Content.ReadAsStringAsync();
            var publisherCreated = JsonConvert.DeserializeObject<Publisher>(publisherString)!;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(createPublisherDto.Name, publisherCreated.Name);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/publisher")]
        public async Task PublisherCreateAsyncTask_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            CreatePublisherDto publisherDtoNull = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var response = await client.PostAsJsonAsync(url, publisherDtoNull);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/publisher")]
        public async Task PublisherCreateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreatePublisherDto
            {
                Name = "Test-publisher-name",
            });

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/publisher")]
        public async Task PublisherCreateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreatePublisherDto
            {
                Name = string.Empty,
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/publisher")]
        public async Task PublisherCreateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync(url, new CreatePublisherDto
            {
                Name = "Test-publisher-name",
            });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/publisher")]
        public async Task PublisherUpdateAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updatePublisherDto = new UpdatePublisherDto()
            {
                Id = 2,
                Name = "Test-publisher-name-EDIT",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updatePublisherDto);

            var publisherString = await response.Content.ReadAsStringAsync();
            var publisherUpdated = JsonConvert.DeserializeObject<Publisher>(publisherString);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updatePublisherDto.Id, publisherUpdated!.Id);
            Assert.Equal(updatePublisherDto.Name, publisherUpdated.Name);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/publisher")]
        public async Task PublisherUpdateAsyncTask_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updatePublisherDto = new UpdatePublisherDto()
            {
                Id = 3,
                Name = "Test-publisher-name-EDIT",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updatePublisherDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/publisher")]
        public async Task PublisherUpdateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var updatePublisherDto = new UpdatePublisherDto()
            {
                Id = 1,
                Name = "Test-publisher-name-EDIT",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updatePublisherDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/publisher")]
        public async Task PublisherUpdateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updatePublisherDto = new UpdatePublisherDto()
            {
                Id = 1,
                Name = string.Empty,
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updatePublisherDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/publisher")]
        public async Task PublisherUpdateAsyncTask_Return_NotFound_404(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updatePublisherDto = new UpdatePublisherDto()
            {
                Id = 999999999,
                Name = "Test-publisher-name",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updatePublisherDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/publisher/3")]
        public async Task PublisherDeleteAsyncTask_Return_Ok(string url)
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
        [InlineData("api/publisher/2")]
        public async Task PublisherDeleteAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/publisher/2")]
        public async Task PublisherDeleteAsyncTask_Return_Forbidden_403(string url)
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

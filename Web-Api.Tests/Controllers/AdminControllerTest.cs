using ApiBookStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using DLL.Models;
using Newtonsoft.Json;
using Web_Api.Tests.Extensions;
using Web_Api.Tests.Startup;
using Web_Api.Tests.Startup.JwtHandler;
using Xunit;
using BLL.DTO.User;
using Bogus;

namespace Web_Api.Tests.Controllers
{
    public class AdminControllerTest : IClassFixture<WebApplicationFactoryTest<Program>>
    {
        private readonly WebApplicationFactoryTest<Program> _appFactory;
        private readonly TokenServiceTest _tokenJwtService;

        public AdminControllerTest(WebApplicationFactoryTest<Program> appFactory)
        {
            _appFactory = appFactory;

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            _tokenJwtService = new TokenServiceTest(config);
        }

        [Theory]
        [InlineData("/api/admin")]
        public async Task GetAllAsyncTask_Return_Unauthorized_Anonymous_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin")]
        public async Task GetAllAsyncTask_Return_Forbidden_Buyer_Role_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin")]
        public async Task GetAllAsyncTask_Return_Forbidden_Manager_Role_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin")]
        public async Task GetAllAsyncTask_Return_Ok_200(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenAdminRole("Admin-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.GetAsync(url);

            var usersString = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(usersString)!;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(users);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/admin/count")]
        public async Task GetCountAsync_Return_Ok_200(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenAdminRole("Admin-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.GetAsync(url);

            var usersCountString = await response.Content.ReadAsStringAsync();
            var count = JsonConvert.DeserializeObject<int>(usersCountString)!;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(count >= 0);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/admin/1")]
        [InlineData("/api/admin/2")]
        public async Task GetByIdAsyncTask_Return_Ok_200(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenAdminRole("Admin-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.GetAsync(url);

            var userString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(userString)!;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(user);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/admin/1")]
        [InlineData("/api/admin/2")]
        public async Task GetByIdAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin/1")]
        [InlineData("/api/admin/2")]
        public async Task GetByIdAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin")]
        public async Task CreateUserAsync_Return_Created_201(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenAdminRole("Admin-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var faker = new Faker<CreateUserDto>()
                .RuleFor(x => x.Country, f => f.Address.Country())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Address, f => f.Address.FullAddress())
                .RuleFor(x => x.Username, f => f.Name.FullName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Login, f => f.Random.String2(10, 50))
                .RuleFor(x => x.Password, f => f.Random.String2(10, 50))
                .RuleFor(x => x.RolesIds, new List<int> { 1 });

            var createUserDto = faker.Generate();

            // Act
            var response = await client.PostAsJsonAsync(url, createUserDto);

            var userString = await response.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<User>(userString)!;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(createdUser);
            Assert.Equal(createUserDto.Country, createdUser.Country);
            Assert.Equal(createUserDto.City, createdUser.City);
            Assert.Equal(createUserDto.Address, createdUser.Address);
            Assert.Equal(createUserDto.Username, createdUser.UserName);
            Assert.Equal(createUserDto.Email, createdUser.Email);
            Assert.Equal(createUserDto.Login, createdUser.Login);
            Assert.Equal(createUserDto.RolesIds.Count, createdUser.Roles.Count);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/admin")]
        public async Task CreateUserAsync_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenManagerRole("Manager-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var faker = new Faker<CreateUserDto>()
                .RuleFor(x => x.Country, f => f.Address.Country())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Address, f => f.Address.FullAddress())
                .RuleFor(x => x.Username, f => f.Name.FullName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Login, f => f.Random.String2(10, 50))
                .RuleFor(x => x.Password, f => f.Random.String2(10, 50))
                .RuleFor(x => x.RolesIds, new List<int> { 1, 2, 3 });

            var createUserDto = faker.Generate();

            // Act
            var response = await client.PostAsJsonAsync(url, createUserDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin")]
        public async Task CreateUserAsync_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var faker = new Faker<CreateUserDto>()
                .RuleFor(x => x.Country, f => f.Address.Country())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Address, f => f.Address.FullAddress())
                .RuleFor(x => x.Username, f => f.Name.FullName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Login, f => f.Random.String2(10, 50))
                .RuleFor(x => x.Password, f => f.Random.String2(10, 50))
                .RuleFor(x => x.RolesIds, new List<int> { 1, 2, 3 });

            var createUserDto = faker.Generate();

            // Act
            var response = await client.PostAsJsonAsync(url, createUserDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin")]
        public async Task CreateUserAsync_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenAdminRole("Admin-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            CreateUserDto createUserDto = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var response = await client.PostAsJsonAsync(url, createUserDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin")]
        public async Task UpdateUserAsync_Return_Ok_200(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenAdminRole("Admin-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var faker = new Faker<UpdateUserDto>()
                .RuleFor(x => x.Id, 1)
                .RuleFor(x => x.Country, f => f.Address.Country())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Address, f => f.Address.FullAddress())
                .RuleFor(x => x.Username, f => f.Name.FullName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Login, f => f.Random.String2(10, 50))
                .RuleFor(x => x.OrderIds, new List<int> { 1, 2, 3 })
                .RuleFor(x => x.RolesIds, new List<int> { 1, 2, 3 });

            var updateUserDto = faker.Generate();

            // Act
            var response = await client.PutAsJsonAsync(url, updateUserDto);

            var userString = await response.Content.ReadAsStringAsync();
            var updatedUser = JsonConvert.DeserializeObject<User>(userString)!;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(updatedUser);
            Assert.Equal(updateUserDto.Id, updatedUser.Id);
            Assert.Equal(updateUserDto.Country, updatedUser.Country);
            Assert.Equal(updateUserDto.City, updatedUser.City);
            Assert.Equal(updateUserDto.Address, updatedUser.Address);
            Assert.Equal(updateUserDto.Username, updatedUser.UserName);
            Assert.Equal(updateUserDto.Email, updatedUser.Email);
            Assert.Equal(updateUserDto.Login, updatedUser.Login);
            Assert.Equal(updateUserDto.RolesIds.Count, updatedUser.Roles.Count);
            Assert.Equal(updateUserDto.OrderIds.Count, updatedUser.Orders.Count);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/admin")]
        public async Task UpdateUserAsync_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var faker = new Faker<UpdateUserDto>()
                .RuleFor(x => x.Id, f => f.Random.Int(1, 10))
                .RuleFor(x => x.Country, f => f.Address.Country())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Address, f => f.Address.FullAddress())
                .RuleFor(x => x.Username, f => f.Name.FullName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Login, f => f.Random.String2(10, 50))
                .RuleFor(x => x.OrderIds, new List<int> { 1, 2, 3 })
                .RuleFor(x => x.RolesIds, new List<int> { 1, 2, 3 });

            var updateUserDto = faker.Generate();

            // Act
            var response = await client.PutAsJsonAsync(url, updateUserDto);
            
            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin")]
        public async Task UpdateUserAsync_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();
            
            var faker = new Faker<UpdateUserDto>()
                .RuleFor(x => x.Id, f => f.Random.Int(1, 10))
                .RuleFor(x => x.Country, f => f.Address.Country())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Address, f => f.Address.FullAddress())
                .RuleFor(x => x.Username, f => f.Name.FullName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Login, f => f.Random.String2(10, 50))
                .RuleFor(x => x.OrderIds, new List<int> { 1, 2, 3 })
                .RuleFor(x => x.RolesIds, new List<int> { 1, 2, 3 });

            var updateUserDto = faker.Generate();

            // Act
            var response = await client.PutAsJsonAsync(url, updateUserDto);
            
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin")]
        public async Task UpdateUserAsync_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();
            
            var tokenJwt = _tokenJwtService.CreateTokenAdminRole("Admin-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization
            
            UpdateUserDto updateUserDto = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var response = await client.PutAsJsonAsync(url, updateUserDto);
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin/1")]
        [InlineData("/api/admin/2")]
        public async Task DeleteByIdAsyncTask_Return_Ok_200(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();
            
            var tokenJwt = _tokenJwtService.CreateTokenAdminRole("Admin-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization
            
            // Act
            var response = await client.DeleteAsync(url);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin/1")]
        [InlineData("/api/admin/2")]
        public async Task DeleteByIdAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();
            
            var tokenJwt = _tokenJwtService.CreateTokenBuyerRole("Buyer-user");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization
            
            // Act
            var response = await client.DeleteAsync(url);
            
            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/admin/1")]
        [InlineData("/api/admin/2")]
        public async Task DeleteByIdAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();
            
            // Act
            var response = await client.DeleteAsync(url);
            
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}

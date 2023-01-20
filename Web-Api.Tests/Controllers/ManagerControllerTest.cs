using System.Net;
using System.Net.Http.Json;
using ApiBookStore;
using BLL.DTO.Author;
using BLL.DTO.Genre;
using BLL.DTO.Publisher;
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
    public class ManagerControllerTest : IClassFixture<WebApplicationFactoryTest<Program>>
    {
        private readonly WebApplicationFactoryTest<Program> _appFactory;

        public ManagerControllerTest(WebApplicationFactoryTest<Program> appFactory)
        {
            _appFactory = appFactory;
        }

        [Theory]
        [InlineData("/api/manager/author")]
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
        [InlineData("/api/manager/genre")]
        public async Task GenreGetAllAsyncTask_Return_Ok(string url)
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
        [InlineData("/api/manager/publisher")]
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
        [InlineData("/api/manager/author/1")]
        [InlineData("/api/manager/author/2")]
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
        [InlineData("/api/manager/genre/1")]
        [InlineData("/api/manager/genre/2")]
        public async Task GenreGetByIdAsyncTask_Return_Ok(string url)
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
        [InlineData("/api/manager/publisher/1")]
        [InlineData("/api/manager/publisher/2")]
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
        [InlineData("api/manager/author")]
        public async Task AuthorCreateAsyncTask_Return_Created_201(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-1");

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
        [InlineData("api/manager/genre")]
        public async Task GenreCreateAsyncTask_Return_Created_201(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-1");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var createGenreDto = new CreateGenreDto
            {
                Name = "Test-genre-name"
            };

            // Act
            var response = await client.PostAsJsonAsync(url, createGenreDto);

            var genreString = await response.Content.ReadAsStringAsync();
            var genreCreated = JsonConvert.DeserializeObject<Genre>(genreString)!;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(createGenreDto.Name, genreCreated.Name);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/manager/publisher")]
        public async Task PublisherCreateAsyncTask_Return_Created_201(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-1");

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
        [InlineData("api/manager/author")]
        public async Task AuthorCreateAsyncTask_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-2");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            CreateAuthorDto authorDtoNull = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var response = await client.PostAsJsonAsync(url, authorDtoNull);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/genre")]
        public async Task GenreCreateAsyncTask_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-2");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            CreateGenreDto genreDtoNull = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var response = await client.PostAsJsonAsync(url, genreDtoNull);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/publisher")]
        public async Task PublisherCreateAsyncTask_Return_BadRequest_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-2");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            CreatePublisherDto publisherDtoNull = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var response = await client.PostAsJsonAsync(url, publisherDtoNull);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/author")]
        public async Task AuthorCreateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenBuyerRole("Manager-3");

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
        [InlineData("api/manager/genre")]
        public async Task GenreCreateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenBuyerRole("Manager-3");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateGenreDto
            {
                Name = "Test-genre-name",
            });

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/publisher")]
        public async Task PublisherCreateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenBuyerRole("Manager-3");

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
        [InlineData("api/manager/author")]
        public async Task AuthorCreateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-4");

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
        [InlineData("api/manager/genre")]
        public async Task GenreCreateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-4");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateGenreDto
            {
                Name = string.Empty,
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/publisher")]
        public async Task PublisherCreateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-4");

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
        [InlineData("api/manager/author")]
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
        [InlineData("api/manager/genre")]
        public async Task GenreCreateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync(url, new CreateGenreDto
            {
                Name = "Test-genre-name",
            });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/publisher")]
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
        [InlineData("api/manager/author")]
        public async Task AuthorUpdateAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-5");

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
        [InlineData("api/manager/genre")]
        public async Task GenreUpdateAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-5");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateGenreDto = new UpdateGenreDto()
            {
                Id = 1,
                Name = "Test-genre-name-EDIT",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateGenreDto);

            var genreString = await response.Content.ReadAsStringAsync();
            var genreUpdated = JsonConvert.DeserializeObject<Genre>(genreString);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updateGenreDto.Id, genreUpdated!.Id);
            Assert.Equal(updateGenreDto.Name, genreUpdated.Name);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("api/manager/publisher")]
        public async Task PublisherUpdateAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-5");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updatePublisherDto = new UpdatePublisherDto()
            {
                Id = 1,
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
        [InlineData("api/manager/author")]
        public async Task AuthorUpdateAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenBuyerRole("Manager-6");

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
        [InlineData("api/manager/genre")]
        public async Task GenreUpdateAsyncTask_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenBuyerRole("Manager-6");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateGenreDto = new UpdateGenreDto()
            {
                Id = 1,
                Name = "Test-genre-name-EDIT",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateGenreDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/publisher")]
        public async Task PublisherUpdateAsyncTask_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenBuyerRole("Manager-6");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updatePublisherDto = new UpdatePublisherDto()
            {
                Id = 1,
                Name = "Test-ublisher-name-EDIT",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updatePublisherDto);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }


        [Theory]
        [InlineData("api/manager/author")]
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
        [InlineData("api/manager/genre")]
        public async Task GenreUpdateAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            var updateGenreDto = new UpdateGenreDto()
            {
                Id = 1,
                Name = "Test-genre-name-EDIT",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateGenreDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/publisher")]
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
        [InlineData("api/manager/author")]
        public async Task AuthorUpdateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-7");

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
        [InlineData("api/manager/genre")]
        public async Task GenreUpdateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-7");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateGenreDto = new UpdateGenreDto()
            {
                Id = 1,
                Name = string.Empty,
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateGenreDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/publisher")]
        public async Task PublisherUpdateAsyncTask_Return_ValidationError_400(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-7");

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
        [InlineData("api/manager/author")]
        public async Task AuthorUpdateAsyncTask_Return_NotFound_404(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-8");

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
        [InlineData("api/manager/genre")]
        public async Task GenreUpdateAsyncTask_Return_NotFound_404(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-8");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            var updateGenreDto = new UpdateGenreDto()
            {
                Id = 999999999,
                Name = "Test-genre-name",
            };

            // Act
            var response = await client.PutAsJsonAsync(url, updateGenreDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/publisher")]
        public async Task PublisherUpdateAsyncTask_Return_NotFound_404(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-8");

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
        [InlineData("api/manager/author/3")]
        public async Task AuthorDeleteAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-9");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/genre/2")]
        public async Task GenreDeleteAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-9");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/publisher/3")]
        public async Task PublisherDeleteAsyncTask_Return_Ok(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenManagerRole("Manager-9");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/author/3")]
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
        [InlineData("api/manager/genre/2")]
        public async Task GenreDeleteAsyncTask_Return_Unauthorized_401(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/publisher/2")]
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
        [InlineData("api/manager/author/2")]
        public async Task AuthorDeleteAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenBuyerRole("Manager-10");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/genre/2")]
        public async Task GenreDeleteAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenBuyerRole("Manager-10");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("api/manager/publisher/2")]
        public async Task PublisherDeleteAsyncTask_Return_Forbidden_403(string url)
        {
            // Arrange
            var client = _appFactory.CreateClient();

            using var configScope = _appFactory.Services.CreateScope();
            var config = configScope.ServiceProvider.GetRequiredService<IConfiguration>();

            var tokenJwtService = new TokenServiceTest(config);
            var tokenJwt = tokenJwtService.CreateTokenBuyerRole("Manager-10");

            client.AddJwtToken(tokenJwt); // Add HTML header-request Authorization

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }

}

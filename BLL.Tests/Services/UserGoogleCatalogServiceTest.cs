using AutoMapper;
using BLL.DTO.User;
using BLL.Errors;
using BLL.Infrastructure.Mapper;
using BLL.Services.Contract;
using BLL.Services.Implementation;
using BLL.Tests.Infrastructure;
using BLL.Tests.Infrastructure.Mock;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Web_Api.Tests.Startup.DbSettings;
using Xunit;

namespace BLL.Tests.Services
{
    public class UserGoogleCatalogServiceTest
    {
        private const string SecretJwtKey = "ThWmZq4t7w!z%C&F)J@NcRfUjXn2r5u8";

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserGoogleCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();

            // Create config InMemory for testing
            var inMemorySettings = new Dictionary<string, string> {
                {"JwtToken:Key", SecretJwtKey}
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var tokenService = new TokenJwtService(config);

            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [Fact]
        public async Task RegisterGoogleAsync_Return_Ok()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 800,
                    Login = "test-login",
                    Email = "test_email@example.com"
                }
            };

            var usersExpectedSize = users.Count + 1;

            var registrationGoogleUserDto = new RegistrationGoogleUserDto
            {
                GoogleToken = "some token",
                Password = "some password"
            };

            var googleExpectedPayload = new GoogleJsonWebSignature.Payload
            {
                Email = "test_email@gmail.com",
                EmailVerified = true,
                Name = "test username"
            };

            var googleTokenService = GoogleTokenServiceMock.MockGoogleTokenService(googleExpectedPayload);
            var userManager = UserManagerMock.MockUserManagerSuccess(users);

            var userGoogleCatalogService = new UserGoogleCatalogService(_repositoryWrapper, _mapper, googleTokenService.Object, userManager.Object, _tokenService);

            // Act
            var result = await userGoogleCatalogService.RegisterGoogleAsync(registrationGoogleUserDto);

            // Assert
            Assert.Equal(usersExpectedSize, users.Count);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Username);
            Assert.NotEmpty(result.JwtToken);
        }

        [Fact]
        public async Task RegisterGoogleAsync_Return_InvalidUserLoginError()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 800,
                    Login = "test-login",
                    Email = "test_email@example.com"
                }
            };

            var usersExpectedSize = users.Count;

            var googleExpectedPayload = new GoogleJsonWebSignature.Payload
            {
                Email = "test-login", // This login is already created in StartUp RepositoryWrapper
                EmailVerified = true,
                Name = "test username"
            };

            var registrationGoogleUserDto = new RegistrationGoogleUserDto
            {
                GoogleToken = "some token",
                Password = "some password"
            };

            var googleTokenService = GoogleTokenServiceMock.MockGoogleTokenService(googleExpectedPayload);
            var userManager = UserManagerMock.MockUserManagerSuccess(users);

            var userGoogleCatalogService = new UserGoogleCatalogService(_repositoryWrapper, _mapper, googleTokenService.Object, userManager.Object, _tokenService);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidUserLoginError>(() => userGoogleCatalogService.RegisterGoogleAsync(registrationGoogleUserDto));
            Assert.Equal(usersExpectedSize, users.Count);
        }

        [Fact]
        public async Task RegisterGoogleAsync_Return_CreateIdentityUserException()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 800,
                    Login = "test-login",
                    Email = "test_email@example.com"
                }
            };

            var usersExpectedSize = users.Count;

            var googleExpectedPayload = new GoogleJsonWebSignature.Payload
            {
                Email = "example_email@gmail.com",
                EmailVerified = true,
                Name = "test username"
            };

            var registrationGoogleUserDto = new RegistrationGoogleUserDto
            {
                GoogleToken = "some token",
                Password = "some password"
            };

            var googleTokenService = GoogleTokenServiceMock.MockGoogleTokenService(googleExpectedPayload);
            var userManager = UserManagerMock.MockUserManagerFailure(users);

            var userGoogleCatalogService = new UserGoogleCatalogService(_repositoryWrapper, _mapper, googleTokenService.Object, userManager.Object, _tokenService);

            // Act & Assert
            await Assert.ThrowsAsync<CreateIdentityUserException>(() => userGoogleCatalogService.RegisterGoogleAsync(registrationGoogleUserDto));
            Assert.Equal(usersExpectedSize, users.Count);
        }

        [Fact]
        public async Task LoginGoogleAsync_Return_Ok()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 800,
                    Login = "test-login",
                    Email = "test_email@example.com"
                }
            };

            var usersExpectedSize = users.Count;

            var googleExpectedPayload = new GoogleJsonWebSignature.Payload
            {
                Email = "test-login",
                EmailVerified = true,
                Name = "test username"
            };

            var loginGoogleUserDto = new LoginGoogleUserDto()
            {
                GoogleToken = "some token"
            };

            var googleTokenService = GoogleTokenServiceMock.MockGoogleTokenService(googleExpectedPayload);
            var userManager = UserManagerMock.MockUserManagerFailure(users);

            var userGoogleCatalogService = new UserGoogleCatalogService(_repositoryWrapper, _mapper, googleTokenService.Object, userManager.Object, _tokenService);

            // Act
            var result = await userGoogleCatalogService.LoginGoogleAsync(loginGoogleUserDto);

            // Assert
            Assert.Equal(usersExpectedSize, users.Count);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Username);
            Assert.NotEmpty(result.JwtToken);
        }

        [Fact]
        public async Task LoginGoogleAsync_Return_UserLoginIsNotFound()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 800,
                    Login = "test-login",
                    Email = "test_email@example.com"
                }
            };

            var usersExpectedSize = users.Count;

            var googleExpectedPayload = new GoogleJsonWebSignature.Payload
            {
                Email = "test_email@gmail.com",
                EmailVerified = true,
                Name = "test username"
            };

            var loginGoogleUserDto = new LoginGoogleUserDto()
            {
                GoogleToken = "some token"
            };

            var googleTokenService = GoogleTokenServiceMock.MockGoogleTokenService(googleExpectedPayload);
            var userManager = UserManagerMock.MockUserManagerFailure(users);

            var userGoogleCatalogService = new UserGoogleCatalogService(_repositoryWrapper, _mapper, googleTokenService.Object, userManager.Object, _tokenService);

            // Act & Assert
            await Assert.ThrowsAsync<UserLoginIsNotFound>(() => userGoogleCatalogService.LoginGoogleAsync(loginGoogleUserDto));
            Assert.Equal(usersExpectedSize, users.Count);
        }
    }
}

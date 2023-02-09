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
using Microsoft.Extensions.Configuration;
using Web_Api.Tests.Startup.DbSettings;
using Xunit;

namespace BLL.Tests.Services
{
    public class UserCatalogServiceTest
    {
        private const string SecretJwtKey = "ThWmZq4t7w!z%C&F)J@NcRfUjXn2r5u8";
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserCatalogServiceTest()
        {
            // Create DbContext InMemory for testing
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();

            // Create config InMemory for testing
            var inMemorySettings = new Dictionary<string, string> {
                {"JwtToken:Key", SecretJwtKey},
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
        public async Task RegisterAsync_Return_Ok()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 800,
                    Login = "test-login",
                    Email = "test_email@example.com"
                },
                new User
                {
                    Id = 801,
                    Login = "test-login-2",
                    Email = "test_email-2@example.com"
                }
            };
            var usersExpectedSize = users.Count + 1;

            // Mock UserManager<TUser> for testing 
            var userManagerMock = UserManagerMock.MockUserManagerSuccess(users);
            var userCatalogService = new UserCatalogService(_repositoryWrapper, _mapper, _tokenService, userManagerMock.Object);

            var registrationUserDto = new RegistrationUserDto
            {
                Login = "new-login-for-testing",
                Password = "789651s3d2f1sd68f451",
                Username = "test-username-for-testing"
            };

            // Act
            var result = await userCatalogService.RegisterAsync(registrationUserDto);

            // Assert
            Assert.Equal(usersExpectedSize, users.Count);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Username);
            Assert.NotEmpty(result.JwtToken);
        }

        [Fact]
        public async Task RegisterAsync_Return_InvalidUserLoginError()
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

            // Mock UserManager<TUser> for testing 
            var userManagerMock = UserManagerMock.MockUserManagerSuccess(users);
            var userCatalogService = new UserCatalogService(_repositoryWrapper, _mapper, _tokenService, userManagerMock.Object);

            var registrationUserDto = new RegistrationUserDto
            {
                Login = "test-login",
                Password = "789651s3d2f1sd68f451",
                Username = "test-username-for-testing"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidUserLoginError>(() => userCatalogService.RegisterAsync(registrationUserDto));
            Assert.Equal(usersExpectedSize, users.Count);
        }

        [Fact]
        public async Task RegisterAsync_Return_CreateIdentityUserException()
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

            // Mock UserManager<TUser> for testing 
            var userManagerMock = UserManagerMock.MockUserManagerFailure(users);
            var userCatalogService = new UserCatalogService(_repositoryWrapper, _mapper, _tokenService, userManagerMock.Object);

            var registrationUserDto = new RegistrationUserDto
            {
                Login = "test-login",
                Password = "789651s3d2f1sd68f451",
                Username = "test-username-for-testing"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidUserLoginError>(() => userCatalogService.RegisterAsync(registrationUserDto));
            Assert.Equal(usersExpectedSize, users.Count);
        }

        [Fact]
        public async Task LoginAsync_Return_Ok()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 800,
                    Login = "test-login",
                    Email = "test_email@example.com"
                },
                new User
                {
                    Id = 801,
                    Login = "test-login-2",
                    Email = "test_email-2@example.com"
                }
            };
            var usersExpectedSize = users.Count;

            // Mock UserManager<TUser> for testing 
            var userManagerMock = UserManagerMock.MockUserManagerSuccess(users);
            var userCatalogService = new UserCatalogService(_repositoryWrapper, _mapper, _tokenService, userManagerMock.Object);

            var loginUserDto = new LoginUserDto()
            {
                Login = "test-login", // This login is already created in StartUp RepositoryWrapper
                Password = "789651s3d2f1sd68f451"
            };

            // Act
            var result = await userCatalogService.LoginAsync(loginUserDto);

            // Assert
            Assert.Equal(usersExpectedSize, users.Count);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Username);
            Assert.NotEmpty(result.JwtToken);
        }

        [Fact]
        public async Task LoginAsync_Return_UserLoginIsNotFound()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 800,
                    Login = "test-login",
                    Email = "test_email@example.com"
                },
                new User
                {
                    Id = 801,
                    Login = "test-login-2",
                    Email = "test_email-2@example.com"
                }
            };
            var usersExpectedSize = users.Count;

            // Mock UserManager<TUser> for testing 
            var userManagerMock = UserManagerMock.MockUserManagerSuccess(users);
            var userCatalogService = new UserCatalogService(_repositoryWrapper, _mapper, _tokenService, userManagerMock.Object);

            var loginUserDto = new LoginUserDto()
            {
                Login = "non-created-user-login",
                Password = "789651s3d2f1sd68f451"
            };

            // Act & Assert
            await Assert.ThrowsAsync<UserLoginIsNotFound>(() => userCatalogService.LoginAsync(loginUserDto));
            Assert.Equal(usersExpectedSize, users.Count);
        }

        [Fact]
        public async Task LoginAsync_Return_WrongUserPasswordError()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 800,
                    Login = "test-login",
                    Email = "test_email@example.com"
                },
                new User
                {
                    Id = 801,
                    Login = "test-login-2",
                    Email = "test_email-2@example.com"
                }
            };
            var usersExpectedSize = users.Count;

            // Mock UserManager<TUser> for testing 
            var userManagerMock = UserManagerMock.MockUserManagerFailure(users);
            var userCatalogService = new UserCatalogService(_repositoryWrapper, _mapper, _tokenService, userManagerMock.Object);

            var loginUserDto = new LoginUserDto()
            {
                Login = "test-login",  // This login is already created in StartUp RepositoryWrapper
                Password = ""
            };

            // Act & Assert
            await Assert.ThrowsAsync<WrongUserPasswordError>(() => userCatalogService.LoginAsync(loginUserDto));
            Assert.Equal(usersExpectedSize, users.Count);
        }
    }
}

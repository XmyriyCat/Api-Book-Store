using AutoMapper;
using BLL.DTO.User;
using BLL.Errors;
using BLL.Infrastructure.Mapper;
using BLL.Services.Implementation;
using BLL.Tests.Infrastructure;
using BLL.Tests.Infrastructure.Mock;
using DLL.Errors;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentAssertions;
using Web_Api.Tests.Startup.DbSettings;
using Xunit;

namespace BLL.Tests.Services
{
    public class AdminCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public AdminCatalogServiceTest()
        {
            // Create DbContext InMemory for testing
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            var mapper = mapperConfiguration.CreateMapper();

            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arranges
            var usersAll = await _repositoryWrapper.Users.GetAllIncludeAsync();

            var userManagerMock = UserManagerMock.MockUserManagerSuccess(new List<User>());
            var adminCatalogService = new AdminCatalogService(_repositoryWrapper, _mapper, userManagerMock.Object);

            // Act
            var usersDb = await adminCatalogService.GetAllAsync();

            // Assert
            Assert.NotNull(usersDb);
            usersAll.Should().BeEquivalentTo(usersDb, options => options
                .Excluding(x => x.Roles)
                .Excluding(x => x.Orders));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task FindAsync_Return_Ok(int userId)
        {
            // Arrange
            var userActual = await _repositoryWrapper.Users.FindIncludeAsync(userId);

            var userManagerMock = UserManagerMock.MockUserManagerSuccess(new List<User>());
            var adminCatalogService = new AdminCatalogService(_repositoryWrapper, _mapper, userManagerMock.Object);

            // Act
            var foundedUser = await adminCatalogService.FindAsync(userId);

            // Assert
            Assert.NotNull(foundedUser);
            userActual.Should().BeEquivalentTo(foundedUser);
            Assert.Equal(userId, foundedUser.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(9999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int userId)
        {
            // Arrange
            var userManagerMock = UserManagerMock.MockUserManagerSuccess(new List<User>());
            var adminCatalogService = new AdminCatalogService(_repositoryWrapper, _mapper, userManagerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => adminCatalogService.FindAsync(userId));
        }

        [Fact]
        public async Task AddAsync_Return_Ok()
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

            var usersExpectedCount = users.Count + 1;

            var createUserDto = new CreateUserDto
            {
                Country = "Belarus",
                City = "Minsk",
                Address = "My street 5, apartment 44",
                Email = "my_email@example.com",
                Login = "new-login-for-test",
                Password = "123456789",
                RolesIds = new List<int> { 1, 2 },
                Username = "TEST-USER"
            };

            var userManagerMock = UserManagerMock.MockUserManagerSuccess(users);
            var adminCatalogService = new AdminCatalogService(_repositoryWrapper, _mapper, userManagerMock.Object);

            // Act
            var createdUser = await adminCatalogService.AddAsync(createUserDto);

            // Assert
            Assert.Equal(createUserDto.Country, createdUser.Country);
            Assert.Equal(createUserDto.City, createdUser.City);
            Assert.Equal(createUserDto.Address, createdUser.Address);
            Assert.Equal(createUserDto.Email, createdUser.Email);
            Assert.Equal(createUserDto.Login, createdUser.Login);
            Assert.Equal(createUserDto.Username, createdUser.UserName);
            Assert.Equal(createUserDto.RolesIds.Count, createdUser.Roles.Count);
            Assert.Equal(usersExpectedCount, users.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(9999)]
        public async Task AddAsync_Return_DbEntityNotFoundException(int idRole)
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
            
            var createUserDto = new CreateUserDto
            {
                Country = "Belarus",
                City = "Minsk",
                Address = "My street 5, apartment 44",
                Email = "my_email@example.com",
                Login = "new-login-for-test",
                Password = "123456789",
                RolesIds = new List<int> { idRole },
                Username = "TEST-USER"
            };

            var userManagerMock = UserManagerMock.MockUserManagerSuccess(users);
            var adminCatalogService = new AdminCatalogService(_repositoryWrapper, _mapper, userManagerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => adminCatalogService.AddAsync(createUserDto));
        }

        [Fact]
        public async Task AddAsync_Return_CreateIdentityUserException()
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
            
            var createUserDto = new CreateUserDto
            {
                Country = "Belarus",
                City = "Minsk",
                Address = "My street 5, apartment 44",
                Email = "my_email@example.com",
                Login = "new-login-for-test",
                Password = "123456789",
                RolesIds = new List<int> { 1 },
                Username = "TEST-USER"
            };

            var userManagerMock = UserManagerMock.MockUserManagerFailure(users);
            var adminCatalogService = new AdminCatalogService(_repositoryWrapper, _mapper, userManagerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<CreateIdentityUserException>(() => adminCatalogService.AddAsync(createUserDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task UpdateAsync_Return_Ok(int userId)
        {
            // Arrange
            var updateUserDto = new UpdateUserDto
            {
                Id = userId,
                Country = "Belarus",
                City = "Minsk",
                Address = "My street 5, apartment 44",
                Email = "my_email@example.com",
                Login = "new-login-for-test",
                RolesIds = new List<int> { 1, 2 },
                OrderIds = new List<int> { 1 },
                Username = "TEST-USER"
            };

            var userManagerMock = UserManagerMock.MockUserManagerSuccess(new List<User>());
            var adminCatalogService = new AdminCatalogService(_repositoryWrapper, _mapper, userManagerMock.Object);

            // Act
            var updatedUser = await adminCatalogService.UpdateAsync(updateUserDto);

            // Assert
            Assert.Equal(updateUserDto.Id, updatedUser.Id);
        }

        [Theory]
        [InlineData(9999, 1, 1)]
        [InlineData(1, 9999, 1)]
        [InlineData(1, 1, 9999)]
        [InlineData(9999, 9999, 9999)]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int userId, int roleId, int orderId)
        {
            // Arrange
            var updateUserDto = new UpdateUserDto
            {
                Id = userId,
                Country = "Belarus",
                City = "Minsk",
                Address = "My street 5, apartment 44",
                Email = "my_email@example.com",
                Login = "new-login-for-test",
                RolesIds = new List<int> { roleId },
                OrderIds = new List<int> { orderId },
                Username = "TEST-USER"
            };

            var userManagerMock = UserManagerMock.MockUserManagerSuccess(new List<User>());
            var adminCatalogService = new AdminCatalogService(_repositoryWrapper, _mapper, userManagerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => adminCatalogService.UpdateAsync(updateUserDto));
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 1)]
        [InlineData(2, 2, 1)]
        [InlineData(2, 2, 2)]
        public async Task UpdateAsync_Return_UpdateIdentityUserException(int userId, int roleId, int orderId)
        {
            // Arrange
            var updateUserDto = new UpdateUserDto
            {
                Id = userId,
                Country = "Belarus",
                City = "Minsk",
                Address = "My street 5, apartment 44",
                Email = "my_email@example.com",
                Login = "new-login-for-test",
                RolesIds = new List<int> { roleId },
                OrderIds = new List<int> { orderId },
                Username = "TEST-USER"
            };

            var userManagerMock = UserManagerMock.MockUserManagerFailure(new List<User>());
            var adminCatalogService = new AdminCatalogService(_repositoryWrapper, _mapper, userManagerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<UpdateIdentityUserException>(() => adminCatalogService.UpdateAsync(updateUserDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task DeleteAsync_Return_Ok(int userId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Users.CountAsync();
            var expectedUserCount = actualCount - 1;

            var userManagerMock = UserManagerMock.MockUserManagerFailure(new List<User>());
            var adminCatalogService = new AdminCatalogService(_repositoryWrapper, _mapper, userManagerMock.Object);
            
            // Act
            await adminCatalogService.DeleteAsync(userId);
            var actualDbCount = await _repositoryWrapper.Users.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _repositoryWrapper.Users.FindIncludeAsync(userId));
            Assert.Equal(expectedUserCount, actualDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Users.CountAsync();

            var userManagerMock = UserManagerMock.MockUserManagerFailure(new List<User>());
            var adminCatalogService = new AdminCatalogService(_repositoryWrapper, _mapper, userManagerMock.Object);

            // Act
            var resultCountDb = await adminCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount, resultCountDb);
        }
    }
}

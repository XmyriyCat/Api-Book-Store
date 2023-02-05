using AutoMapper;
using BLL.DTO.Role;
using BLL.Infrastructure.Mapper;
using BLL.Services.Contract;
using BLL.Services.Implementation;
using BLL.Tests.Infrastructure;
using DLL.Errors;
using DLL.Repository.UnitOfWork;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Web_Api.Tests.Startup.DbSettings;
using Xunit;

namespace BLL.Tests.Services
{
    public class RoleCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IRoleCatalogService _roleCatalogService;

        public RoleCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();

            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _roleCatalogService = new RoleCatalogService(_repositoryWrapper, mapper);
        }

        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arrange
            var rolesSource = await _repositoryWrapper.Roles.GetAll().ToListAsync();

            // Act
            var rolesAll = await _roleCatalogService.GetAllAsync();

            // Assert
            Assert.NotNull(rolesAll);
            rolesSource.Should().BeEquivalentTo(rolesAll);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_Return_Ok(int id)
        {
            // Arrange
            var roleActual = await _repositoryWrapper.Roles.FindAsync(id);

            // Act
            var foundedRole = await _roleCatalogService.FindAsync(id);

            // Assert
            Assert.NotNull(foundedRole);
            roleActual.Should().BeEquivalentTo(foundedRole);
            Assert.Equal(id, foundedRole.Id);
        }

        [Theory]
        [InlineData(999999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int roleId)
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _roleCatalogService.FindAsync(roleId));
        }

        [Theory]
        [InlineData("role-name")]
        [InlineData("r")]
        public async Task AddAsync_Return_Ok(string roleName)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Roles.CountAsync();
            var rolesTotal = actualCount + 1;    

            var createRoleDto = new CreateRoleDto
            {
                Name = roleName
            };

            // Act
            var createdRole = await _roleCatalogService.AddAsync(createRoleDto);
            var rolesDbCount = await _repositoryWrapper.Roles.CountAsync();

            // Assert
            Assert.NotNull(createdRole);
            Assert.Equal(createRoleDto.Name, createdRole.Name);
            Assert.Equal(rolesTotal, rolesDbCount);
        }
        
        [Theory]
        [InlineData(1, "new-role-name")]
        [InlineData(2, "n")]
        public async Task UpdateAsync_Return_Ok(int id, string roleName)
        {
            // Arrange
            var roleSource = await _repositoryWrapper.Roles.FindAsync(id);

            var updateRoleDto = new UpdateRoleDto
            {
                Id = id,
                Name = roleName
            };

            // Act
            var updatedRole = await _roleCatalogService.UpdateAsync(updateRoleDto);

            // Assert
            Assert.NotNull(updatedRole);
            Assert.Equal(roleSource, updatedRole); // test EF tracking
            Assert.Equal(updateRoleDto.Id, updatedRole.Id);
            Assert.Equal(updateRoleDto.Name, updatedRole.Name);
        }
        
        [Theory]
        [InlineData(9999, "new-role-name")]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int id, string roleName)
        {
            // Arrange
            var updateRoleDto = new UpdateRoleDto
            {
                Id = id,
                Name = roleName
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _roleCatalogService.UpdateAsync(updateRoleDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteAsync_Return_DbEntityNotFoundException(int id)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Roles.CountAsync();
            var totalCount = actualCount - 1;

            // Act
            await _roleCatalogService.DeleteAsync(id);
            var rolesDbCount = await _repositoryWrapper.Roles.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _roleCatalogService.FindAsync(id));
            Assert.Equal(totalCount, rolesDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Roles.CountAsync();

            // Act
            var resultCountDb = await _roleCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount, resultCountDb);
        }
    }
}

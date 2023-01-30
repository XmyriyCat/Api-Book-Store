using AutoMapper;
using BLL.DTO.Warehouse;
using BLL.Infrastructure.Mapper;
using BLL.Services.Contract;
using BLL.Services.Implementation;
using BLL.Tests.Infrastructure;
using DLL.Errors;
using DLL.Repository.UnitOfWork;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Web_Api.Tests.Startup.DbSettings;
using Xunit;

namespace BLL.Tests.Services
{
    public class WarehouseCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IWarehouseCatalogService _warehouseCatalogService;

        public WarehouseCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();

            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _warehouseCatalogService = new WarehouseCatalogService(_repositoryWrapper, mapper);
        }

        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arrange
            var warehousesSource = await _repositoryWrapper.Warehouses.GetAll().ToListAsync();

            // Act
            var warehousesAll = await _warehouseCatalogService.GetAllAsync();

            // Assert
            Assert.NotNull(warehousesAll);
            warehousesSource.Should().BeEquivalentTo(warehousesAll);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_Return_Ok(int warehouseId)
        {
            // Arrange
            var warehouseActual = await _repositoryWrapper.Warehouses.FindAsync(warehouseId);

            // Act
            var foundedWarehouse = await _warehouseCatalogService.FindAsync(warehouseId);

            // Assert
            Assert.NotNull(foundedWarehouse);
            warehouseActual.Should().BeEquivalentTo(foundedWarehouse);
            Assert.Equal(warehouseId, foundedWarehouse.Id);
        }

        [Theory]
        [InlineData(999999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int warehouseId)
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _warehouseCatalogService.FindAsync(warehouseId));
        }


        [Theory]
        [InlineData("name", "Belarus", "Minsk", "Pushkina 26, 13", "+375291111111")]
        [InlineData("name", "Belarus", "Minsk", "Pushkina 28, 1", "+375291111112")]
        [InlineData("name", "Belarus", "Minsk", "Ushkina 29, 1", "+375292111112")]
        public async Task AddAsync_Return_Ok(string name, string country, string city, string address, string phoneNumber)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Warehouses.CountAsync();
            var warehousesTotal = actualCount + 1;

            var createWarehouseDto = new CreateWarehouseDto
            {
                Name = name,
                Country = country,
                City = city,
                Address = address,
                PhoneNumber = phoneNumber
            };

            // Act
            var warehouseCreated = await _warehouseCatalogService.AddAsync(createWarehouseDto);
            var warehousesDbCount = await _repositoryWrapper.Warehouses.CountAsync();

            // Assert
            Assert.NotNull(warehouseCreated);
            Assert.Equal(createWarehouseDto.Name, warehouseCreated.Name);
            Assert.Equal(createWarehouseDto.Country, warehouseCreated.Country);
            Assert.Equal(createWarehouseDto.City, warehouseCreated.City);
            Assert.Equal(createWarehouseDto.Address, warehouseCreated.Address);
            Assert.Equal(createWarehouseDto.PhoneNumber, warehouseCreated.PhoneNumber);
            Assert.Equal(warehousesTotal, warehousesDbCount);
        }

        [Theory]
        [InlineData("name", "Belarus", "Minsk", "Pushkina 26, 13", "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg" +
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")] 
        [InlineData("name", "Belarus", "Minsk", "Pushkina 28, 1", "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg" +
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")] 
        [InlineData("name", "Belarus", "Minsk", "Ushkina 29, 1", "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg" +
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")]
        public async Task AddAsync_Return_ValidationException(string name, string country, string city, string address, string phoneNumber)
        {
            // Arrange
            var createWarehouseDto = new CreateWarehouseDto
            {
                Name = name,
                Country = country,
                City = city,
                Address = address,
                PhoneNumber = phoneNumber
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _warehouseCatalogService.AddAsync(createWarehouseDto));
        }

        [Theory]
        [InlineData(1, "name", "Belarus", "Minsk", "Pushkina 26, 13", "+375291111111")]
        [InlineData(2, "name", "Belarus", "Minsk", "Pushkina 28, 1", "+375291111112")]
        [InlineData(2, "name", "Belarus", "Minsk", "Ushkina 29, 1", "+375292111112")]
        public async Task UpdateAsync_Return_Ok(int warehouseId, string name, string country, string city, string address, string phoneNumber)
        {
            // Arrange
            var warehouseSource = await _repositoryWrapper.Warehouses.FindAsync(warehouseId);

            var updateWarehouseDto = new UpdateWarehouseDto
            {
                Id = warehouseId,
                Name = name,
                Country = country,
                City = city,
                Address = address,
                PhoneNumber = phoneNumber
            };

            // Act
            var updatedWarehouse = await _warehouseCatalogService.UpdateAsync(updateWarehouseDto);

            // Assert
            Assert.NotNull(updatedWarehouse);
            Assert.Equal(warehouseSource, updatedWarehouse); // test EF tracking
            Assert.Equal(updateWarehouseDto.Id, updatedWarehouse.Id);
            Assert.Equal(updateWarehouseDto.Name, updatedWarehouse.Name);
            Assert.Equal(updateWarehouseDto.Country, updatedWarehouse.Country);
            Assert.Equal(updateWarehouseDto.City, updatedWarehouse.City);
            Assert.Equal(updateWarehouseDto.Address, updatedWarehouse.Address);
            Assert.Equal(updateWarehouseDto.PhoneNumber, updatedWarehouse.PhoneNumber);
        }

        [Theory]
        [InlineData(999999999, "name", "Belarus", "Minsk", "Pushkina 26, 13", "+375291111111")]
        [InlineData(999999999, "name", "Belarus", "Minsk", "Pushkina 28, 1", "+375291111112")]
        [InlineData(999999999, "name", "Belarus", "Minsk", "Ushkina 29, 1", "+375292111112")]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int warehouseId, string name, string country, string city, string address, string phoneNumber)
        {
            // Arrange
            var updateWarehouseDto = new UpdateWarehouseDto
            {
                Id = warehouseId,
                Name = name,
                Country = country,
                City = city,
                Address = address,
                PhoneNumber = phoneNumber
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _warehouseCatalogService.UpdateAsync(updateWarehouseDto));
        }

        [Theory]
        [InlineData(1,"name", "Belarus", "Minsk", "Pushkina 26, 13", "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg" +
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")]
        [InlineData(2, "name", "Belarus", "Minsk", "Pushkina 28, 1", "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg" +
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")]
        [InlineData(3, "name", "Belarus", "Minsk", "Ushkina 29, 1", "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg" +
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")]
        public async Task UpdateAsync_Return_ValidationException(int warehouseId, string name, string country, string city, string address, string phoneNumber)
        {
            // Arrange
            var updateWarehouseDto = new UpdateWarehouseDto
            {
                Id = warehouseId,
                Name = name,
                Country = country,
                City = city,
                Address = address,
                PhoneNumber = phoneNumber
            };

            // Act & Asserts
            await Assert.ThrowsAsync<ValidationException>(() => _warehouseCatalogService.UpdateAsync(updateWarehouseDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteAsync_Return_DbException(int warehouseId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Warehouses.CountAsync();
            var warehousesTotal = actualCount - 1;

            // Act
            await _warehouseCatalogService.DeleteAsync(warehouseId);
            var warehousesDbCount = await _repositoryWrapper.Warehouses.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _warehouseCatalogService.FindAsync(warehouseId));
            Assert.Equal(warehousesTotal, warehousesDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Warehouses.CountAsync();

            // Act
            var resultCountDb = await _warehouseCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount, resultCountDb);
        }
    }
}

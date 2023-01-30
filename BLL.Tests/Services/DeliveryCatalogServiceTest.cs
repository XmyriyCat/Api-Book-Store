using AutoMapper;
using BLL.DTO.Delivery;
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
    public class DeliveryCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IDeliveryCatalogService _deliveryCatalogService;

        public DeliveryCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();
            
            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _deliveryCatalogService = new DeliveryCatalogService(_repositoryWrapper, mapper);
        }

        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arrange
            var deliveriesSource = await _repositoryWrapper.Deliveries.GetAll().ToListAsync();

            // Act
            var deliveriesAll = _deliveryCatalogService.GetAllAsync().Result.ToList();

            // Assert
            Assert.NotNull(deliveriesAll);
            deliveriesSource.Should().BeEquivalentTo(deliveriesAll);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_Return_Ok(int deliveryId)
        {
            // Arrange
            var deliveryActual = await _repositoryWrapper.Deliveries.FindAsync(deliveryId);

            // Act
            var foundedDelivery = await _deliveryCatalogService.FindAsync(deliveryId);

            // Assert
            Assert.NotNull(foundedDelivery);
            deliveryActual.Should().BeEquivalentTo(foundedDelivery);
            Assert.Equal(deliveryId, foundedDelivery.Id);
        }

        [Theory]
        [InlineData(999999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int deliveryId)
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _deliveryCatalogService.FindAsync(deliveryId));
        }

        [Theory]
        [InlineData(0, "name")]
        [InlineData(1234567, "1234567890-=<>?")]
        [InlineData(100, "a")]
        [InlineData(123456789, "/*-+!@#$%^&*()")]
        public async Task AddAsync_Return_Ok(decimal price, string name)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Deliveries.CountAsync();
            var deliveriesTotal = actualCount + 1;

            var deliveryDto = new CreateDeliveryDto
            {
                Price = price,
                Name = name
            };

            // Act
            var deliveryDb = await _deliveryCatalogService.AddAsync(deliveryDto);
            var deliveriesDbCount = await _repositoryWrapper.Deliveries.CountAsync();

            // Assert
            Assert.NotNull(deliveryDb);
            Assert.Equal(deliveryDto.Price, deliveryDb.Price);
            Assert.Equal(deliveryDto.Name, deliveryDb.Name);
            Assert.Equal(deliveriesTotal, deliveriesDbCount);
        }

        [Theory]
        [InlineData(-100, "")]
        [InlineData(10, "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg" +
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")] // size of name > 150 chars.
        public async Task AddAsync_Return_ValidationException(decimal price, string name)
        {
            // Arrange
            var deliveryDto = new CreateDeliveryDto
            {
                Price = price,
                Name = name
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _deliveryCatalogService.AddAsync(deliveryDto));
        }

        [Theory]
        [InlineData(1, 0, "name")]
        [InlineData(1, 1234567, "1234567890-=<>?")]
        [InlineData(1, 100, "a")]
        [InlineData(1, 123456789, "/*-+!@#$%^&*()")]
        public async Task UpdateAsync_Return_Ok(int deliveryId, decimal price, string name)
        {
            // Arrange
            var deliverySource = await _repositoryWrapper.Deliveries.FindAsync(deliveryId);

            var updateDeliveryDto = new UpdateDeliveryDto()
            {
                Id = deliveryId,
                Price = price,
                Name = name
            };

            // Act 
            var updatedDeliveryDb = await _deliveryCatalogService.UpdateAsync(updateDeliveryDto);

            // Assert
            Assert.Equal(deliveryId, updatedDeliveryDb.Id);
            Assert.Equal(deliverySource, updatedDeliveryDb); // test EF Tracking
            Assert.Equal(name, updatedDeliveryDb.Name);
            Assert.Equal(price, updatedDeliveryDb.Price);
        }

        [Theory]
        [InlineData(9999999, 10, "name")]
        [InlineData(9999999, 100, "1234567890-=<>?")]
        [InlineData(9999999, 0, "a")]
        [InlineData(9999999, 1, "/*-+!@#$%^&*()")]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int deliveryId, decimal price, string name)
        {
            // Arrange
            var updateDeliveryDto = new UpdateDeliveryDto()
            {
                Id = deliveryId,
                Price = price,
                Name = name
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _deliveryCatalogService.UpdateAsync(updateDeliveryDto));
        }

        [Theory]
        [InlineData(1, -100, "")]
        [InlineData(1, 0, "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg" +
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")] // size of name > 150 chars.
        public async Task UpdateAsync_Return_ValidationException(int deliveryId, decimal price, string name)
        {
            // Arrange
            var updateDeliveryDto = new UpdateDeliveryDto()
            {
                Id = deliveryId,
                Price = price,
                Name = name
            };

            // Act & Asserts
            await Assert.ThrowsAsync<ValidationException>(() => _deliveryCatalogService.UpdateAsync(updateDeliveryDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task DeleteAsync_Return_DbException(int deliveryId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Deliveries.CountAsync();
            var deliveriesTotal = actualCount - 1;

            // Act
            await _deliveryCatalogService.DeleteAsync(deliveryId);
            var deliveriesDbCount = await _repositoryWrapper.Deliveries.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _deliveryCatalogService.FindAsync(deliveryId));
            Assert.Equal(deliveriesTotal, deliveriesDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Deliveries.CountAsync();

            // Act
            var resultCountDb = await _deliveryCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount, resultCountDb);
        }
    }
}
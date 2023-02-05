using AutoMapper;
using BLL.DTO.Shipment;
using BLL.Infrastructure.Mapper;
using BLL.Infrastructure.Validators.Shipment;
using BLL.Services.Contract;
using BLL.Services.Implementation;
using BLL.Tests.Infrastructure;
using DLL.Errors;
using DLL.Repository.UnitOfWork;
using FluentAssertions;
using FluentValidation;
using Web_Api.Tests.Startup.DbSettings;
using Xunit;

namespace BLL.Tests.Services
{
    public class ShipmentCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IShipmentCatalogService _shipmentCatalogService;

        public ShipmentCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();
            var createShipmentDtoValidator = new CreateShipmentDtoValidator();
            var updateShipmentDtoValidator = new UpdateShipmentDtoValidator();

            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _shipmentCatalogService = new ShipmentCatalogService(_repositoryWrapper, mapper, createShipmentDtoValidator, updateShipmentDtoValidator);
        }

        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arrange
            var shipmentsSource = await _repositoryWrapper.Shipments.GetAllIncludeAsync();

            // Act
            var shipmentsAll = await _shipmentCatalogService.GetAllAsync();

            // Assert
            Assert.NotNull(shipmentsAll);
            shipmentsSource.Should().BeEquivalentTo(shipmentsAll, options => options
                .Excluding(x => x.Delivery)
                .Excluding(x => x.PaymentWay)
            );
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_Return_Ok(int shipmentId)
        {
            // Arrange
            var shipmentActual = await _repositoryWrapper.Shipments.FindIncludeAsync(shipmentId);

            // Act
            var foundedShipment = await _shipmentCatalogService.FindAsync(shipmentId);

            // Assert
            Assert.NotNull(foundedShipment);
            shipmentActual.Should().BeEquivalentTo(foundedShipment);
            Assert.Equal(shipmentId, foundedShipment.Id);
        }

        [Theory]
        [InlineData(999999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int shipmentId)
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _shipmentCatalogService.FindAsync(shipmentId));
        }


        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 2)]
        public async Task AddAsync_Return_Ok(int deliveryId, int paymentWayId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Shipments.CountAsync();
            var shipmentsTotal = actualCount + 1;

            var createShipmentDto = new CreateShipmentDto
            {
               DeliveryId = deliveryId,
               PaymentWayId = paymentWayId
            };

            // Act
            var shipmentCreated = await _shipmentCatalogService.AddAsync(createShipmentDto);
            var shipmentsDbCount = await _repositoryWrapper.Shipments.CountAsync();

            // Assert
            Assert.NotNull(shipmentCreated);
            Assert.Equal(createShipmentDto.DeliveryId, shipmentCreated.DeliveryId);
            Assert.Equal(createShipmentDto.PaymentWayId, shipmentCreated.PaymentWayId);
            Assert.Equal(shipmentsTotal, shipmentsDbCount);
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(-2, -2)]
        public async Task AddAsync_Return_ValidationException(int deliveryId, int paymentWayId)
        {
            // Arrange
            var createShipmentDto = new CreateShipmentDto
            {
                DeliveryId = deliveryId,
                PaymentWayId = paymentWayId
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _shipmentCatalogService.AddAsync(createShipmentDto));
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 2)]
        [InlineData(3, 3, 1)]
        [InlineData(4, 4, 2)]
        public async Task UpdateAsync_Return_Ok(int shipmentId, int deliveryId, int paymentWayId)
        {
            // Arrange
            var shipmentSource = await _repositoryWrapper.Shipments.FindAsync(shipmentId);

            var updateShipmentDto = new UpdateShipmentDto()
            {
                Id = shipmentId,
                DeliveryId = deliveryId,
                PaymentWayId = paymentWayId
            };

            // Act
            var updatedShipment = await _shipmentCatalogService.UpdateAsync(updateShipmentDto);

            // Assert
            Assert.NotNull(updatedShipment);
            Assert.Equal(shipmentSource, updatedShipment); // test EF tracking
            Assert.Equal(updateShipmentDto.DeliveryId, updatedShipment.DeliveryId);
            Assert.Equal(updateShipmentDto.PaymentWayId, updatedShipment.PaymentWayId);
        }

        [Theory]
        [InlineData(999999999, 1, 1)]
        [InlineData(999999999, 2, 2)]
        [InlineData(999999999, 3, 1)]
        [InlineData(999999999, 4, 2)]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int shipmentId, int deliveryId, int paymentWayId)
        {
            // Arrange
            var updateShipmentDto = new UpdateShipmentDto()
            {
                Id = shipmentId,
                DeliveryId = deliveryId,
                PaymentWayId = paymentWayId
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _shipmentCatalogService.UpdateAsync(updateShipmentDto));
        }

        [Theory]
        [InlineData(1, -1, -1)]
        [InlineData(2, -2, -2)]
        [InlineData(3, -3, -1)]
        [InlineData(4, -4, -2)]
        public async Task UpdateAsync_Return_ValidationException(int shipmentId, int deliveryId, int paymentWayId)
        {
            // Arrange
            var updateShipmentDto = new UpdateShipmentDto()
            {
                Id = shipmentId,
                DeliveryId = deliveryId,
                PaymentWayId = paymentWayId
            };

            // Act & Asserts
            await Assert.ThrowsAsync<ValidationException>(() => _shipmentCatalogService.UpdateAsync(updateShipmentDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task DeleteAsync_Return_DbException(int shipmentId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Shipments.CountAsync();
            var shipmentsTotal = actualCount - 1;

            // Act
            await _shipmentCatalogService.DeleteAsync(shipmentId);
            var shipmentsDbCount = await _repositoryWrapper.Shipments.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _shipmentCatalogService.FindAsync(shipmentId));
            Assert.Equal(shipmentsTotal, shipmentsDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Shipments.CountAsync();

            // Act
            var resultCountDb = await _shipmentCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount, resultCountDb);
        }
    }
}

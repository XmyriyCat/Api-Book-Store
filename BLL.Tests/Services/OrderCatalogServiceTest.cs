using AutoMapper;
using BLL.DTO.Order;
using BLL.Infrastructure.Mapper;
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
    public class OrderCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IOrderCatalogService _orderCatalogService;

        public OrderCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();

            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _orderCatalogService = new OrderCatalogService(_repositoryWrapper, mapper);
        }

        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arrange
            var ordersSource = await _repositoryWrapper.Orders.GetAllIncludeAsync();

            // Act
            var ordersAll = await _orderCatalogService.GetAllAsync();

            // Assert
            Assert.NotNull(ordersAll);
            ordersAll.Should().BeEquivalentTo(ordersSource, options => options
                .Excluding(x => x.User)
                .Excluding(x => x.Shipment));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_Return_Ok(int id)
        {
            // Arrange
            var orderActual = await _repositoryWrapper.Orders.FindIncludeAsync(id);

            // Act
            var orderFounded = await _orderCatalogService.FindAsync(id);

            // Assert
            Assert.NotNull(orderFounded);
            Assert.Equal(orderActual.Id, orderFounded.Id);
            orderFounded.Should().BeEquivalentTo(orderActual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int id)
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _orderCatalogService.FindAsync(id));
        }

        [Theory]
        [InlineData(54215.4512, 1, 1)]
        [InlineData(845484.4856, 1, 2)]
        [InlineData(0, 2, 1)]
        [InlineData(875685.23231480, 2, 2)]
        public async Task AddAsync_Return_Ok(decimal totalPrice, int shipmentId, int customerId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Orders.CountAsync();
            var expectedCount = actualCount + 1;

            var createOrderDto = new CreateOrderDto
            {
                TotalPrice = totalPrice,
                OrderDate = DateTime.Now,
                ShipmentId = shipmentId,
                CustomerId = customerId
            };

            // Act
            var createdOrder = await _orderCatalogService.AddAsync(createOrderDto);
            var ordersDbCount = await _repositoryWrapper.Orders.CountAsync();

            // Assert
            Assert.NotNull(createdOrder);
            Assert.Equal(createOrderDto.TotalPrice, createdOrder.TotalPrice);
            Assert.Equal(createOrderDto.ShipmentId, createdOrder.ShipmentId);
            Assert.Equal(createOrderDto.CustomerId, createdOrder.CustomerId);
            Assert.Equal(expectedCount, ordersDbCount);
        }

        [Theory]
        [InlineData(-88465.4512, 1, 1)]
        [InlineData(123456.789, 0, 1)]
        [InlineData(123456.789, 1, 0)]
        [InlineData(123456.789, 0, 0)]
        public async Task AddAsync_Return_ValidationException(decimal totalPrice, int shipmentId, int customerId)
        {
            // Arrange
            var createOrderDto = new CreateOrderDto
            {
                TotalPrice = totalPrice,
                OrderDate = DateTime.Now,
                ShipmentId = shipmentId,
                CustomerId = customerId
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _orderCatalogService.AddAsync(createOrderDto));
        }

        [Theory]
        [InlineData(1, 54215.4512, 1, 1)]
        [InlineData(1, 456.123789, 1, 2)]
        [InlineData(2, 0, 2, 1)]
        [InlineData(2, 789456.321, 2, 2)]
        public async Task UpdateAsync_Return_Ok(int id, decimal totalPrice, int shipmentId, int customerId)
        {
            // Arrange
            var orderSource = await _repositoryWrapper.Orders.FindIncludeAsync(id);

            var updateOrderDto = new UpdateOrderDto()
            {
                Id = id,
                TotalPrice = totalPrice,
                OrderDate = DateTime.Now,
                ShipmentId = shipmentId,
                CustomerId = customerId
            };

            // Act
            var updatedOrder = await _orderCatalogService.UpdateAsync(updateOrderDto);

            // Assert
            Assert.NotNull(updatedOrder);
            Assert.Equal(orderSource, updatedOrder); // test EF tracking
            Assert.Equal(orderSource.Id, updatedOrder.Id);
            Assert.Equal(orderSource.TotalPrice, updatedOrder.TotalPrice);
            Assert.Equal(orderSource.OrderDate, updatedOrder.OrderDate);
            Assert.Equal(orderSource.ShipmentId, updatedOrder.ShipmentId);
            Assert.Equal(orderSource.CustomerId, updatedOrder.CustomerId);
        }

        [Theory]
        [InlineData(9999999, 123, 1, 1)]
        [InlineData(1, 123, 9999999, 1)]
        [InlineData(1, 123, 1, 9999999)]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int id, decimal totalPrice, int shipmentId, int customerId)
        {
            // Arrange
            var updateOrderDto = new UpdateOrderDto()
            {
                Id = id,
                TotalPrice = totalPrice,
                OrderDate = DateTime.Now,
                ShipmentId = shipmentId,
                CustomerId = customerId
            };
            
            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _orderCatalogService.UpdateAsync(updateOrderDto));
        }

        [Theory]
        [InlineData(0, 123456.789, 1, 1)]
        [InlineData(1, -88465.4512, 1, 1)]
        [InlineData(1, 123456.789, 0, 1)]
        [InlineData(1, 123456.789, 1, 0)]
        [InlineData(1, 123456.789, 0, 0)]
        public async Task UpdateAsync_Return_ValidationException(int id, decimal totalPrice, int shipmentId, int customerId)
        {
            // Arrange
            var updateOrderDto = new UpdateOrderDto()
            {
                Id = id,
                TotalPrice = totalPrice,
                OrderDate = DateTime.Now,
                ShipmentId = shipmentId,
                CustomerId = customerId
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _orderCatalogService.UpdateAsync(updateOrderDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteAsync_Return_DbEntityNotFoundException(int orderId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Orders.CountAsync();
            var ordersTotal = actualCount - 1;

            // Act
            await _orderCatalogService.DeleteAsync(orderId);
            var ordersDbCount = await _repositoryWrapper.Orders.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _orderCatalogService.FindAsync(orderId));
            Assert.Equal(ordersTotal, ordersDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var ordersCount = await _repositoryWrapper.Orders.CountAsync();

            // Act
            var ordersDbCount = await _orderCatalogService.CountAsync();

            // Assert
            Assert.Equal(ordersDbCount, ordersCount);
        }
    }
}

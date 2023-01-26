using AutoMapper;
using BLL.DTO.OrderLine;
using BLL.Infrastructure.Mapper;
using BLL.Infrastructure.Validators.OrderLine;
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
    public class OrderLineCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IOrderLineCatalogService _orderLineCatalogService;

        public OrderLineCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();
            var createOrderLineDtoValidator = new CreateOrderLineDtoValidator();
            var updateOrderLineDtoValidator = new UpdateOrderLineDtoValidator();

            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _orderLineCatalogService = new OrderLineCatalogService(_repositoryWrapper, mapper, createOrderLineDtoValidator, updateOrderLineDtoValidator);
        }
        
        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arrange
            var orderLinesSource = await _repositoryWrapper.OrderLines.GetAllIncludeAsync();

            // Act
            var orderLinesAll = await _orderLineCatalogService.GetAllAsync();

            // Assert
            Assert.NotNull(orderLinesAll);
            orderLinesSource.Should().BeEquivalentTo(orderLinesAll, options => options
                .Excluding(x => x.WarehouseBook)
                .Excluding(x => x.Order));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_Return_Ok(int orderLineId)
        {
            // Arrange
            var orderLineActual = await _repositoryWrapper.OrderLines.FindIncludeAsync(orderLineId);

            // Act
            var foundedOrderLine = await _orderLineCatalogService.FindAsync(orderLineId);

            // Assert
            Assert.NotNull(foundedOrderLine);
            orderLineActual.Should().BeEquivalentTo(foundedOrderLine);
            Assert.Equal(orderLineId, foundedOrderLine.Id);
        }

        [Theory]
        [InlineData(999999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int bookId)
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _orderLineCatalogService.FindAsync(bookId));
        }

        [Theory]
        [InlineData(70, 1, 1)]
        [InlineData(25, 1, 2)]
        [InlineData(6, 2, 1)]
        [InlineData(18, 2, 2)]
        public async Task AddAsync_Return_Ok(int quantity, int orderId, int warehouseBookId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.OrderLines.CountAsync();
            var orderLinesTotal = actualCount + 1;

            var createOrderLineDto = new CreateOrderLineDto
            {
                Quantity = quantity,
                OrderId = orderId,
                WarehouseBookId = warehouseBookId
            };

            // Act
            var createdOrderLine = await _orderLineCatalogService.AddAsync(createOrderLineDto);
            var orderLinesDbCount = await _repositoryWrapper.OrderLines.CountAsync();

            // Assert
            Assert.NotNull(createdOrderLine);
            Assert.Equal(createOrderLineDto.Quantity, createdOrderLine.Quantity);
            Assert.Equal(createOrderLineDto.OrderId, createdOrderLine.OrderId);
            Assert.Equal(createOrderLineDto.WarehouseBookId, createdOrderLine.WarehouseBookId);
            Assert.Equal(orderLinesTotal, orderLinesDbCount);
        }

        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(25, 0, 2)]
        [InlineData(6, 2, 0)]
        [InlineData(0, 0, 0)]
        public async Task AddAsync_Return_ValidationException(int quantity, int orderId, int warehouseBookId)
        {
            // Arrange
            var createOrderLineDto = new CreateOrderLineDto
            {
                Quantity = quantity,
                OrderId = orderId,
                WarehouseBookId = warehouseBookId
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _orderLineCatalogService.AddAsync(createOrderLineDto));
        }

        [Theory]
        [InlineData(2, 9999, 1)]
        [InlineData(25, 1, 9999)]
        [InlineData(6, 9999, 9999)]
        public async Task AddAsync_Return_DbEntityNotFoundException(int quantity, int orderId, int warehouseBookId)
        {
            // Arrange
            var createOrderLineDto = new CreateOrderLineDto
            {
                Quantity = quantity,
                OrderId = orderId,
                WarehouseBookId = warehouseBookId
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _orderLineCatalogService.AddAsync(createOrderLineDto));
        }

        [Theory]
        [InlineData(1, 70, 1, 1)]
        [InlineData(1, 23, 1, 2)]
        [InlineData(2, 11, 2, 1)]
        [InlineData(2, 163, 2, 2)]
        public async Task UpdateAsync_Return_Ok(int id, int quantity, int orderId, int warehouseBookId)
        {
            // Arrange
            var orderLineSource = await _repositoryWrapper.OrderLines.FindIncludeAsync(id);

            var updateOrderLineDto = new UpdateOrderLineDto
            {
                Id = id,
                Quantity = quantity,
                OrderId = orderId,
                WarehouseBookId = warehouseBookId
            };

            // Act
            var updatedOrderLine = await _orderLineCatalogService.UpdateAsync(updateOrderLineDto);

            // Assert
            Assert.NotNull(updatedOrderLine);
            Assert.Equal(orderLineSource, updatedOrderLine); // test EF tracking
            Assert.Equal(updateOrderLineDto.Id, updatedOrderLine.Id);
            Assert.Equal(updateOrderLineDto.Quantity, updatedOrderLine.Quantity);
            Assert.Equal(updateOrderLineDto.OrderId, updatedOrderLine.OrderId);
            Assert.Equal(updateOrderLineDto.WarehouseBookId, updatedOrderLine.WarehouseBookId);
        }

        [Theory]
        [InlineData(0, 70, 1, 1)]
        [InlineData(1, 0, 1, 2)]
        [InlineData(2, 11, 0, 1)]
        [InlineData(2, 163, 2, 0)]
        [InlineData(0, 0, 0, 0)]
        public async Task UpdateAsync_Return_ValidationException(int id, int quantity, int orderId, int warehouseBookId)
        {
            // Arrange
            var updateOrderLineDto = new UpdateOrderLineDto
            {
                Id = id,
                Quantity = quantity,
                OrderId = orderId,
                WarehouseBookId = warehouseBookId
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _orderLineCatalogService.UpdateAsync(updateOrderLineDto));
        }

        [Theory]
        [InlineData(9999, 70, 1, 1)]
        [InlineData(2, 11, 9999, 1)]
        [InlineData(2, 163, 2, 9999)]
        [InlineData(9999, 45, 9999, 9999)]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int id, int quantity, int orderId, int warehouseBookId)
        {
            // Arrange
            var updateOrderLineDto = new UpdateOrderLineDto
            {
                Id = id,
                Quantity = quantity,
                OrderId = orderId,
                WarehouseBookId = warehouseBookId
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _orderLineCatalogService.UpdateAsync(updateOrderLineDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteAsync_Return_DbException(int orderLineId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.OrderLines.CountAsync();
            var orderLinesTotal = actualCount - 1;

            // Act
            await _orderLineCatalogService.DeleteAsync(orderLineId);
            var orderLinesDbCount = await _repositoryWrapper.OrderLines.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _orderLineCatalogService.FindAsync(orderLineId));
            Assert.Equal(orderLinesTotal, orderLinesDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var actualCount = await _repositoryWrapper.OrderLines.CountAsync();

            // Act
            var resultCountDb = await _orderLineCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount, resultCountDb);
        }
    }
}

using AutoMapper;
using BLL.DTO.PaymentWay;
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
    public class PaymentWayCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IPaymentWayCatalogService _paymentWayCatalogService;
        
        public PaymentWayCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();

            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _paymentWayCatalogService = new PaymentWayCatalogService(_repositoryWrapper, mapper);
        }

        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arrange
            var paymentWaysSource = await _repositoryWrapper.PaymentWays.GetAll().ToListAsync();

            // Act
            var paymentWaysAll = await _paymentWayCatalogService.GetAllAsync();

            // Assert
            Assert.NotNull(paymentWaysAll);
            paymentWaysSource.Should().BeEquivalentTo(paymentWaysAll);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_Return_Ok(int paymentWayId)
        {
            // Arrange
            var bookActual = await _repositoryWrapper.PaymentWays.FindAsync(paymentWayId);

            // Act
            var foundedBook = await _paymentWayCatalogService.FindAsync(paymentWayId);

            // Assert
            Assert.NotNull(foundedBook);
            bookActual.Should().BeEquivalentTo(foundedBook);
            Assert.Equal(paymentWayId, foundedBook.Id);
        }

        [Theory]
        [InlineData(999999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int paymentWayId)
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _paymentWayCatalogService.FindAsync(paymentWayId));
        }

        [Theory]
        [InlineData("test-payment-way")]
        [InlineData("t")]
        public async Task AddAsync_Return_Ok(string name)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.PaymentWays.CountAsync();
            var paymentWaysTotal = actualCount + 1;

            var createPaymentWayDto = new CreatePaymentWayDto
            {
                Name = name
            };

            // Act
            var createdPaymentWay = await _paymentWayCatalogService.AddAsync(createPaymentWayDto);
            var paymentWaysDbCount = await _repositoryWrapper.PaymentWays.CountAsync();

            // Assert
            Assert.NotNull(createdPaymentWay);
            Assert.Equal(createPaymentWayDto.Name, createdPaymentWay.Name);
            Assert.Equal(paymentWaysTotal, paymentWaysDbCount);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula " +
                    "eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pa")] // // size of payment way name > 150 chars.
        public async Task AddAsync_Return_ValidationException(string name)
        {
            // Arrange
            var createPaymentWayDto = new CreatePaymentWayDto
            {
                Name = name
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _paymentWayCatalogService.AddAsync(createPaymentWayDto));
        }

        [Theory]
        [InlineData(1, "new-test-name")]
        [InlineData(2, "simple-name")]
        public async Task UpdateAsync_Return_Ok(int id, string name)
        {
            // Arrange
            var sourcePaymentWay = await _repositoryWrapper.PaymentWays.FindAsync(id);
            
            var updatePaymentWayDto = new UpdatePaymentWayDto
            {
                Id = id,
                Name = name
            };

            // Act
            var updatedPaymentWay = await _paymentWayCatalogService.UpdateAsync(updatePaymentWayDto);

            // Assert
            Assert.NotNull(updatedPaymentWay);
            Assert.Equal(sourcePaymentWay, updatedPaymentWay); // test EF tracking
            Assert.Equal(updatePaymentWayDto.Id, updatedPaymentWay.Id);
            Assert.Equal(updatePaymentWayDto.Name, updatedPaymentWay.Name);
        }

        [Theory]
        [InlineData(1, "")]
        [InlineData(2, "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula " +
                    "eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pa")] // size of payment way name > 150 chars.
        public async Task UpdateAsync_Return_ValidationException(int id, string name)
        {
            // Arrange
            var updatePaymentWayDto = new UpdatePaymentWayDto
            {
                Id = id,
                Name = name
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _paymentWayCatalogService.UpdateAsync(updatePaymentWayDto));
        }

        [Theory]
        [InlineData(9999, "new-test-name")]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int id, string name)
        {
            // Arrange
            var updatePaymentWayDto = new UpdatePaymentWayDto
            {
                Id = id,
                Name = name
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _paymentWayCatalogService.UpdateAsync(updatePaymentWayDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteAsync_Return_DbException(int paymentWayId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.PaymentWays.CountAsync();
            var paymentWaysTotal = actualCount - 1;

            // Act
            await _paymentWayCatalogService.DeleteAsync(paymentWayId);
            var paymentWaysDbCount = await _repositoryWrapper.PaymentWays.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _paymentWayCatalogService.FindAsync(paymentWayId));
            Assert.Equal(paymentWaysTotal, paymentWaysDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var actualCount = await _repositoryWrapper.PaymentWays.CountAsync();

            // Act
            var resultCountDb = await _paymentWayCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount, resultCountDb);
        }
    }
}

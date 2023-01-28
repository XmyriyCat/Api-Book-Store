using AutoMapper;
using BLL.DTO.WarehouseBook;
using BLL.Infrastructure.Mapper;
using BLL.Infrastructure.Validators.WarehouseBook;
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
    public class WarehouseBookCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IWarehouseBookCatalogService _warehouseBookCatalogService;

        public WarehouseBookCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();
            var createWarehouseBookDtoValidator = new CreateWarehouseBookDtoValidator();
            var updateWarehouseBookDtoValidator = new UpdateWarehouseBookDtoValidator();

            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _warehouseBookCatalogService = new WarehouseBookCatalogService(_repositoryWrapper, mapper, createWarehouseBookDtoValidator, updateWarehouseBookDtoValidator);
        }

        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arrange
            var warehouseBooksSource = await _repositoryWrapper.WarehouseBooks.GetAllIncludeAsync();

            // Act
            var warehouseBooksAll = await _warehouseBookCatalogService.GetAllAsync();

            // Assert
            Assert.NotNull(warehouseBooksAll);
            warehouseBooksSource.Should().BeEquivalentTo(warehouseBooksAll, options => options
                .Excluding(x => x.Warehouse)
                .Excluding(x => x.Book)
            );
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_Return_Ok(int warehouseBookId)
        {
            // Arrange
            var warehouseBookActual = await _repositoryWrapper.WarehouseBooks.FindIncludeAsync(warehouseBookId);

            // Act
            var foundedWarehouseBook = await _warehouseBookCatalogService.FindAsync(warehouseBookId);

            // Assert
            Assert.NotNull(foundedWarehouseBook);
            warehouseBookActual.Should().BeEquivalentTo(foundedWarehouseBook);
            Assert.Equal(warehouseBookId, foundedWarehouseBook.Id);
        }

        [Theory]
        [InlineData(999999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int warehouseBookId)
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _warehouseBookCatalogService.FindAsync(warehouseBookId));
        }


        [Theory]
        [InlineData(100, 1, 1)]
        [InlineData(200, 2, 2)]
        [InlineData(300, 1, 2)]
        [InlineData(105, 2, 1)]
        public async Task AddAsync_Return_Ok(int quantity, int warehouseId, int bookId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.WarehouseBooks.CountAsync();
            var warehouseBooksTotal = actualCount + 1;

            var createWarehouseBookDto = new CreateWarehouseBookDto
            {
                Quantity = quantity,
                WarehouseId = warehouseId,
                BookId = bookId
            };

            // Act
            var warehouseBookCreated = await _warehouseBookCatalogService.AddAsync(createWarehouseBookDto);
            var warehouseBooksDbCount = await _repositoryWrapper.WarehouseBooks.CountAsync();

            // Assert
            Assert.NotNull(warehouseBookCreated);
            Assert.Equal(createWarehouseBookDto.Quantity, warehouseBookCreated.Quantity);
            Assert.Equal(createWarehouseBookDto.WarehouseId, warehouseBookCreated.WarehouseId);
            Assert.Equal(createWarehouseBookDto.BookId, warehouseBookCreated.BookId);
            Assert.Equal(warehouseBooksTotal, warehouseBooksDbCount);
        }

        [Theory]
        [InlineData(-100, 1, 1)]
        [InlineData(-200, 2, 2)]
        [InlineData(-300, 3, 3)]
        [InlineData(-105, 4, 4)]
        public async Task AddAsync_Return_ValidationException(int quantity, int warehouseId, int bookId)
        {
            // Arrange
            var createWarehouseBookDto = new CreateWarehouseBookDto
            {
                Quantity = quantity,
                WarehouseId = warehouseId,
                BookId = bookId
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _warehouseBookCatalogService.AddAsync(createWarehouseBookDto));
        }

        [Theory]
        [InlineData(1, 100, 1, 1)]
        [InlineData(1, 200, 2, 2)]
        [InlineData(2, 300, 1, 2)]
        [InlineData(2, 105, 2, 1)]
        public async Task UpdateAsync_Return_Ok(int warehouseBookId, int quantity, int warehouseId, int bookId)
        {
            // Arrange
            var warehouseBookSource = await _repositoryWrapper.WarehouseBooks.FindAsync(warehouseBookId);

            var updateWarehouseBookDto = new UpdateWarehouseBookDto()
            {
                Id = warehouseBookId,
                Quantity = quantity,
                WarehouseId = warehouseId,
                BookId = bookId
            };

            // Act
            var updatedWarehouseBook = await _warehouseBookCatalogService.UpdateAsync(updateWarehouseBookDto);

            // Assert
            Assert.NotNull(updatedWarehouseBook);
            Assert.Equal(warehouseBookSource, updatedWarehouseBook); // test EF tracking
            Assert.Equal(updateWarehouseBookDto.Id, updatedWarehouseBook.Id);
            Assert.Equal(updateWarehouseBookDto.Quantity, updatedWarehouseBook.Quantity);
            Assert.Equal(updateWarehouseBookDto.WarehouseId, updatedWarehouseBook.WarehouseId);
            Assert.Equal(updateWarehouseBookDto.BookId, updatedWarehouseBook.BookId);
        }

        [Theory]
        [InlineData(999999999, 100, 1, 1)]
        [InlineData(999999999, 200, 2, 2)]
        [InlineData(999999999, 300, 3, 3)]
        [InlineData(999999999, 105, 4, 4)]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int warehouseBookId, int quantity, int warehouseId, int bookId)
        {
            // Arrange
            var updateWarehouseBookDto = new UpdateWarehouseBookDto()
            {
                Id = warehouseBookId,
                Quantity = quantity,
                WarehouseId = warehouseId,
                BookId = bookId
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _warehouseBookCatalogService.UpdateAsync(updateWarehouseBookDto));
        }

        [Theory]
        [InlineData(999999999, -100, 1, 1)]
        [InlineData(999999999, -200, 2, 2)]
        [InlineData(999999999, -300, 3, 3)]
        [InlineData(999999999, -105, 4, 4)]
        public async Task UpdateAsync_Return_ValidationException(int warehouseBookId, int quantity, int warehouseId, int bookId)
        {
            // Arrange
            var updateWarehouseBookDto = new UpdateWarehouseBookDto()
            {
                Id = warehouseBookId,
                Quantity = quantity,
                WarehouseId = warehouseId,
                BookId = bookId
            };

            // Act & Asserts
            await Assert.ThrowsAsync<ValidationException>(() => _warehouseBookCatalogService.UpdateAsync(updateWarehouseBookDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteAsync_Return_DbException(int warehouseBookId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.WarehouseBooks.CountAsync();
            var warehouseBooksTotal = actualCount - 1;

            // Act
            await _warehouseBookCatalogService.DeleteAsync(warehouseBookId);
            var warehouseBooksDbCount = await _repositoryWrapper.WarehouseBooks.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _warehouseBookCatalogService.FindAsync(warehouseBookId));
            Assert.Equal(warehouseBooksTotal, warehouseBooksDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var actualCount = await _repositoryWrapper.WarehouseBooks.CountAsync();

            // Act
            var resultCountDb = await _warehouseBookCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount, resultCountDb);
        }
    }
}

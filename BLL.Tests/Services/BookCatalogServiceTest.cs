using AutoMapper;
using BLL.DTO.Book;
using BLL.Infrastructure.Mapper;
using BLL.Infrastructure.Validators.Book;
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
    public class BookCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBookCatalogService _bookCatalogService;

        public BookCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();
            var createBookDtoValidator = new CreateBookDtoValidator();
            var updateBookDtoValidator = new UpdateBookDtoValidator();

            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _bookCatalogService = new BookCatalogService(_repositoryWrapper, mapper, createBookDtoValidator, updateBookDtoValidator);
        }

        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arrange
            var booksSource = await _repositoryWrapper.Books.GetAllIncludeAsync();

            // Act
            var booksAll = await _bookCatalogService.GetAllAsync();

            // Assert
            Assert.NotNull(booksAll);
            booksSource.Should().BeEquivalentTo(booksAll, options => options
                .Excluding(x => x.Publisher)
                .Excluding(x => x.Genres)
                .Excluding(x => x.Authors)
            );
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_Return_Ok(int bookId)
        {
            // Arrange
            var bookActual = await _repositoryWrapper.Books.FindIncludeAsync(bookId);

            // Act
            var foundedBook = await _bookCatalogService.FindAsync(bookId);

            // Assert
            Assert.NotNull(foundedBook);
            bookActual.Should().BeEquivalentTo(foundedBook);
            Assert.Equal(bookId, foundedBook.Id);
        }

        [Theory]
        [InlineData(999999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int bookId)
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _bookCatalogService.FindAsync(bookId));
        }


        [Theory]
        [InlineData("Classic book", 85.52, 1, 1, 1)]
        [InlineData("1234567890-=<>?", 78652.556, 2, 2, 2)]
        [InlineData("a", 7896321.245, 1, 2, 3)]
        [InlineData("/*-+!@#$%^&*()", 12345788.2645343121, 3, 3, 3)]
        public async Task AddAsync_Return_Ok(string bookName, decimal price, int idPublisher, int idGenre, int idAuthor)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Books.CountAsync();
            var booksTotal = actualCount + 1;

            var createBookDto = new CreateBookDto
            {
                Name = bookName,
                Price = price,
                IdPublisher = idPublisher,
                GenresId = new List<int> { idGenre },
                AuthorsId = new List<int> { idAuthor }
            };

            // Act
            var bookCreated = await _bookCatalogService.AddAsync(createBookDto);
            var booksDbCount = await _repositoryWrapper.Books.CountAsync();

            // Assert
            Assert.NotNull(bookCreated);
            Assert.Equal(createBookDto.Name, bookCreated.Name);
            Assert.Equal(createBookDto.Price, bookCreated.Price);
            Assert.Equal(createBookDto.IdPublisher, bookCreated.Publisher.Id);
            Assert.Equal(booksTotal, booksDbCount);
        }

        [Theory]
        [InlineData("", 85.52, 1, 1, 1)]
        [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor." +
            " Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec qua", 8685.52552, 2, 2, 2)] // size of book name > 200 chars.
        public async Task AddAsync_Return_ValidationException(string bookName, decimal price, int idPublisher, int idGenre, int idAuthor)
        {
            // Arrange
            var createBookDto = new CreateBookDto
            {
                Name = bookName,
                Price = price,
                IdPublisher = idPublisher,
                GenresId = new List<int> { idGenre },
                AuthorsId = new List<int> { idAuthor }
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _bookCatalogService.AddAsync(createBookDto));
        }

        [Theory]
        [InlineData(1, "Classic book", 85.52, 1, 1, 1)]
        [InlineData(1, "1234567890-=<>?", 78652.556, 2, 2, 2)]
        [InlineData(2, "a", 7896321.245, 1, 2, 3)]
        [InlineData(3, "/*-+!@#$%^&*()", 12345788.2645343121, 3, 3, 3)]
        public async Task UpdateAsync_Return_Ok(int bookId, string bookName, decimal price, int idPublisher, int idGenre, int idAuthor)
        {
            // Arrange
            var bookSource = await _repositoryWrapper.Books.FindAsync(bookId);

            var updateBookDto = new UpdateBookDto()
            {
                Id = bookId,
                Name = bookName,
                Price = price,
                IdPublisher = idPublisher,
                GenresId = new List<int> { idGenre },
                AuthorsId = new List<int> { idAuthor }
            };

            // Act
            var updatedBook = await _bookCatalogService.UpdateAsync(updateBookDto);

            // Assert
            Assert.NotNull(updatedBook);
            Assert.Equal(bookSource, updatedBook); // test EF tracking
            Assert.Equal(updateBookDto.Name, updatedBook.Name);
            Assert.Equal(updateBookDto.Price, updatedBook.Price);
            Assert.Equal(updateBookDto.IdPublisher, updatedBook.Publisher.Id);
        }

        [Theory]
        [InlineData(9999999, "Classic book", 85.52, 1, 1, 1)]
        [InlineData(9999999, "1234567890-=<>?", 78652.556, 2, 2, 2)]
        [InlineData(9999999, "a", 7896321.245, 1, 2, 3)]
        [InlineData(9999999, "/*-+!@#$%^&*()", 12345788.2645343121, 3, 3, 3)]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int bookId, string bookName, decimal price, int idPublisher, int idGenre, int idAuthor)
        {
            // Arrange
            var updateBookDto = new UpdateBookDto()
            {
                Id = bookId,
                Name = bookName,
                Price = price,
                IdPublisher = idPublisher,
                GenresId = new List<int> { idGenre },
                AuthorsId = new List<int> { idAuthor }
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _bookCatalogService.UpdateAsync(updateBookDto));
        }

        [Theory]
        [InlineData(1, "", 85.52, 1, 1, 1)]
        [InlineData(2, "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor." +
                    " Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec qua", 8685.52552, 2, 2, 2)] // size of book name > 200 chars.
        public async Task UpdateAsync_Return_ValidationException(int bookId, string bookName, decimal price, int idPublisher, int idGenre, int idAuthor)
        {
            // Arrange
            var updateBookDto = new UpdateBookDto()
            {
                Id = bookId,
                Name = bookName,
                Price = price,
                IdPublisher = idPublisher,
                GenresId = new List<int> { idGenre },
                AuthorsId = new List<int> { idAuthor }
            };
            
            // Act & Asserts
            await Assert.ThrowsAsync<ValidationException>(() => _bookCatalogService.UpdateAsync(updateBookDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task DeleteAsync_Return_DbException(int bookId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Books.CountAsync();
            var booksTotal = actualCount - 1;

            // Act
            await _bookCatalogService.DeleteAsync(bookId);
            var booksDbCount = await _repositoryWrapper.Books.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _bookCatalogService.FindAsync(bookId));
            Assert.Equal(booksTotal, booksDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Authors.CountAsync();

            // Act
            var resultCountDb = await _bookCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount, resultCountDb);
        }
    }
}

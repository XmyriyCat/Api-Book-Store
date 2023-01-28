using AutoMapper;
using BLL.DTO.Genre;
using BLL.Infrastructure.Mapper;
using BLL.Infrastructure.Validators.Genre;
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
    public class GenreCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IGenreCatalogService _genreCatalogService;

        public GenreCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();
            var createGenreDtoValidator = new CreateGenreDtoValidator();
            var updateGenreDtoValidator = new UpdateGenreDtoValidator();

            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _genreCatalogService = new GenreCatalogService(_repositoryWrapper, mapper, createGenreDtoValidator, updateGenreDtoValidator);
        }

        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arrange
            var genresSource = await _repositoryWrapper.Genres.GetAll().ToListAsync();

            // Act
            var genresAll = _genreCatalogService.GetAllAsync().Result.ToList();

            // Assert
            Assert.NotNull(genresAll);
            genresSource.Should().BeEquivalentTo(genresAll);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_Return_Ok(int genreId)
        {
            // Arrange
            var genreActual = await _repositoryWrapper.Genres.FindAsync(genreId);

            // Act
            var foundedGenre = await _genreCatalogService.FindAsync(genreId);

            // Assert
            Assert.NotNull(foundedGenre);
            genreActual.Should().BeEquivalentTo(foundedGenre);
            Assert.Equal(genreId, foundedGenre.Id);
        }

        [Theory]
        [InlineData(999999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int genreId)
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _genreCatalogService.FindAsync(genreId));
        }

        [Theory]
        [InlineData("name")]
        [InlineData("1234567890-=<>?")]
        [InlineData("a")]
        [InlineData("/*-+!@#$%^&*()")]
        public async Task AddAsync_Return_Ok(string name)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Genres.CountAsync();
            var genresTotal = actualCount + 1;

            var genreDto = new CreateGenreDto
            {
                Name = name
            };

            // Act
            var genreDb = await _genreCatalogService.AddAsync(genreDto);
            var genresDbCount = await _repositoryWrapper.Genres.CountAsync();

            // Assert
            Assert.NotNull(genreDb);
            Assert.Equal(genreDto.Name, genreDb.Name);
            Assert.Equal(genresTotal, genresDbCount);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg" +
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")] // size of name > 150 chars.
        public async Task AddAsync_Return_ValidationException(string name)
        {
            // Arrange
            var genreDto = new CreateGenreDto
            {             
                Name = name
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _genreCatalogService.AddAsync(genreDto));
        }

        [Theory]
        [InlineData(1, "name")]
        [InlineData(1, "1234567890-=<>?")]
        [InlineData(1, "a")]
        [InlineData(1, "/*-+!@#$%^&*()")]
        public async Task UpdateAsync_Return_Ok(int genreId, string name)
        {
            // Arrange
            var genreSource = await _repositoryWrapper.Genres.FindAsync(genreId);

            var updateGenreDto = new UpdateGenreDto()
            {
                Id = genreId,
                Name = name
            };

            // Act 
            var updatedGenreDb = await _genreCatalogService.UpdateAsync(updateGenreDto);

            // Assert
            Assert.Equal(genreId, updatedGenreDb.Id);
            Assert.Equal(genreSource, updatedGenreDb); // test EF Tracking
            Assert.Equal(name, updatedGenreDb.Name);          
        }

        [Theory]
        [InlineData(9999999, "name")]
        [InlineData(9999999, "1234567890-=<>?")]
        [InlineData(9999999, "a")]
        [InlineData(9999999, "/*-+!@#$%^&*()")]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int genreId, string name)
        {
            // Arrange
            var updateGenreDto = new UpdateGenreDto()
            {
                Id = genreId,
                Name = name
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _genreCatalogService.UpdateAsync(updateGenreDto));
        }

        [Theory]
        [InlineData(1, "")]
        [InlineData(1, "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg" +
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")] // size of name > 150 chars.
        public async Task UpdateAsync_Return_ValidationException(int genreId, string name)
        {
            // Arrange
            var updateGenreDto = new UpdateGenreDto()
            {
                Id = genreId,
                Name = name
            };

            // Act & Asserts
            await Assert.ThrowsAsync<ValidationException>(() => _genreCatalogService.UpdateAsync(updateGenreDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task DeleteAsync_Return_DbException(int genreId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Genres.CountAsync();
            var genresTotal = actualCount - 1;

            // Act
            await _genreCatalogService.DeleteAsync(genreId);
            var genresDbCount = await _repositoryWrapper.Genres.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _genreCatalogService.FindAsync(genreId));
            Assert.Equal(genresTotal, genresDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Genres.CountAsync();

            // Act
            var resultCountDb = await _genreCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount, resultCountDb);
        }
    }
}
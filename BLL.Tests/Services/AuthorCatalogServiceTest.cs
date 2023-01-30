using AutoMapper;
using BLL.DTO.Author;
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
    public class AuthorCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IAuthorCatalogService _authorCatalogService;

        public AuthorCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            
            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();
            
            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _authorCatalogService = new AuthorCatalogService(_repositoryWrapper, mapper);
        }

        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arrange
            var authorsSource = await _repositoryWrapper.Authors.GetAll().ToListAsync();

            // Act
            var authorsAll = _authorCatalogService.GetAllAsync().Result.ToList();

            // Assert
            Assert.NotNull(authorsAll);
            authorsSource.Should().BeEquivalentTo(authorsAll);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_Return_Ok(int authorId)
        {
            // Arrange
            var authorActual = await _repositoryWrapper.Authors.FindAsync(authorId);
            
            // Act
            var foundedAuthor = await _authorCatalogService.FindAsync(authorId);

            // Assert
            Assert.NotNull(foundedAuthor);
            authorActual.Should().BeEquivalentTo(foundedAuthor);
            Assert.Equal(authorId, foundedAuthor.Id);
        }

        [Theory]
        [InlineData(999999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int authorId)
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _authorCatalogService.FindAsync(authorId));
        }

        [Theory]
        [InlineData("firstname", "lastname")]
        [InlineData("1234567890-=<>?", "1234567890-=<>?")]
        [InlineData("a", "a")]
        [InlineData("/*-+!@#$%^&*()", "/*-+!@#$%^&*()")]
        public async Task AddAsync_Return_Ok(string firstname, string lastname)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Authors.CountAsync();
            var authorsTotal = actualCount + 1;

            var authorDto = new CreateAuthorDto
            {
                FirstName = firstname,
                LastName = lastname
            };

            // Act
            var authorDb = await _authorCatalogService.AddAsync(authorDto);
            var authorsDbCount = await _repositoryWrapper.Authors.CountAsync();

            // Assert
            Assert.NotNull(authorDb);
            Assert.Equal(authorDto.FirstName, authorDb.FirstName);
            Assert.Equal(authorDto.LastName, authorDb.LastName);
            Assert.Equal(authorsTotal, authorsDbCount);
        }
        
        [Theory]
        [InlineData(1, "new_firstname", "new_lastname")]
        [InlineData(1, "1234567890-=<>?", "1234567890-=<>?")]
        [InlineData(1, "a", "a")]
        [InlineData(1, "/*-+!@#$%^&*()", "/*-+!@#$%^&*()")]
        public async Task UpdateAsync_Return_Ok(int authorId, string firstnameNew, string lastnameNew)
        {
            // Arrange
            var authorSource = await _repositoryWrapper.Authors.FindAsync(authorId);

            var updateAuthorDto = new UpdateAuthorDto()
            {
                Id = authorId,
                FirstName = firstnameNew,
                LastName = lastnameNew
            };

            // Act 
            var updatedAuthorDb = await _authorCatalogService.UpdateAsync(updateAuthorDto);

            // Assert
            Assert.Equal(authorId, updatedAuthorDb.Id);
            Assert.Equal(authorSource, updatedAuthorDb); // test EF Tracking
            Assert.Equal(firstnameNew, updatedAuthorDb.FirstName);
            Assert.Equal(lastnameNew, updatedAuthorDb.LastName);
        }

        [Theory]
        [InlineData(9999999, "new_firstname", "new_lastname")]
        [InlineData(9999999, "1234567890-=<>?", "1234567890-=<>?")]
        [InlineData(9999999, "a", "a")]
        [InlineData(9999999, "/*-+!@#$%^&*()", "/*-+!@#$%^&*()")]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int authorId, string firstnameNew, string lastnameNew)
        {
            // Arrange
            var updateAuthorDto = new UpdateAuthorDto()
            {
                Id = authorId,
                FirstName = firstnameNew,
                LastName = lastnameNew
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _authorCatalogService.UpdateAsync(updateAuthorDto));
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task DeleteAsync_Return_DbException(int authorId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Authors.CountAsync();
            var authorsTotal = actualCount - 1;

            // Act
            await _authorCatalogService.DeleteAsync(authorId);
            var authorsDbCount = await _repositoryWrapper.Authors.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _authorCatalogService.FindAsync(authorId));
            Assert.Equal(authorsTotal, authorsDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Authors.CountAsync();

            // Act
            var resultCountDb = await _authorCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount, resultCountDb);
        }
    }
}
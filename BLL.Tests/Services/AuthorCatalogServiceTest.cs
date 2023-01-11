using AutoMapper;
using BLL.DTO.Author;
using BLL.Infrastructure.Mapper;
using BLL.Infrastructure.Validators.Author;
using BLL.Services.Contract;
using BLL.Services.Implementation;
using BLL.Tests.Infrastructure;
using DLL.Errors;
using FluentValidation;
using Xunit;

namespace BLL.Tests.Services
{
    public class AuthorCatalogServiceTest
    {
        private readonly IAuthorCatalogService _authorCatalogService;

        public AuthorCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // for testing DB commands inMemoryDB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            var repositoryWrapperInMemory = new RepositoryWrapperInMemory(dbContextInMemory);
            var mapper = mapperConfiguration.CreateMapper();
            var createAuthorDtoValidator = new CreateAuthorDtoValidator();
            var updateAuthorDtoValidator = new UpdateAuthorDtoValidator();

            _authorCatalogService = new AuthorCatalogService(repositoryWrapperInMemory, mapper,
                createAuthorDtoValidator, updateAuthorDtoValidator);
        }

        [Fact]
        public async Task GetAllAsync_ReturnOk()
        {
            // Arrange
            var actualCount = await _authorCatalogService.CountAsync();
            const int authorsCount = 8;
            var authorsTotal = authorsCount + actualCount;

            for (var i = 0; i < authorsCount; i++)
            {
                var authorDto = new CreateAuthorDto()
                {
                    FirstName = "test firstname",
                    LastName = "test lastname",
                };
                await _authorCatalogService.AddAsync(authorDto);
            }

            // Act
            var authorsDb = _authorCatalogService.GetAllAsync().Result.ToList();

            // Assert
            Assert.NotNull(authorsDb);
            Assert.NotEmpty(authorsDb);
            Assert.Equal(authorsTotal, authorsDb.Count);
        }

        [Fact]
        public async Task FindAsync_ReturnOk()
        {
            // Arrange
            var authorDto = new CreateAuthorDto
            {
                FirstName = "test firstname",
                LastName = "test lastname",
            };
            var authorDb = await _authorCatalogService.AddAsync(authorDto);
            var authorId = authorDb.Id;

            // Act
            var foundAuthor = await _authorCatalogService.FindAsync(authorId);

            // Assert
            Assert.NotNull(foundAuthor);
            Assert.Equal(authorDb, foundAuthor);
        }

        [Theory]
        [InlineData("firstname", "lastname")]
        [InlineData("1234567890-=<>?", "1234567890-=<>?")]
        [InlineData("a", "a")]
        [InlineData("/*-+!@#$%^&*()", "/*-+!@#$%^&*()")]
        public async Task AddAsync_ReturnOk(string firstname, string lastname)
        {
            // Arrange
            var actualCount = await _authorCatalogService.CountAsync();
            var authorsTotal = actualCount + 1;

            var authorDto = new CreateAuthorDto
            {
                FirstName = firstname,
                LastName = lastname
            };

            // Act
            var authorDb = await _authorCatalogService.AddAsync(authorDto);
            var authorsDbCount = await _authorCatalogService.CountAsync();

            // Assert
            Assert.NotNull(authorDb);
            Assert.Equal(authorDto.FirstName, authorDb.FirstName);
            Assert.Equal(authorDto.LastName, authorDb.LastName);
            Assert.Equal(authorsTotal, authorsDbCount);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg",
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")] // size of firstname & lastname > 150 chars.
        public async Task AddAsync_ThrowValidationException(string firstname, string lastname)
        {
            // Arrange
            var authorDto = new CreateAuthorDto
            {
                FirstName = firstname,
                LastName = lastname
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _authorCatalogService.AddAsync(authorDto));
        }

        [Theory]
        [InlineData("new_firstname", "new_lastname")]
        [InlineData("1234567890-=<>?", "1234567890-=<>?")]
        [InlineData("a", "a")]
        [InlineData("/*-+!@#$%^&*()", "/*-+!@#$%^&*()")]
        public async Task UpdateAsync_ReturnOk(string firstnameNew, string lastnameNew)
        {
            // Arrange
            var createAuthorDto = new CreateAuthorDto
            {
                FirstName = "firstnameTest",
                LastName = "lastnameTest"
            };

            var createAuthorDb = await _authorCatalogService.AddAsync(createAuthorDto);

            var authorId = createAuthorDb.Id;

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
            Assert.Equal(firstnameNew, updatedAuthorDb.FirstName);
            Assert.Equal(lastnameNew, updatedAuthorDb.LastName);
            Assert.NotEqual(createAuthorDto.FirstName, updatedAuthorDb.FirstName);
            Assert.NotEqual(createAuthorDto.LastName, updatedAuthorDb.LastName);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg",
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")] // size of firstname & lastname > 150 chars.
        public async Task UpdateAsync_ThrowValidationException(string firstnameNew, string lastnameNew)
        {
            // Arrange
            var createAuthorDto = new CreateAuthorDto
            {
                FirstName = "firstnameTest",
                LastName = "lastnameTest"
            };

            var createAuthorDb = await _authorCatalogService.AddAsync(createAuthorDto);

            var authorId = createAuthorDb.Id;

            var updateAuthorDto = new UpdateAuthorDto()
            {
                Id = authorId,
                FirstName = firstnameNew,
                LastName = lastnameNew
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _authorCatalogService.UpdateAsync(updateAuthorDto));
        }

        [Theory]
        [InlineData("new_firstname", "new_lastname")]
        [InlineData("1234567890-=<>?", "1234567890-=<>?")]
        [InlineData("a", "a")]
        [InlineData("/*-+!@#$%^&*()", "/*-+!@#$%^&*()")]
        public async Task DeleteAsync_ThrowDbException(string firstname, string lastname)
        {
            // Arrange
            var createAuthorDto = new CreateAuthorDto
            {
                FirstName = firstname,
                LastName = lastname
            };

            var createAuthorDb = await _authorCatalogService.AddAsync(createAuthorDto);
            var authorId = createAuthorDb.Id;

            // Act
            await _authorCatalogService.DeleteAsync(authorId);

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _authorCatalogService.FindAsync(authorId));
        }


        [Theory]
        [InlineData(4)]
        [InlineData(15)]
        [InlineData(20)]
        [InlineData(0)]
        public async Task CountAsync_ReturnOk(int authorsCount)
        {
            // Arrange
            var actualCount = await _authorCatalogService.CountAsync();

            for (var i = 0; i < authorsCount; i++)
            {
                var authorDto = new CreateAuthorDto
                {
                    FirstName = "firstnameTest",
                    LastName = "lastnameTest"
                };

                await _authorCatalogService.AddAsync(authorDto);
            }

            // Act
            var resultCountDb = await _authorCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount + authorsCount, resultCountDb);
        }
    }
}
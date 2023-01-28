using AutoMapper;
using BLL.DTO.Publisher;
using BLL.Infrastructure.Mapper;
using BLL.Infrastructure.Validators.Publisher;
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
    public class PublisherCatalogServiceTest
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IPublisherCatalogService _publisherCatalogService;

        public PublisherCatalogServiceTest()
        {
            var dbContextInMemory = DbInMemory.CreateDbContextInMemory(); // For testing DB commands inMemory DB
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            DbUtilities.InitializeDbForTests(dbContextInMemory);

            var mapper = mapperConfiguration.CreateMapper();
            var createPublisherDtoValidator = new CreatePublisherDtoValidator();
            var updatePublisherDtoValidator = new UpdatePublisherDtoValidator();

            _repositoryWrapper = new RepositoryWrapper(dbContextInMemory);
            _publisherCatalogService = new PublisherCatalogService(_repositoryWrapper, mapper, createPublisherDtoValidator, updatePublisherDtoValidator);
        }

        [Fact]
        public async Task GetAllAsync_Return_Ok()
        {
            // Arrange
            var publishersSource = await _repositoryWrapper.Publishers.GetAll().ToListAsync();

            // Act
            var publishersAll = _publisherCatalogService.GetAllAsync().Result.ToList();

            // Assert
            Assert.NotNull(publishersAll);
            publishersSource.Should().BeEquivalentTo(publishersAll);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_Return_Ok(int publisherId)
        {
            // Arrange
            var publisherActual = await _repositoryWrapper.Publishers.FindAsync(publisherId);

            // Act
            var foundedPublisher = await _publisherCatalogService.FindAsync(publisherId);

            // Assert
            Assert.NotNull(foundedPublisher);
            publisherActual.Should().BeEquivalentTo(foundedPublisher);
            Assert.Equal(publisherId, foundedPublisher.Id);
        }

        [Theory]
        [InlineData(999999)]
        public async Task FindAsync_Return_DbEntityNotFoundException(int publisherId)
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _publisherCatalogService.FindAsync(publisherId));
        }

        [Theory]
        [InlineData("name")]
        [InlineData("1234567890-=<>?")]
        [InlineData("a")]
        [InlineData("/*-+!@#$%^&*()")]
        public async Task AddAsync_Return_Ok(string name)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Publishers.CountAsync();
            var publishersTotal = actualCount + 1;

            var publisherDto = new CreatePublisherDto
            {
                Name = name
            };

            // Act
            var publisherDb = await _publisherCatalogService.AddAsync(publisherDto);
            var publishersDbCount = await _repositoryWrapper.Publishers.CountAsync();

            // Assert
            Assert.NotNull(publisherDb);
            Assert.Equal(publisherDto.Name, publisherDb.Name);
            Assert.Equal(publishersTotal, publishersDbCount);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg" +
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")] // size of name > 150 chars.
        public async Task AddAsync_Return_ValidationException(string name)
        {
            // Arrange
            var publisherDto = new CreatePublisherDto
            {
                Name = name
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _publisherCatalogService.AddAsync(publisherDto));
        }

        [Theory]
        [InlineData(1, "name")]
        [InlineData(1, "1234567890-=<>?")]
        [InlineData(1, "a")]
        [InlineData(1, "/*-+!@#$%^&*()")]
        public async Task UpdateAsync_Return_Ok(int publisherId, string name)
        {
            // Arrange
            var publisherSource = await _repositoryWrapper.Publishers.FindAsync(publisherId);

            var updatePublisherDto = new UpdatePublisherDto()
            {
                Id = publisherId,
                Name = name
            };

            // Act 
            var updatedPublisherDb = await _publisherCatalogService.UpdateAsync(updatePublisherDto);

            // Assert
            Assert.Equal(publisherId, updatedPublisherDb.Id);
            Assert.Equal(publisherSource, updatedPublisherDb); // test EF Tracking
            Assert.Equal(name, updatedPublisherDb.Name);
        }

        [Theory]
        [InlineData(9999999, "name")]
        [InlineData(9999999, "1234567890-=<>?")]
        [InlineData(9999999, "a")]
        [InlineData(9999999, "/*-+!@#$%^&*()")]
        public async Task UpdateAsync_Return_DbEntityNotFoundException(int publisherId, string name)
        {
            // Arrange
            var updatePublisherDto = new UpdatePublisherDto()
            {
                Id = publisherId,
                Name = name
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _publisherCatalogService.UpdateAsync(updatePublisherDto));
        }

        [Theory]
        [InlineData(1, "")]
        [InlineData(1, "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg" +
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pg")] // size of name > 150 chars.
        public async Task UpdateAsync_Return_ValidationException(int publisherId, string name)
        {
            // Arrange
            var updatePublisherDto = new UpdatePublisherDto()
            {
                Id = publisherId,
                Name = name
            };

            // Act & Asserts
            await Assert.ThrowsAsync<ValidationException>(() => _publisherCatalogService.UpdateAsync(updatePublisherDto));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task DeleteAsync_Return_DbException(int publisherId)
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Publishers.CountAsync();
            var publishersTotal = actualCount - 1;

            // Act
            await _publisherCatalogService.DeleteAsync(publisherId);
            var publishersDbCount = await _repositoryWrapper.Publishers.CountAsync();

            // Assert
            await Assert.ThrowsAsync<DbEntityNotFoundException>(() => _publisherCatalogService.FindAsync(publisherId));
            Assert.Equal(publishersTotal, publishersDbCount);
        }

        [Fact]
        public async Task CountAsync_Return_Ok()
        {
            // Arrange
            var actualCount = await _repositoryWrapper.Publishers.CountAsync();

            // Act
            var resultCountDb = await _publisherCatalogService.CountAsync();

            // Assert
            Assert.Equal(actualCount, resultCountDb);
        }
    }
}
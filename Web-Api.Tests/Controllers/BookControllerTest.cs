using System.Net;
using ApiBookStore;
using ApiBookStore.Controllers;
using BLL.Services.Contract;
using DLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using Web_Api.Tests.Startup;
using Xunit;

namespace Web_Api.Tests.Controllers
{
    public class BookControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly Mock<IBookCatalogService> _bookCatalogServiceMock;
        private readonly WebApplicationFactory<Program> _factory;

        public BookControllerTest(WebApplicationFactory<Program> factory)
        {
            _bookCatalogServiceMock = new Mock<IBookCatalogService>();
            _factory = factory;
        }

        [Fact]
        public async Task BookGetAllAsyncTask_ReturnOk()
        {
            // Arrange
            var testBooks = new List<Book>
            {
                new Book(),
                new Book(),
                new Book(),
                new Book()
            };

            _bookCatalogServiceMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(testBooks);

            var bookController = new BookController(_bookCatalogServiceMock.Object);

            // Act
            var result = await bookController.BookGetAllAsyncTask();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result); // Status code 200
        }

        [Fact]
        public async Task BookGetByIdAsyncTask_ReturnOk()
        {
            // Arrange
            var testBook = new Book();

            const int bookId = 5;

            _bookCatalogServiceMock.Setup(x => x.FindAsync(bookId))
                .ReturnsAsync(testBook);

            var bookController = new BookController(_bookCatalogServiceMock.Object);

            // Act
            var result = await bookController.BookGetByIdAsyncTask(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result); // Status code 200
        }

        [Theory]
        [InlineData("/api/Book")]
        public async Task BookGetAllAsyncTask_ReturnOk_TestByMicrosoft(string url)
        {
            // Arranges
            var factory = new WebApplicationFactoryTest<Program>();
            var client = factory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:8000/");

            // Act
            var response = await client.GetAsync(url);
            
            // Assert
            response.EnsureSuccessStatusCode(); // Status code 200-299
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/api/Book")]
        public async Task BookGetAllAsyncTask_ReturnOk_Test3(string url)
        {
            // Arranges
            var webAppFactory = new WebApplicationFactory<Program>();

            var httpClient = webAppFactory.CreateClient();

            // Act
            var response = await httpClient.GetAsync(url);

            // Assert
            Assert.IsType<OkObjectResult>(response); // Status code 200
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }
        

        // TODO: Realize tests for Create, Update, Delete methods!!! 
    }
}
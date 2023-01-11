using ApiBookStore.Controllers;
using BLL.Services.Contract;
using DLL.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Web_Api.Tests.Controllers
{
    public class BookControllerTest
    {
        private readonly Mock<IBookCatalogService> _bookCatalogServiceMock;

        public BookControllerTest()
        {
            _bookCatalogServiceMock = new Mock<IBookCatalogService>();
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

        public async Task BookCreateAsyncTask_ReturnOk()
        {
            // Arrange


            // Act


            // Assert

            throw new NotImplementedException();
        }


        // TODO: Realize tests for Create, Update, Delete methods!!! 
    }
}
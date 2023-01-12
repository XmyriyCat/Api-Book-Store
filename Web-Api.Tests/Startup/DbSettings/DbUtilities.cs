using DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_Api.Tests.Startup.DbSettings
{
    public static class DbUtilities
    {
        public static void InitializeDbForTests(DbContext db)
        {
            db.Set<Book>().AddRange(GetTestsBooks());
            db.SaveChanges();
        }

        private static IEnumerable<Book> GetTestsBooks() // Adding Books, Publishers, Genres and Authors
        {
            return new List<Book>
            {
                new Book
                {
                    Price = 80.55m,
                    Name = "TestBook-1",
                    Publisher = new Publisher { Name = "test-publisher-1"},
                    Genres = new List<Genre> { new Genre { Name = "testGenre-1" } },
                    Authors = new List<Author> { new Author { FirstName = "Test-firstname", LastName = "Test-lastname" } }
                },
                new Book
                {
                    Price = 188.27m,
                    Name = "TestBook-2",
                    Publisher = new Publisher { Name = "test-publisher-2"},
                    Genres = new List<Genre> { new Genre { Name = "testGenre-2" }, new Genre{Name = "testGenre-3"} },
                    Authors = new List<Author> { new Author { FirstName = "Test-firstname-2", LastName = "Test-lastname-2" } }
                }
            };
        }
    }
}

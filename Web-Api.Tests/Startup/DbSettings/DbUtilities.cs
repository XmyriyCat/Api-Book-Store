using DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_Api.Tests.Startup.DbSettings
{
    public static class DbUtilities
    {
        public static void InitializeDbForTests(DbContext db)
        {
            db.Set<Book>().AddRange(GetTestsBooks());
            db.Set<Order>().AddRange(GetTestsOrders());
            db.Set<OrderLine>().AddRange(GetTestOrderLine());
            db.Set<User>().AddRange(GetTestUsers());
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
                },
                new Book
                {
                    Price = 653.12m,
                    Name = "TestBook-3",
                    Publisher = new Publisher { Name = "test-publisher-3"},
                    Genres = new List<Genre> { new Genre { Name = "testGenre-4" }},
                    Authors = new List<Author> { new Author { FirstName = "Test-firstname-3", LastName = "Test-lastname-3" } }
                }
            };
        }

        private static IEnumerable<Order> GetTestsOrders() // Adding Orders, Shipments, Users, Deliveries, PaymentWays
        {
            return new List<Order>
            {
                new Order
                {
                    TotalPrice = 4532.12M,
                    OrderDate = DateTime.Now,
                    Shipment = new Shipment
                    {
                        Delivery = new Delivery { Name = "test-delivery", Price = 0 },
                        PaymentWay = new PaymentWay { Name = "test-paymentWay" },
                    },
                    User = new User
                    {
                        UserName = "test-username",
                        PasswordHash = "some-test-hash",
                        Login = "test-login",
                        Country = "test-country",
                        City = "test-city",
                        Address = "test-address"
                    }
                },
                new Order
                {
                    TotalPrice = 999999999.99999M,
                    OrderDate = DateTime.Now,
                    Shipment = new Shipment
                    {
                        Delivery = new Delivery { Name = "test-delivery-2", Price = 99999.4632M },
                        PaymentWay = new PaymentWay { Name = "test-paymentWay-2" }
                    },
                    User = new User
                    {
                        UserName = "test-username-2",
                        PasswordHash = "some-test-hash-2",
                        Login = "test-login-2",
                        Country = "test-country-2",
                        City = "test-city-2",
                        Address = "test-address-2"
                    }
                }
            };
        }

        private static IEnumerable<OrderLine> GetTestOrderLine() // Adding OrderLines, WarehouseBooks, Warehouses, Orders, Books
        {
            return new List<OrderLine>
            {
                new OrderLine
                {
                    Quantity = 4,
                    Order = new Order
                    {
                        TotalPrice = 156156,
                        OrderDate = DateTime.Now,
                        Shipment = new Shipment
                        {
                            Delivery = new Delivery { Name = "test-delivery", Price = 0 },
                            PaymentWay = new PaymentWay { Name = "test-paymentWay" },
                        },
                        User = new User
                        {
                            UserName = "test-username",
                            PasswordHash = "some-test-hash",
                            Login = "test-login",
                            Country = "test-country",
                            City = "test-city",
                            Address = "test-address"
                        }
                    },
                    WarehouseBook = new WarehouseBook
                    {
                        Quantity = 20,
                        Warehouse = new Warehouse
                        {
                            Name = "test-name",
                            Address = "test-address",
                            City = "test-city",
                            Country = "test-country",
                            PhoneNumber = "+375(17)177-77-79"
                        },
                        Book = new Book
                        {
                            Price = 99.99m,
                            Name = "TestBook",
                            Publisher = new Publisher { Name = "test-publisher"},
                            Genres = new List<Genre> { new Genre { Name = "testGenre" } },
                            Authors = new List<Author> { new Author { FirstName = "Test-firstname", LastName = "Test-lastname" } }
                        }
                    }
                },
                new OrderLine
                {
                    Quantity = 6,
                    Order = new Order
                    {
                        TotalPrice = 654123,
                        OrderDate = DateTime.Now,
                        Shipment = new Shipment
                        {
                            Delivery = new Delivery { Name = "test-delivery", Price = 999 },
                            PaymentWay = new PaymentWay { Name = "test-paymentWay" },
                        },
                        User = new User
                        {
                            UserName = "test-username",
                            PasswordHash = "some-test-hash",
                            Login = "test-login",
                            Country = "test-country",
                            City = "test-city",
                            Address = "test-address"
                        }
                    },
                    WarehouseBook = new WarehouseBook
                    {
                        Quantity = 70,
                        Warehouse = new Warehouse
                        {
                            Name = "test-name",
                            Address = "test-address",
                            City = "test-city",
                            Country = "test-country",
                            PhoneNumber = "+375(17)177-77-79"
                        },
                        Book = new Book
                        {
                            Price = 99.99m,
                            Name = "TestBook",
                            Publisher = new Publisher { Name = "test-publisher"},
                            Genres = new List<Genre> { new Genre { Name = "testGenre" } },
                            Authors = new List<Author> { new Author { FirstName = "Test-firstname", LastName = "Test-lastname" } }
                        }
                    }
                }
            };
        }

        private static IEnumerable<User> GetTestUsers() // Adding Users, Orders, Roles
        {
            return new List<User>
            {
                new User
                {
                    UserName = "test-username",
                    PasswordHash = "some-test-hash",
                    Login = "test-login",
                    Country = "test-country",
                    City = "test-city",
                    Address = "test-address",
                    Orders = new List<Order>
                    {
                        new Order
                        {
                            TotalPrice = 4532.12M,
                            OrderDate = DateTime.Now,
                            Shipment = new Shipment
                            {
                                Delivery = new Delivery { Name = "test-delivery", Price = 0 },
                                PaymentWay = new PaymentWay { Name = "test-paymentWay" },
                            }
                        }
                    },
                    Roles = new List<Role> { new Role { Name = "Buyer" } }
                },
                new User
                {
                    UserName = "test-username-2",
                    PasswordHash = "some-test-hash-2",
                    Login = "test-login-2",
                    Country = "test-country-2",
                    City = "test-city-2",
                    Address = "test-address-2",
                    Roles = new List<Role> { new Role { Name = "Manager" } }
                }
            };
        }
    }
}

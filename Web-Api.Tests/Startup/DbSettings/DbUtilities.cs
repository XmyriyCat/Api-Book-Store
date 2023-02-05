﻿using Bogus;
using DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_Api.Tests.Startup.DbSettings
{
    public static class DbUtilities
    {
        public static void InitializeDbForTests(DbContext db)
        {
            db.Set<Book>().AddRange(GetTestsBooks(10));
            db.Set<Order>().AddRange(GetTestsOrders(10));
            db.Set<OrderLine>().AddRange(GetTestOrderLines(10));
            db.Set<User>().AddRange(GetTestUsers(10));
            db.Set<WarehouseBook>().AddRange(GetTestWarehouseBooks(10));
            db.Set<Shipment>().AddRange(GetTestShipments(10));
            db.SaveChanges();
        }

        private static IEnumerable<Book> GetTestsBooks(int count) // Adding Books, Publishers, Genres and Authors
        {
            if (count <= 0)
            {
                return Enumerable.Empty<Book>();
            }

            var faker = new Faker<Book>()
                .RuleFor(x => x.Price, f => f.Random.Decimal())
                .RuleFor(x => x.Name, f => f.Random.String2(10, 50))
                .RuleFor(x => x.Publisher, f => new Publisher
                {
                    Name = f.Random.String2(10, 50)
                })
                .RuleFor(x => x.Genres, f => new List<Genre>
                {
                    new Genre
                    {
                        Name = f.Random.String2(10, 50)
                    }
                })
                .RuleFor(x => x.Authors, f => new List<Author>
                {
                    new Author
                    {
                        FirstName = f.Name.FirstName(),
                        LastName = f.Name.LastName()
                    }
                });

            var books = faker.Generate(count);

            return books;
        }

        private static IEnumerable<Order> GetTestsOrders(int count) // Adding Orders, Shipments, Users, Deliveries, PaymentWays
        {
            if (count <= 0)
            {
                return Enumerable.Empty<Order>();
            }

            var faker = new Faker<Order>()
                .RuleFor(x => x.TotalPrice, f => f.Random.Decimal())
                .RuleFor(x => x.OrderDate, f => f.Date.Between(new DateTime(2000, 1, 1), DateTime.Now))
                .RuleFor(x => x.Shipment, f => new Shipment
                {
                    Delivery = new Delivery
                    {
                        Name = f.Random.String2(10, 50),
                        Price = f.Random.Decimal()
                    },
                    PaymentWay = new PaymentWay
                    {
                        Name = f.Random.String2(10, 50)
                    }
                })
                .RuleFor(x => x.User, f => new User()
                {
                    UserName = f.Random.String2(10, 50),
                    PasswordHash = f.Random.String2(10, 50),
                    Login = f.Random.String2(10, 50),
                    Country = f.Random.String2(10, 50),
                    City = f.Random.String2(10, 50),
                    Address = f.Random.String2(10, 50)
                });

            var orders = faker.Generate(count);

            return orders;
        }

        private static IEnumerable<OrderLine> GetTestOrderLines(int count) // Adding OrderLines, WarehouseBooks, Warehouses, Orders, Books
        {
            if (count <= 0)
            {
                return Enumerable.Empty<OrderLine>();
            }

            var faker = new Faker<OrderLine>()
                .RuleFor(x => x.Quantity, f => f.Random.Int(1))
                .RuleFor(x => x.Order, f => new Order()
                {
                    TotalPrice = f.Random.Decimal(),
                    OrderDate = f.Date.Between(new DateTime(2000, 1, 1), DateTime.Now),
                    Shipment = new Shipment
                    {
                        Delivery = new Delivery
                        {
                            Name = f.Random.String2(10, 50),
                            Price = f.Random.Decimal()
                        },
                        PaymentWay = new PaymentWay
                        {
                            Name = f.Random.String2(10, 50)
                        },
                    },
                    User = new User
                    {
                        UserName = f.Random.String2(10, 50),
                        PasswordHash = f.Random.String2(10, 50),
                        Login = f.Random.String2(10, 50),
                        Country = f.Random.String2(10, 50),
                        City = f.Random.String2(10, 50),
                        Address = f.Random.String2(10, 50)
                    }
                })
                .RuleFor(x => x.WarehouseBook, f => new WarehouseBook
                {
                    Quantity = f.Random.Int(1),
                    Warehouse = new Warehouse
                    {
                        Name = f.Random.String2(10, 50),
                        Address = f.Random.String2(10, 50),
                        City = f.Random.String2(10, 50),
                        Country = f.Random.String2(10, 50),
                        PhoneNumber = f.Phone.PhoneNumber("+375(17)###-##-##")
                    },
                    Book = new Book
                    {
                        Price = f.Random.Decimal(),
                        Name = f.Random.String2(10, 50),
                        Publisher = new Publisher
                        {
                            Name = f.Random.String2(10, 50)
                        },
                        Genres = new List<Genre>
                        {
                            new Genre
                            {
                                Name = f.Random.String2(10, 50)
                            }
                        },
                        Authors = new List<Author>
                        {
                            new Author
                            {
                                FirstName = f.Name.FirstName(),
                                LastName = f.Name.LastName()
                            }
                        }
                    }
                });

            var orderLines = faker.Generate(count);

            return orderLines;
        }

        private static IEnumerable<User> GetTestUsers(int count) // Adding Users, Orders, Roles
        {
            if (count <= 0)
            {
                return Enumerable.Empty<User>();
            }

            var faker = new Faker<User>()
                .RuleFor(x => x.UserName, f => f.Name.FullName())
                .RuleFor(x => x.PasswordHash, f => f.Random.String2(10, 50))
                .RuleFor(x => x.Login, f => f.Random.String2(10, 50))
                .RuleFor(x => x.Country, f => f.Address.Country())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Address, f => f.Address.StreetAddress(true))
                .RuleFor(x => x.Orders, f => new List<Order>
                {
                    new Order
                    {
                        TotalPrice = f.Random.Decimal(),
                        OrderDate = f.Date.Between(new DateTime(2000, 1, 1), DateTime.Now),
                        Shipment = new Shipment
                        {
                            Delivery = new Delivery
                            {
                                Name = f.Random.String2(10, 50),
                                Price = f.Random.Decimal()
                            },
                            PaymentWay = new PaymentWay
                            {
                                Name = f.Random.String2(10, 50)
                            },
                        }
                    }
                })
                .RuleFor(x => x.Roles, f => new List<Role>
                {
                    new Role { Name = f.Random.String2(10, 50) }
                });

            var users = faker.Generate(count);

            return users;
        }

        private static IEnumerable<WarehouseBook> GetTestWarehouseBooks(int count) // Adding WarehouseBook, Warehouse, Book
        {
            if (count <= 0)
            {
                return Enumerable.Empty<WarehouseBook>();
            }

            var faker = new Faker<WarehouseBook>()
                .RuleFor(x => x.Quantity, f => f.Random.Int(1))
                .RuleFor(x => x.Warehouse, f => new Warehouse
                {
                    Name = f.Random.String2(10, 50),
                    Address = f.Random.String2(10, 50),
                    City = f.Random.String2(10, 50),
                    Country = f.Random.String2(10, 50),
                    PhoneNumber = f.Phone.PhoneNumber("+375(17)###-##-##")

                })
                .RuleFor(x => x.Book, f => new Book
                {
                    Price = f.Random.Decimal(),
                    Name = f.Random.String2(10, 50),
                    Publisher = new Publisher
                    {
                        Name = f.Random.String2(10, 50)
                    },
                    Genres = new List<Genre>
                    {
                        new Genre
                        {
                            Name = f.Random.String2(10, 50)
                        }
                    },
                    Authors = new List<Author>
                    {
                        new Author
                        {
                            FirstName = f.Name.FirstName(),
                            LastName = f.Name.LastName()
                        }
                    }
                });

            var warehouseBooks = faker.Generate(count);

            return warehouseBooks;
        }

        private static IEnumerable<Shipment> GetTestShipments(int count)
        {
            if (count <= 0)
            {
                return Enumerable.Empty<Shipment>();
            }

            var faker = new Faker<Shipment>()
                .RuleFor(x => x.Delivery, f => new Delivery
                {
                    Name = f.Random.String2(10, 50),
                    Price = f.Random.Decimal()
                })
                .RuleFor(x => x.PaymentWay, f => new PaymentWay
                {
                    Name = f.Random.String2(10, 50)
                });

            var shipments = faker.Generate(count);

            return shipments;
        }
    }
}

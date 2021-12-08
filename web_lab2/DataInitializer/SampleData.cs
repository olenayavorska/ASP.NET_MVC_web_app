using System.Collections.Generic;
using System.IO;
using System.Linq;
using web_lab2.Models;

namespace web_lab2.DataInitializer
{
    public static class SampleData
    {
        private const string AdminRole = "Admin";
        private const string CustomerRole = "Customer";

        public static void Initialize(DatabaseContext ctx)
        {
            var books = CreateBooks();
            var roles = CreateRoles();
            var images = GetProfileImages();
            var users = CreateUsers(roles);
            var sages = CreateSages(books, images);
            var orders = CreateOrders(books, users);

            if (!ctx.Books.Any())
            {
                ctx.Books.AddRange(books);
            }

            if (!ctx.Roles.Any())
            {
                ctx.Roles.AddRange(roles);
            }

            if (!ctx.Users.Any())
            {
                ctx.Users.AddRange(users);
            }

            if (!ctx.Sages.Any())
            {
                ctx.Sages.AddRange(sages);
            }

            if (!ctx.Orders.Any())
            {
                ctx.Orders.AddRange(orders);
            }

            ctx.SaveChanges();
        }

        private static List<Order> CreateOrders(List<Book> books, List<User> users)
        {
            return new[]
            {
                new Order
                {
                    Customer = users[1],
                    OrdersDetails = new[]
                    {
                        new OrdersBooks {Book = books[0], Number = 1}
                    }.ToList()
                },
                new Order
                {
                    Customer = users[1],
                    OrdersDetails = new[]
                    {
                        new OrdersBooks {Book = books[0], Number = 1},
                        new OrdersBooks {Book = books[2], Number = 2},
                        new OrdersBooks {Book = books[5], Number = 4},
                        new OrdersBooks {Book = books[1], Number = 1}
                    }.ToList()
                },
                new Order
                {
                    Customer = users[2],
                    OrdersDetails = new[]
                    {
                        new OrdersBooks {Book = books[0], Number = 1},
                        new OrdersBooks {Book = books[3], Number = 1},
                        new OrdersBooks {Book = books[4], Number = 2}
                    }.ToList()
                },
                new Order
                {
                    Customer = users[3],
                    OrdersDetails = new[]
                    {
                        new OrdersBooks {Book = books[1], Number = 2},
                        new OrdersBooks {Book = books[5], Number = 2}
                    }.ToList()
                },
                new Order
                {
                    Customer = users[3],
                    OrdersDetails = new[]
                    {
                        new OrdersBooks {Book = books[0], Number = 1},
                        new OrdersBooks {Book = books[2], Number = 1}
                    }.ToList()
                }
            }.ToList();
        }

        private static List<Book> CreateBooks()
        {
            return new[]
            {
                new Book
                {
                    Name = "Traitor of wood",
                    Description = "Hope springs from the knowledge that there is light even in the darkest of shadows."
                },
                new Book
                {
                    Name = "Captains And Invaders",
                    Description = "Judge your day by the seeds you plant for tomorrow"
                },
                new Book
                {
                    Name = "Enemies And Beasts",
                    Description =
                        "Desire is the seed from which all achievements are harvested. The starting point of all achievement is desire. (Napoleon Hill)"
                },
                new Book
                {
                    Name = "Failure Of The New Age",
                    Description =
                        "Be the sunshine on someone's rainy day. Try to be a rainbow in someone's cloud. (Maya Angelou)"
                },
                new Book
                {
                    Name = "Defenseless Against A Nuclear Winter",
                    Description =
                        "It's not the words you speak, but the way you say them that matters. People may hear your words, but they feel your attitude. (John C. Maxwell)"
                },
                new Book
                {
                    Name = "Failure Of Stardust",
                    Description =
                        "All it takes to change your world is to change the way you think. Change your thoughts and you change your world. (Norman Vincent Peale)"
                }
            }.ToList();
        }

        private static List<Role> CreateRoles()
        {
            return new[]
            {
                new Role {Name = AdminRole},
                new Role {Name = CustomerRole}
            }.ToList();
        }

        private static List<User> CreateUsers(List<Role> allRoles)
        {
            return new[]
            {
                new User
                {
                    Username = "OnlyAdmin",
                    Password = "OnlyAdmin",
                    Roles = allRoles.Where(r => r.Name == AdminRole).ToList()
                },
                new User
                {
                    Username = "Admin",
                    Password = "Admin",
                    Roles = allRoles
                },
                new User
                {
                    Username = "Makar",
                    Password = "Makar123",
                    Roles = allRoles.Where(r => r.Name == CustomerRole).ToList()
                },
                new User
                {
                    Username = "Sofia",
                    Password = "Sofia123",
                    Roles = allRoles.Where(r => r.Name == CustomerRole).ToList()
                },
                new User
                {
                    Username = "Michael",
                    Password = "Michael123",
                    Roles = allRoles.Where(r => r.Name == CustomerRole).ToList()
                }
            }.ToList();
        }

        private static List<Sage> CreateSages(List<Book> books, List<byte[]> images)
        {
            return new[]
            {
                new Sage
                {
                    Name = "Louise D. Saunders",
                    Age = 26,
                    Photo = images[0],
                    City = "Morris",
                    Books = new[] {books[0], books[1], books[4]}.ToList()
                },
                new Sage
                {
                    Name = "Brandi J. Rubalcava",
                    Age = 45,
                    Photo = images[1],
                    City = "Concord",
                    Books = new[] {books[1], books[2], books[3]}.ToList()
                },
                new Sage
                {
                    Name = "Craig V. Bates",
                    Age = 30,
                    Photo = images[2],
                    City = "San Diego",
                    Books = new[] {books[5]}.ToList()
                },
                new Sage
                {
                    Name = "John J. Friend",
                    Age = 22,
                    Photo = images[3],
                    City = "Osula",
                    Books = new[] {books[1], books[3], books[4]}.ToList()
                }
            }.ToList();
        }

        private static List<byte[]> GetProfileImages()
        {
            var files = Directory.GetFiles("DataInitializer/profiles", "*.jpg");

            return files.Select(File.ReadAllBytes).ToList();
        }
    }
}
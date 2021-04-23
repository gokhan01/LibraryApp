using LibraryApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (context.Authors.Any())
                {
                    return;   // DB has been seeded
                }

                context.Authors.AddRange(
                    new Author
                    {
                        Name = "İlber",
                        Surname = "Ortaylı",
                        Books = new List<Book>
                        {
                            new Book
                            {
                                Name = "Gazi Mustafa Kemal Atatürk",
                                Quantity = 10
                            },
                            new Book
                            {
                                Name = "Yakın Tarihin Gerçekleri",
                                Quantity = 10
                            }
                        }
                    },
                    new Author
                    {
                        Name = "Halil",
                        Surname = "İnalcık",
                        Books = new List<Book>
                        {
                            new Book
                            {
                                Name = "Doğu Batı / Makaleler 1",
                                Quantity = 10
                            },
                            new Book
                            {
                                Name = "Osmanlı ve Modern Türkiye",
                                Quantity = 10
                            }
                        }
                    },
                    new Author
                    {
                        Name = "Selim",
                        Surname = "Erdoğan",
                        Books = new List<Book>
                        {
                            new Book
                            {
                                Name = "Sakarya",
                                Quantity = 10
                            },
                            new Book
                            {
                                Name = "Büyük Taarruz",
                                Quantity = 10
                            }
                        }
                    }
                );

                context.Members.Add(new Member
                {
                    Name = "Gökhan",
                    Surname = "Uyar",
                    EMail = "gokhanuyar01@gmail.com",
                    PhoneNumber = "05365254747"
                });
                context.SaveChanges();
            }
        }
    }
}

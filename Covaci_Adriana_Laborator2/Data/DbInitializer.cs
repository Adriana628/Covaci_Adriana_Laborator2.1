using Microsoft.EntityFrameworkCore;
using Covaci_Adriana_Laborator2.Models;

namespace Covaci_Adriana_Laborator2.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LibraryContext(serviceProvider.GetRequiredService<DbContextOptions<LibraryContext>>()))
            {
                if (context.Book.Any())
                {
                    return; // Baza de date a fost deja populată
                }

                // Autori
                var authors = new[]
                {
                    new Author { FirstName = "Mihail", LastName = "Sadoveanu" },
                    new Author { FirstName = "George", LastName = "Călinescu" },
                    new Author { FirstName = "Mircea", LastName = "Eliade" }
                };
                context.Author.AddRange(authors);

                // Genuri
                var genres = new[]
                {
                    new Genre { Name = "Roman" },
                    new Genre { Name = "Nuvelă" },
                    new Genre { Name = "Poezie" }
                };
                context.Genre.AddRange(genres);

                // Cărți
                var books = new[]
                {
                    new Book { Title = "Baltagul", Author = authors[0], Genre = genres[0], Price = 22 },
                    new Book { Title = "Enigma Otiliei", Author = authors[1], Genre = genres[0], Price = 18 },
                    new Book { Title = "Maitreyi", Author = authors[2], Genre = genres[0], Price = 27 }
                };
                context.Book.AddRange(books);

                context.SaveChanges();
            }
        }
    }
}

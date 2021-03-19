namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
            Console.WriteLine(RemoveBooks(db));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context.Books
                .ToList()
                .Where(x => x.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .ToArray() // Can't test without this
                .Where(x => x.EditionType.ToString() == nameof(EditionType.Gold) && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(x =>  x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(x => new { x.Title, x.Price })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate == null || x.ReleaseDate.Value.Year != year )
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {

            string[] inputs = input.Split().Select(x => x.ToLower()).ToArray();
            var books = context.Books
                .Include(x => x.BookCategories)
                .ThenInclude(x => x.Category)
                .ToArray()
                .Where(x => inputs.Any(y => x.BookCategories.Any(c => c.Category.Name.ToLower() == y)))
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToList();



            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var books = context.Books
               .Where(x => x.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture) )
               .OrderByDescending(x => x.ReleaseDate)
               .Select(x => new { x.Title, x.EditionType, x.Price })
               .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
               .Where(x => x.FirstName.EndsWith(input))
               .Select(x => new { FullName = x.FirstName + " " + x.LastName})
               .OrderBy(x => x.FullName)
               .ToList();

            var sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine(author.FullName);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
               .Where(x => x.Title.ToLower().Contains(input.ToLower()))
               .Select(x => x.Title)
               .OrderBy(x => x)
               .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
               .OrderBy(x => x.BookId)
               .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
               .Select(x => new { x.Title, FullName = x.Author.FirstName + " " + x.Author.LastName })
               .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.FullName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var booksCount = context.Books
               .Where(x => x.Title.Length > lengthCheck)
               .Count();


            return booksCount;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
               .Select(x => new {  FullName = x.FirstName + " " + x.LastName, Count = x.Books.Sum(x => x.Copies) })
               .OrderByDescending(x => x.Count)
               .ToList();

            var sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FullName} - {author.Count}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
              .Select(x => new { x.Name, TotalProfit = x.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies) })
              .OrderByDescending(x => x.TotalProfit)
              .ThenBy(x => x.Name)
              .ToList();

            var sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"{category.Name} ${category.TotalProfit:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
             .Select(x => new { 
                 x.Name,
                 Books = x.CategoryBooks.OrderByDescending(b => b.Book.ReleaseDate).Take(3)
                 .Select(b => new { b.Book.Title, b.Book.ReleaseDate }) 
             })
             .OrderBy(x => x.Name)
             .ThenBy(x => x.Name)
             .ToList();

            var sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            //context.Database.ExecuteSqlRaw("UPDATE Books SET Price = Price + 5 WHERE YEAR(ReleaseDate) < 2010");
            var books = context.Books.Where(x => x.ReleaseDate.HasValue && x.ReleaseDate.Value.Year < 2010).ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }
            context.SaveChanges();

        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books.Where(x => x.Copies <= 4200).ToList();
            int removedCount = books.Count();
            foreach (var book in books)
            {
                context.Remove(book);
            }
            context.SaveChanges();
            return removedCount;
        }
    }
}

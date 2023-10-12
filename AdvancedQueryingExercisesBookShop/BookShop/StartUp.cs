namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var dbContext = new BookShopContext();
            DbInitializer.ResetDatabase(dbContext);

            var n = RemoveBooks(dbContext);
            Console.WriteLine(n);
        }

        // P02 Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext dbContext, string ageRestriction)
        {
            var books = dbContext.Books
                .ToArray()
                .Where(b => Enum.GetName(b.AgeRestriction).ToLower() == ageRestriction.ToLower())
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }


        //P03 Golden Books
        public static string GetGoldenBooks(BookShopContext dbContext)
        {
            var editionType = (EditionType)Enum.Parse(typeof(EditionType), "Gold");

            var books = dbContext.Books
                .Where(b => b.EditionType == editionType && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }


        //P04 Books by Price
        public static string GetBooksByPrice(BookShopContext dbContext)
        {
            var books = dbContext.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }


        //P05 Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext dbContext, int year)
        {
            var books = dbContext.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }


        //P06 Book Titles by Category
        public static string GetBooksByCategory(BookShopContext dbContext, string input)
        {
            var categories = input.ToLower()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToHashSet();

            var books = dbContext.Books
                .Where(b => b.BookCategories
                .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }


        //P07 Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext dbContext, string date)
        {
            var parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = dbContext.Books
                .Where(b => b.ReleaseDate < parsedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    Info = $"{b.Title} - {b.EditionType} - ${b.Price:f2}"
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Info);
            }

            return sb.ToString().TrimEnd();
        }


        //P08 Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext dbContext, string input)
        {
            var authhors = dbContext.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToArray();

            return string.Join(Environment.NewLine, authhors);
        }


        //P09 Book Search
        public static string GetBookTitlesContaining(BookShopContext dbContext, string input)
        {
            var books = dbContext.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }


        //P10 Book Search by Author
        public static string GetBooksByAuthor(BookShopContext dbContext, string input)
        {
            var authorsTwithBooks = dbContext.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();


            return string.Join(Environment.NewLine, authorsTwithBooks);
        }


        //P11 Count Books
        public static int CountBooks(BookShopContext dbContext, int lengthCheck)
            => dbContext.Books
                .Count(b => b.Title.Length > lengthCheck);


        //P12 Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext dbContext)
        {
            var authorsWithTotalBookCopies = dbContext.Authors
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}",
                    Copies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(b => b.Copies)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authorsWithTotalBookCopies)
            {
                sb.AppendLine($"{author.FullName} - {author.Copies}");
            }

            return sb.ToString().TrimEnd();
        }


        //P13 Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext dbContext)
        {
            var categoriesWithTotalProfit = dbContext.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks.Sum(b => b.Book.Copies * b.Book.Price) 
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach(var category in categoriesWithTotalProfit)
            {
                sb.AppendLine($"{category.Name} ${category.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }


        //P14 Most Recent Books
        public static string GetMostRecentBooks(BookShopContext dbContext)
        {
            var categories = dbContext.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks
                        .OrderByDescending(b => b.Book.ReleaseDate)
                        .Take(3)
                        .Select(b => $"{b.Book.Title} ({b.Book.ReleaseDate.Value.Year})")
                        .ToArray()
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach(var category in categories)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine(book);
                }
            }

            return sb.ToString().TrimEnd();
        }


        //P15 Increase Prices
        public static void IncreasePrices(BookShopContext dbContext)
        {
           var books = dbContext.Books;

            foreach (var book in books)
            {
                book.Price += 5;
            }

            dbContext.SaveChanges();
        }


        //P16 Remove Books
        public static int RemoveBooks(BookShopContext dbContext)
        {
            var books = dbContext.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            var booksRemoved = books.Length;

            dbContext.Books.RemoveRange(books);
            dbContext.SaveChanges();

            return booksRemoved;
        }
    }
}




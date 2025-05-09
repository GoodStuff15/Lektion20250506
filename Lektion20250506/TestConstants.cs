using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lektion20250506
{
    public static class TestConstants
    {

        public static List<Book> books = new List<Book>
        {
            new Book("1984", "George Orwell", "9780451524935", 1949, true, new DateTime(2025,4,30,15,44,22)),
            new Book("To Kill a Mockingbird", "Harper Lee", "9780061120084", 1960),
            new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", 1925),
            new Book("The Hobbit", "J.R.R. Tolkien", "9780547928227", 1937, true, new DateTime(2025,4,1,11,05,22)),
            new Book("Pride and Prejudice", "Jane Austen", "9780141439518", 1813),
            new Book("The Catcher in the Rye", "J.D. Salinger", "9780316769488", 1951),
            new Book("Lord of the Flies", "William Golding", "9780399501487", 1954),
            new Book("Brave New World", "Aldous Huxley", "9780060850524", 1932)
        };

        public const decimal LateFeePerDay = 0.5m;
        public const int LoanPeriodDays = 30;
    }
}

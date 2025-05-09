using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lektion20250506
{
    public class LibrarySystem
    {
        private List<Book> books;

        public LibrarySystem()
        {
            books = TestConstants.books;
            // Add some initial books
            //books.Add(new Book("1984", "George Orwell", "9780451524935", 1949, true));
            //books.Add(new Book("To Kill a Mockingbird", "Harper Lee", "9780061120084", 1960));
            //books.Add(new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", 1925));
            //books.Add(new Book("The Hobbit", "J.R.R. Tolkien", "9780547928227", 1937, true));
            //books.Add(new Book("Pride and Prejudice", "Jane Austen", "9780141439518", 1813));
            //books.Add(new Book("The Catcher in the Rye", "J.D. Salinger", "9780316769488", 1951));
            //books.Add(new Book("Lord of the Flies", "William Golding", "9780399501487", 1954));
            //books.Add(new Book("Brave New World", "Aldous Huxley", "9780060850524", 1932));

        }

        public bool AddBook(Book book)
        {
            if(book.ISBN == "" || book.ISBN == null)
            {
                return false;
            }

            if (DoesBookExistAlready(book)) 
            {
                return false;
            }
            else
            {
                books.Add(book);
                return true;
            }
        }

        public bool DoesBookExistAlready(Book book)
        {
            var ISBN = book.ISBN;

            foreach (Book b in books)
            {
                if (b.ISBN == ISBN)
                { return true; }
            }
            return false;
        }

        public bool RemoveBook(string isbn)
        {
            Book book = SearchByISBN(isbn);
            if (book != null && !book.IsBorrowed)
            {
                books.Remove(book);
                return true;
            }
            return false;
        }

        public Book SearchByISBN(string isbn)
        {
            var book = books.FirstOrDefault(b => b.ISBN == isbn);

            if (book != null)
            {
                return book;
            }
            else
            {
                return null;
            }
        }

        public List<Book>? SearchByTitle(string title)
        {
            //var booksByTitle = books.Where(b => b.Title == title).ToList();
            var booksByTitle = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();

            if (booksByTitle.Any())
            {
                return booksByTitle;
            }
            else
            {
                return new List<Book>();
            }
        }

        public List<Book>? SearchByAuthor(string author)
        {
            var booksByAuthor = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();


            if (booksByAuthor.Any())
            {
                return booksByAuthor;
            }
            else
            {
                return new List<Book>();
            } 
        }

        public bool BorrowBook(string isbn)
        {
            Book book = SearchByISBN(isbn);
            if (book != null && !book.IsBorrowed)
            {
                book.IsBorrowed = true;
                book.BorrowDate = DateTime.Now;
                return true;
            }
            return false;
        }

        public bool ReturnBook(string isbn)
        {
            Book book = SearchByISBN(isbn);
            if (book != null && book.IsBorrowed)
            {
                book.IsBorrowed = false;
                book.BorrowDate = null;
                return true;
            }
            return false;
        }

        public List<Book> GetAllBooks()
        {
            return books;
        }

        public decimal CalculateLateFee(string isbn)
        {

            Book book = SearchByISBN(isbn);
            
            if (book == null)
                return 0;

            TimeSpan daysLate = DateTime.Now - book.BorrowDate.Value - TimeSpan.FromDays(TestConstants.LoanPeriodDays);
            decimal feePerDay = 0.5m;
            return daysLate.Days * feePerDay;
        }

        public bool IsBookOverdue(string isbn)
        {
            Book book = SearchByISBN(isbn);
            if (book != null && book.IsBorrowed && book.BorrowDate.HasValue)
            {
                TimeSpan borrowedFor = DateTime.Now - book.BorrowDate.Value;
                return borrowedFor.Days > TestConstants.LoanPeriodDays;
            }
            return false;
        }
    }
}

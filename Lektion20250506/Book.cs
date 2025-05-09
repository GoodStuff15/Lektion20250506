using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lektion20250506
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PublicationYear { get; set; }
        public bool IsBorrowed { get; set; }
        public DateTime? BorrowDate { get; set; }

        public Book(string title, string author, string isbn, int publicationYear)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            PublicationYear = publicationYear;
            IsBorrowed = false;
            BorrowDate = null;
        }

        // For testing purposes
        public Book(string title, string author, string isbn, int publicationYear, bool isBorrowed)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            PublicationYear = publicationYear;
            IsBorrowed = isBorrowed;
            BorrowDate = null;
        }

        // For testing purposes
        public Book(string title, string author, string isbn, int publicationYear, bool isBorrowed, DateTime dateTime)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            PublicationYear = publicationYear;
            IsBorrowed = isBorrowed;
            BorrowDate = dateTime;
        }
    }
}

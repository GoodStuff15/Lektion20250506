using Lektion20250506;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace TestProject1;

[TestClass]
public class BookHandlingTests
{
    private List<Book>? books;

    private LibrarySystem _system;
    public BookHandlingTests()
    {
        _system = new LibrarySystem();
        books = TestConstants.books;
    }

    [TestInitialize]
    public void Setup()
    {
        books = TestConstants.books;
    }

    // Added Tests



    // Add books 
    [TestMethod]  // PASS
    public void AddBooksToCatalog_CanBookBeAddedToCatalog()
    {
        var newBook = new Book("Titel", "Författare", "12345678910", 1999);
        var expected = true;

        Assert.AreEqual(expected, _system.AddBook(newBook));
    }

    [TestMethod]
    [DataRow("")]
    public void AddBooksToCatalog_DontAddBook_WithoutISBN(string ISBN)
    {
        // Given a new book without ISBN
        var newBook = new Book("Titel", "Författare", ISBN, 1999);

        //When trying to add it to library catalog, and then searching for it
        _system.AddBook(newBook);
        var expected = _system.SearchByISBN(ISBN);

        // Then book should not exist in catalog
        Assert.AreNotEqual(expected, newBook);
    }

    [TestMethod]
    [DataRow("")] // Edge case 
    [DataRow("12345891010")]
    [DataRow("9780061120084")]
    public void AddBooksToCatalog_DontAddBook_IfISBNIsNotUnique(string ISBN)
    {
        // Given a book with ISBN that already exists in catalog
        var newBook = new Book("Titel", "Författare", ISBN, 1999);

        // When adding it to library catalog
        _system.AddBook(newBook);
        var actual = _system.GetAllBooks()
                       .Where(b => b.ISBN == ISBN)
                       .Select(b => b.ISBN)
                       .Where(b => String.IsNullOrEmpty(b) == false)
                       .ToList()
                       .Count();
                       
        var expected = 1;
        // Then only one copy should exist in catalog
        Assert.AreEqual(expected, actual);

    }


    // Remove books
    
    [TestMethod]
    [DataRow("")] // Fail
    [DataRow("12345891010")] // Fail
    [DataRow("9780061120084")] // Success
    public void RemoveBooksFromCatalog_CanBookBeRemovedFromCatalog(string ISBN)
    {
        // Given a ISBN property of a book that should be removed from catalog
        _system.RemoveBook(ISBN);
        //When checking if book is removed
        var actual = _system.SearchByISBN(ISBN);
        // Then book should not exist ( null );
        Assert.IsNull(actual);
    }

    [TestMethod]
    public void RemoveBooks_LoanedOutBooks_CannotBeRemovedFromCatalog()
    {
        // Given a book that is loaned out
        var loanedOut = books
                        .Where(b => b.IsBorrowed)
                        .FirstOrDefault();

        // Where trying to remove book from system
        var actual = _system.RemoveBook(loanedOut.ISBN); 
        
        // Then should not be able to remove book
        Assert.IsFalse(actual);
        
    }

    // Search books

    [TestMethod]
    [DataRow("12345678")] // Edge case
    [DataRow("")] // Edge case
    [DataRow("9780061120084")] // Success

    public void SearchBooks_FindByProperty_ISBN(string ISBN)
    {
        // Where seaching system for book by ISBN property
        var actual = _system.SearchByISBN(ISBN);

        // When the book exists
        Book? expected = _system.GetAllBooks()
                      .Where(b => b.ISBN == ISBN)
                      .FirstOrDefault();

        // Then the book should be found, and should not return null
        //Assert.IsNotNull(actual);
        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    [DataRow("Dassboken 3 - Skitkul")] // Edge case
    [DataRow("Lord of the Flies")] 
    [DataRow("Brave New World")]
    public void SearchBooks_FindByProperty_Title(string title)
    {
        // Given a search for books in library system, filtered by title
        var foundBooks = _system.SearchByTitle(title);

        // When getting result from the search
        List<Book>? actual = books
                            .Where(b => b.Title == title)
                            .ToList();

        // Then should return list with all books with matching titles (if there are any)
        CollectionAssert.AreEqual(actual, foundBooks);
    }

    [TestMethod]
    [DataRow("George Orwell")] // Success
    [DataRow("Göran Persson")] // Fail
    public void SearchBooks_FindByProperty_Author(string author)
    {

        // Given a search for books in library system, filtered by author
        var foundBooks = _system.SearchByAuthor(author);

        // When getting result from the search
        List<Book>? actual = books
                            .Where(b => b.Author == author)
                            .ToList();

        // Then should return list with all books with matching author (if there are any)
        Assert.IsNotNull(actual);
        Assert.IsNotNull(foundBooks);
        CollectionAssert.AreEqual(actual, foundBooks);
    }


    [TestMethod]
    [DataRow("George Orwell")]
    [DataRow("george orwell")]
    [DataRow("GEORGE ORWELL")]
    public void SearchBooks_FindByProperty_Author_IsNotCaseSensitive(string author)
    {
        // Given a search in library system, filtered by author
        var books = _system.SearchByAuthor(author);

        // When getting result from search
        var actual = books
                       .Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                       .ToList();

        // Then results should be equal independent of letter case
        CollectionAssert.AreEqual(actual, books);
    }

    [TestMethod]
    [DataRow("Pride and Prejudice")]
    [DataRow("pride and prejudice")]
    [DataRow("PRIDE AND PREJUDICE")]
    public void SearchBooks_FindByProperty_Title_IsNotCaseSensitive(string title)
    {
        // Given a search in library system, filtered by author
        var books = _system.SearchByTitle(title);

        // When getting result from search
        var actual = books
                       .Where(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                       .ToList();

        // Then results should be equal independent of letter case
        CollectionAssert.AreEqual(actual, books);
    }

    [TestMethod]
    [DataRow("Pride")]
    [DataRow("and")]
    [DataRow("P")]
    [DataRow("j")]
    [DataRow("Prejudice")]
    [DataRow("Ansgarsvar")] // Does not exist Edge Case
    public void SearchBooks_FindByProperty_Title_FindPartialMatches(string partialTitle)
    {
        // Given a search for a book by title 
        var books = _system.SearchByTitle(partialTitle);

        // Where title name contains a partial match
        List<Book>? actual = books
                       .Where(b => b.Title.Contains(partialTitle, StringComparison.OrdinalIgnoreCase))
                       .ToList();


        // Then all partially matching titles should be returned
        Assert.IsNotNull(books); // To handle edge case
        CollectionAssert.AreEqual(actual, books);
    }

    //[TestMethod]
    //[DataRow("9780061120084")]
    //[DataRow("978")]
    //[DataRow("112")]
    //[DataRow("0")]
    //[DataRow("00")]
    //public void SearchBooks_FindByProperty_ISBN_FindPartialMatches(string partialISBN)
    //{
    //    var books = _system.SearchByISBN(partialISBN); //ISBN Partial matches compiler error = only returns a single book
    //    var actual = _system.GetAllBooks()
    //                   .Where(b => b.Title.Contains(partialISBN))
    //                   .ToList();

    //    //CollectionAssert.AreEqual(actual, books);
    //}

    [TestMethod]
    [DataRow("George")]
    [DataRow("Orwell")]
    [DataRow("Geo")]
    [DataRow("well")]
    [DataRow("o")]
    public void SearchBooks_FindByProperty_Author_FindPartialMatches(string partialAuthor)
    {
        // Given a search for a book by author
        var books = _system.SearchByAuthor(partialAuthor);

        // Where author name contains a partial match
        List<Book>? actual = books
                       .Where(b => b.Author.Contains(partialAuthor,StringComparison.OrdinalIgnoreCase))
                       .ToList();

        // Then all partially matching titles should be returned
        Assert.IsNotNull(books);
        CollectionAssert.AreEqual(actual, books);
    }

    [TestCleanup]
    public void Cleanup()
    {
        books = null;
    }
}

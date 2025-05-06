using Lektion20250506;
using System.Xml.Serialization;

namespace TestProject1;

[TestClass]
public class BookHandlingTests
{
    // Add books
    //private Book _book;
    private LibrarySystem _system;
    public BookHandlingTests()
    {
        //_book = book;
        _system = new LibrarySystem();
    }

    [TestMethod]
    public void AddBooksToCatalog_CanBookBeAddedToCatalog()
    {
        var newBook = new Book("Titel", "Författare", "12345678910", 1999);
        var expected = true;

        Assert.AreEqual(expected, _system.AddBook(newBook));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("12345891010")]
    public void AddBooksToCatalog_CheckIfBookHasISBN(string ISBN)
    {
        var newBook = new Book("Titel", "Författare", ISBN, 1999);
        var expected = false;

        Assert.AreEqual(expected, _system.AddBook(newBook));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("12345891010")]
    [DataRow("9780061120084")]
    public void AddBooksToCatalog_CheckIfBookISBN_IsUnique(string ISBN)
    {
        var newBook = new Book("Titel", "Författare", ISBN, 1999);
        var expected = false;

        Assert.AreEqual(expected, _system.AddBook(newBook));
    }


    // Remove books
    
    [TestMethod]
    [DataRow("")]
    [DataRow("12345891010")]
    [DataRow("9780061120084")]
    public void RemoveBooksFromCatalog_CanBookBeRemovedFromCatalog(string ISBN)
    {
        var expected = true;
        Assert.AreEqual(expected, _system.RemoveBook(ISBN));
    }

    [TestMethod]
    [DataRow("9780061120084")]
    public void RemoveBooks_LoanedOutBooks_CannotBeRemovedFromCatalog(string ISBN)
    {
        // Given a book that is loaned out
        var books = _system.BorrowBook(ISBN);
        
        var expected = false;
        
        Assert.AreEqual(expected, _system.RemoveBook(ISBN));
    }

    // Search books

    [TestMethod]
    [DataRow("12345678")]
    [DataRow("9780061120084")]

    public void SearchBooks_FindByProperty_ISBN(string ISBN)
    {
        var foundBook = _system.SearchByISBN(ISBN);
        Book expected = null;

        Assert.AreNotEqual(expected, foundBook);
    }

    [TestMethod]
    [DataRow("Dassboken 3 - Skitkul")]
    [DataRow("1984")]
    public void SearchBooks_FindByProperty_Title(string title)
    {
        var foundBook = _system.SearchByTitle(title);
        Book expected = null;

        //Assert.AreNotEqual(expected, foundBook);
    }

    [TestMethod]
    [DataRow("George Orwell")]
    [DataRow("Göran Persson")]
    public void SearchBooks_FindByProperty_Author(string author)
    {
        var foundBook = _system.SearchByISBN(author);
        Book expected = null;

        Assert.AreNotEqual(expected, foundBook);
    }

    [TestMethod]
    [DataRow()]
    public void SearchBooks_FindByProperty_ISBN_IsNotCaseSensitive()
    {

    }

    [TestMethod]
    [DataRow("George Orwell")]
    [DataRow("george orwell")]
    [DataRow("GEORGE ORWELL")]
    public void SearchBooks_FindByProperty_Author_IsNotCaseSensitive(string author)
    {

    }

    [TestMethod]
    [DataRow("Pride And Prejudice")]
    [DataRow("pride and prejudice")]
    [DataRow("PRIDE AND PREJUDICE")]
    public void SearchBooks_FindByProperty_Title_IsNotCaseSensitive(string title)
    {

    }

    [TestMethod]
    [DataRow("Pride")]
    [DataRow("and")]
    [DataRow("P")]
    [DataRow("j")]
    [DataRow("Prejudice")]
    public void SearchBooks_FindByProperty_Title_FindPartialMatches(string partialTitle)
    {

    }

    [TestMethod]
    [DataRow("9780061120084")]
    [DataRow("978")]
    [DataRow("112")]
    [DataRow("0")]
    [DataRow("00")]
    public void SearchBooks_FindByProperty_ISBN_FindPartialMatches(string partialISBN)
    {

    }
    [TestMethod]
    [DataRow("George")]
    [DataRow("Orwell")]
    [DataRow("Geo")]
    [DataRow("well")]
    [DataRow("o")]
    public void SearchBooks_FindByProperty_Author_FindPartialMatches(string partialTitle)
    {

    }
}

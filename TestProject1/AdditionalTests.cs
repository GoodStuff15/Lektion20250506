using Lektion20250506;

namespace TestProject1;

[TestClass]
public class AdditionalTests
{
    private List<Book> _books;
    private LibrarySystem _system;
    public AdditionalTests()
    {
        _books = TestConstants.books;
        _system = new LibrarySystem();
    }

    [TestMethod]  // Additional Test: G2.2
    public void LateBook_MakeSureDaysLateAreFromSameSourceAsISBN()
    {
        // Given a book that is late
        var book = _books[3];
        TimeSpan daysLate = DateTime.Now - book.BorrowDate.Value - TimeSpan.FromDays(TestConstants.LoanPeriodDays);


        // When entering book isbn and incorrect number of days late
        //var actual = _system.CalculateLateFee(book.ISBN, 28); // Old code
        var actual = _system.CalculateLateFee(book.ISBN); // Refactored code

        var expected = daysLate.Days * TestConstants.LateFeePerDay;
        // Late fee should still be equal to number of days book is late
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]  // Additional Test: G2.2
    public void LateBook_CheckIfBookIsLate_DontCheckInvalidLoanPeriod()
    {
        // Given a book that is late
        var book = _books[3];

        // When entering book isbn and incorrect loan period

        //var actual = _system.IsBookOverdue(book.ISBN, 30); // Old code
        var actual = _system.IsBookOverdue(book.ISBN); // Refactored code
        
        // Should show that book is late
        Assert.IsTrue(actual);
    }

    [TestMethod]
    public void SearchingBooks_ByTitle_ShouldNotReturnNullList()
    {
        // Given a search for books that will return no results
        var actual = _system.SearchByTitle("asdfasdadas");
        // When using result
        List<Book> books = actual;
        // Should not be null
        Assert.IsNotNull(books);
    }

    [TestMethod]
    public void SearchingBooks_ByAuthor_ShouldNotReturnNullList()
    {
        // Given a search for books that will return no results
        var actual = _system.SearchByAuthor("asdfasdadas");
        // When using result
        List<Book> books = actual;
        // Should not be null
        Assert.IsNotNull(books);
    }

}

using Lektion20250506;

namespace TestProject1;

[TestClass]
public class LoanSystemTests
{
    private LibrarySystem _system;

    private List<Book>? books;

    public LoanSystemTests()
    {
        _system = new LibrarySystem();
        books = TestConstants.books;

    }

    [TestInitialize]
    public void Setup()
    {
        books = TestConstants.books;
    }

    // Register new loans
    [TestMethod]
    [DataRow("9780451524935")] // Loaned out
    [DataRow("9780061120084")] // Not loaned out
    public void RegisterLoan_MarkBookAsLoanedOut(string ISBN)
    {
        // Given a book that is being borrowed
        _system.BorrowBook(ISBN);
        // When looking at book status
        Book? getBook = _system.GetAllBooks()
                       .Where(b=> b.IsBorrowed)
                       .Where(b=> b.ISBN == ISBN)
                       .FirstOrDefault();
        // Then book should be marked as borrowed
        Assert.IsNotNull(getBook);
        Assert.IsTrue(getBook.IsBorrowed);
    }
    
    [TestMethod]
    [DataRow("9780451524935")] // Loaned out
    [DataRow("9780061120084")] // Not loaned out

    public void RegisterLoan_CannotRegisterLoanOfBookThatIsLoanedOut(string ISBN)
    {
        // Given a book that is loaned out
        var loanedOut = books
                        .Where(b => b.ISBN == ISBN)
                        .FirstOrDefault();
        // When trying to borrow that book

        var actual = _system.BorrowBook(loanedOut.ISBN);

        // Then it should return false, not being able to be loaned out
        Assert.IsFalse(actual);
    }

    [TestMethod]
    public void RegisterLoan_SetCorrectDateWhenLoaningOut()
    {
        // Given a loan being registered
        _system.BorrowBook(books[1].ISBN);

        // When checking date of loan
        var actual = _system.GetAllBooks()
                       .Where(b => b.ISBN == books[1].ISBN)
                       .Select(b => b.BorrowDate)
                       .FirstOrDefault()
                       .Value.Date;

        var expected = DateTime.Now.Date;

        // Then date should be the same as current date
        Assert.AreEqual(expected, actual);
        
    }

    // 2.2: Returning loans

    [TestMethod]
    public void ReturnLoan_CanBookBeReturned()
    {
        // Given a book being returned
        var book = books[1];
        _system.ReturnBook(books[1].ISBN);
        // When checking if book is marked as "not borrowed"
        var actual = _system.GetAllBooks()
                    .Where(b => b.ISBN == book.ISBN)
                    .Select(b => b.IsBorrowed)
                    .FirstOrDefault();

        // Then IsBorrowed should be false
        Assert.IsFalse(actual);
    }
    [TestMethod]
    public void ReturnLoan_IsBookLoanDateReset()
    {
        // Given a loaned out book
        var book = books[0];
        // When returning that book
        _system.ReturnBook(book.ISBN);
        var actual = _system.GetAllBooks()
               .Where(b => b.ISBN == book.ISBN)
               .Select(b => b.BorrowDate)
               .FirstOrDefault();
        // Then the BorrowDate of the book should be null
        Assert.IsNull(actual);
    }
    [TestMethod] 
    public void ReturnLoan_BookMustBeLoanedOutWhenReturning()
    {
        // Given a book that is not out on loan
        var book = books[1];
        // When book is returned while not being loaned out
        _system.ReturnBook(book.ISBN);
        var actual = _system.GetAllBooks()
                     .Where(b => b.ISBN == book.ISBN)
                     .Select(b => b.IsBorrowed)
                     .FirstOrDefault();

        // Then book should not be marked as borrowed in catalog.
        Assert.IsFalse(actual);
    }

    // Late returns
    [TestMethod]
    [DataRow(3)]
    [DataRow(30)]
    [DataRow(300)]
    public void LateBook_AreAllLateBooksIdentified(int x)
    {
        // Given a list of books that are borrowed
        var loanedOut = books
                        .Where(b => b.IsBorrowed)
                        .ToList();

        // When searching for books that have borrow dates older than X days
        var actual = _system.GetAllBooks().
                     Where(b => b.BorrowDate < DateTime.Now - TimeSpan.FromDays(x))
                     .ToList();

        var expected = books
                       .Where(b => b.BorrowDate < DateTime.Now - TimeSpan.FromDays(x))
                       .ToList();

        // Then result should show books that are late
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(3)]
    [DataRow(33)]
    [DataRow(300)]
    [DataRow(0)]
    public void LateBook_IsThisBookLate(int x)
    {
        // Given a book that has been loaned out for more than 30 days
        var loanedOut = books[3];
        // When checking if book has been loaned out longer than the loan period (X days);
        //var actual = _system.IsBookOverdue(books[3].ISBN, x); // old code
        var actual = _system.IsBookOverdue(books[3].ISBN); // refactored code

        // Then should show that book is late
        Assert.IsTrue(actual);
    }

    [TestMethod]
    public void LateBook_IsLateFeeCorrect()
    {

        // Given a book that is late
        var loanedOut = books[3];

        // When calculating current late fee of this book
        TimeSpan borrowedFor = DateTime.Now - loanedOut.BorrowDate.Value;
        //var actual = _system.CalculateLateFee(loanedOut.ISBN, borrowedFor.Days - TestConstants.LoanPeriodDays); // Old code
        var actual = _system.CalculateLateFee(loanedOut.ISBN); // Refactored code
        
        var expected = (borrowedFor.Days - TestConstants.LoanPeriodDays) * TestConstants.LateFeePerDay; 
        // Then current late fee should match late fee per day
        Assert.AreEqual(expected, actual);

    }

    [TestCleanup]
    public void Cleanup()
    {
        books = null;
    }
}

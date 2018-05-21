using System.Collections.Generic;

namespace MenedzerBiblioteki.Books
{
    interface IBookService
    {
        BookValidationResult AddBookToCatalogue(string bookName, string bookAuthor, string bookISBN);
        bool RemoveBookFromCatalogue(int bookID);
        bool RemoveBookFromCatalogue(Book bookToRemove);
        IEnumerable<Book> SearchBookByName(string bookNameToSearch);
        IEnumerable<Book> SearchBookByAuthor(string bookAuthorToSearch);
        IEnumerable<Book> SearchBookByISBN(string bookISBNToSearch);
        IEnumerable<Book> SearchBooksNotBorrowedLately(int weeks);
        IEnumerable<BorrowerListItem> ShowAllBorrowers();
        IEnumerable<Book> GetAllBooks();
        bool LendBook(Book bookToLend, string borrowerName);
        void LoadBooksCatalogue(IEnumerable<Book> bookCatalogueToLoad);
        bool ReturnBook(Book bookToReturn);
    }
}
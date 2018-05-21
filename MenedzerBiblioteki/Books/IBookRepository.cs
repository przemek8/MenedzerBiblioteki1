using System.Collections.Generic;
using static MenedzerBiblioteki.Books.BookRepository;

namespace MenedzerBiblioteki.Books
{
    interface IBookRepository
    {
        void AddBook(Book bookToAdd);
        bool LendBook(Book bookToLend, string borrowerName);
        bool RemoveBook(Book bookToRemove);
        bool RemoveBookByID(int bookID);
        IEnumerable<Book> SearchBookByName(string bookName);
        IEnumerable<Book> SearchBookByAuthor(string bookAuthor);
        IEnumerable<Book> SearchBookByISBN(string bookISBN);
        IEnumerable<Book> SearchBooksNotBorrowedLately(int weeks);
        IEnumerable<BorrowerListItem> GetBorrowersList();
        IEnumerable<Book> GetAllBooks();
        void LoadAllBooks(IEnumerable<Book> booksCatalogue);
        bool ReturnBook(Book bookToReturn);
    }
}
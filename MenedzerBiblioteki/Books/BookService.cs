using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenedzerBiblioteki.Books
{
    class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookValidator _bookValidator;

        public BookService(IBookRepository bookRepository, IBookValidator bookValidator)
        {
            _bookRepository = bookRepository;
            _bookValidator = bookValidator;
        }
        
        public BookValidationResult AddBookToCatalogue(string bookName, string bookAuthor, string bookISBN)
        {
            Book bookBeingAdded = new Book()
            {
                Name = bookName,
                Author = bookAuthor,
                ISBN = bookISBN,
                LastLendingTime = new DateTime(2000,1,1),
                NameOfCurrentHolder = null
            };
            BookValidationResult validationResult = _bookValidator.ValidateBook(bookBeingAdded);
            if (validationResult.ErrorList.Count() == 0) _bookRepository.AddBook(bookBeingAdded);
            return validationResult;
        }

        public bool RemoveBookFromCatalogue(int bookID)
        {
            return _bookRepository.RemoveBookByID(bookID);
        }

        public bool RemoveBookFromCatalogue(Book bookToRemove)
        {
            return _bookRepository.RemoveBook(bookToRemove);
        }

        public IEnumerable<Book> SearchBookByName(string bookNameToSearch)
        {
            return _bookRepository.SearchBookByName(bookNameToSearch);
        }

        public IEnumerable<Book> SearchBookByAuthor(string bookAuthorToSearch)
        {
            return _bookRepository.SearchBookByAuthor(bookAuthorToSearch);
        }

        public IEnumerable<Book> SearchBookByISBN(string bookISBNToSearch)
        {
            return _bookRepository.SearchBookByISBN(bookISBNToSearch);
        }

        public IEnumerable<Book> SearchBooksNotBorrowedLately(int weeks)
        {
            return _bookRepository.SearchBooksNotBorrowedLately(weeks);
        }

        public IEnumerable<BorrowerListItem> ShowAllBorrowers()
        {
            return _bookRepository.GetBorrowersList();
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public void LoadBooksCatalogue(IEnumerable<Book> bookCatalogueToLoad)
        {
            _bookRepository.LoadAllBooks(bookCatalogueToLoad);
        }

        public bool LendBook(Book bookToLend, string borrowerName)
        {
            return _bookRepository.LendBook(bookToLend, borrowerName);
        }

        public bool ReturnBook(Book bookToReturn)
        {
            return _bookRepository.ReturnBook(bookToReturn);
        }
    }
}

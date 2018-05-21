using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenedzerBiblioteki.Books
{
    class BookRepository : IBookRepository
    {
        private static List<Book> _books = new List<Book>
        {
            //new Book
            //{
            //    Name = "Zasady trakcji elektrycznej",
            //    Author = "Jan Podoski",
            //    ISBN = "1234567890123",
            //    LastLendingTime = new DateTime(2018, 4, 13),
            //    NameOfCurrentHolder = "Jan Kowalski"
            //},
            //new Book
            //{
            //    Name = "C# Rusz głową",
            //    Author = "Jennifer Greene, Andrew Stellman",
            //    ISBN = "7894561230951",
            //    LastLendingTime = new DateTime(2018, 3, 10),
            //    NameOfCurrentHolder = "Frank Martin"
            //},
            //new Book
            //{
            //    Name = "Zakopane odkopane",
            //    Author = "Paulina Młynarska, Beata Sabała-Zielińska",
            //    ISBN = "7778889990456",
            //    LastLendingTime = new DateTime(2018, 5, 12),
            //    NameOfCurrentHolder = "Jan Kowalski"
            //},
            //new Book
            //{
            //    Name = "Podaj piłkę dzieciom. Autostopem do Birmy",
            //    Author = "Mateusz Koszela",
            //    ISBN = "8884443330741",
            //    LastLendingTime = new DateTime(2018, 5, 16),
            //    NameOfCurrentHolder = "Jan Kowalski"
            //}
        };


        public void AddBook(Book bookToAdd)
        {
            _books.Add(bookToAdd);
        }

        public bool RemoveBook(Book bookToRemove)
        {
            return _books.Remove(bookToRemove);
        }

        public bool RemoveBookByID(int bookID)
        {
            if (bookID > _books.Count) return false;
            _books.RemoveAt(bookID - 1);
            return true;
        }

        public IEnumerable<Book> SearchBookByName(string bookName)
        {
            return _books.Where(x => x.Name.Contains(bookName));
        }

        public IEnumerable<Book> SearchBookByAuthor(string bookAuthor)
        {
            return _books.Where(x => x.Author.Contains(bookAuthor));
        }

        public IEnumerable<Book> SearchBookByISBN(string bookISBN)
        {
            return _books.Where(x => x.ISBN.Contains(bookISBN));
        }

        public IEnumerable<Book> SearchBooksNotBorrowedLately(int weeks)
        {
            return _books.Where(x => x.LastLendingTime <= DateTime.Now.AddDays(-7 * weeks));
        }

        public bool LendBook(Book bookToLend, string borrowerName)
        {
            if (!_books.Contains(bookToLend)||bookToLend.NameOfCurrentHolder!=null)
                return false;
            bookToLend.NameOfCurrentHolder = borrowerName;
            bookToLend.LastLendingTime = DateTime.Now;
            return true;
        }

        public bool ReturnBook(Book bookToReturn)
        {
            if (!_books.Contains(bookToReturn) || bookToReturn.NameOfCurrentHolder == null)
                return false;
            bookToReturn.NameOfCurrentHolder = null;
            return true;
        }

        public IEnumerable<BorrowerListItem> GetBorrowersList()
        {
            return from book in _books
                   group book by book.NameOfCurrentHolder into borrowers
                   select new BorrowerListItem() { NameOfBorrower = borrowers.Key, NumberOfBooksBorrowed = borrowers.Count(), }; ;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _books;
        }

        public void LoadAllBooks(IEnumerable<Book> booksCatalogue)
        {
            _books = booksCatalogue.ToList();
        }
    }
}

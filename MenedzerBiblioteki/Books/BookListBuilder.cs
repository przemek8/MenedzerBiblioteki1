using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenedzerBiblioteki.Books
{
    class BookListBuilder : IBookListBuilder
    {
        private readonly IEnumerable<Book> _bookList;
        public int ListSize { get; }

        public BookListBuilder(IEnumerable<Book> bookList)
        {
            _bookList = bookList;
            ListSize = bookList.Count();
        }

        public string BuildList()
        {
            var listBuilder = new StringBuilder();
            int bookIndex = 0;
            if (_bookList.Count() == 0) return "Brak wyników.";
            foreach (var book in _bookList)
            {
                bookIndex++;
                listBuilder.AppendLine($"{bookIndex}. {book.Author}: {book.Name}. \nISBN: {book.ISBN}. \nOst. wypożycz.: {book.LastLendingTime}. ");
                if (!string.IsNullOrWhiteSpace(book.NameOfCurrentHolder)) listBuilder.AppendLine($"Klient: { book.NameOfCurrentHolder}.");
                else listBuilder.AppendLine("Książka na stanie");
                listBuilder.AppendLine();
            }
            return listBuilder.ToString();
        }

        public Book GetBook(int indexOnList)
        {
            if (indexOnList > _bookList.Count()) return null;
            return _bookList.ElementAt(indexOnList - 1);
        }
    }
}

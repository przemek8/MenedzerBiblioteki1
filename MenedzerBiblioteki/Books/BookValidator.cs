using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenedzerBiblioteki.Books
{
    class BookValidator : IBookValidator
    {
        public BookValidationResult ValidateBook(Book book)
        {
            var errorList = new List<string>();

            if (string.IsNullOrWhiteSpace(book.Name))
                errorList.Add("Pusty tytuł książki");
            if (string.IsNullOrWhiteSpace(book.Author))
                errorList.Add("Pusty autor książki");
            if (string.IsNullOrWhiteSpace(book.ISBN))
                errorList.Add("Pusty numer ISBN");

            return new BookValidationResult()
            {
                IsValid = !errorList.Any(),
                ErrorList = errorList
            };
        }
    }
}

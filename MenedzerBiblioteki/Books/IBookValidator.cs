using System.Collections.Generic;

namespace MenedzerBiblioteki.Books
{
    interface IBookValidator
    {
        BookValidationResult ValidateBook(Book book);
    }
}
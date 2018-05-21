using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MenedzerBiblioteki.Books;
using System.Runtime.Serialization;

namespace MenedzerBiblioteki
{
    class Program
    {
        private static readonly IBookService _bookService = new BookService(new BookRepository(), new BookValidator());
        private static readonly IFileService _fileService = new FileService(_bookService);

        static void Main(string[] args)
        {
            Console.WriteLine("Menedżer Biblioteki");
            Console.WriteLine("===================");

            if (args.Length != 1)
            {
                Console.WriteLine("Zła liczba parametrów!");
                Console.WriteLine("Program zostanie zakończony.");
                Console.ReadKey();
                return;
            }
            else Console.WriteLine("Ścieżka katalogu książek: " + args[0]);
            if (Path.GetExtension(args[0]) != ".xml" && Path.GetExtension(args[0]) != ".json")
            {
                Console.WriteLine("Nieprawidłowe rozszerzenie pliku katalogu książek");
                return;
            }
            _fileService.FileName = args[0];
            _fileService.LoadCatalogue();

            string userChoice = string.Empty;
            do
            {
                ShowPossibleOptions();
                userChoice = Console.ReadLine();
                Console.WriteLine("Wybrano opcję " + userChoice);

                switch (userChoice)
                {
                    case "1":
                        BookAddingProcedure();
                        break;

                    case "2":
                        BookRemovingByIDProcedure();
                        break;

                    case "3":
                        BookSearchProcedure();
                        break;

                    case "4":
                        BookSearchByTimeProcedure();
                        break;

                    case "5":
                        BookLendingProcedure();
                        break;

                    case "6":
                        BorrowerListShowingProcedure();
                        break;

                    case "t":
                        Console.WriteLine(new BookListBuilder(_bookService.GetAllBooks()).BuildList());
                        break;
                }
            }
            while (userChoice != "Q" && userChoice != "q");

            Console.WriteLine("Wybrano wyjście z programu. Do widzenia!");
            Console.ReadKey();
        }

        private static void BookAddingProcedure()
        {
            Console.WriteLine("Dodawanie książki do katalogu");
            Console.WriteLine("Podaj tytuł");
            var inputBookName = Console.ReadLine();
            Console.WriteLine("Podaj autora");
            var inputBookAuthor = Console.ReadLine();
            Console.WriteLine("Podaj numer ISBN");
            var inputBookISBN = Console.ReadLine();

            var addingResult = _bookService.AddBookToCatalogue(inputBookName, inputBookAuthor, inputBookISBN);
            if (addingResult.IsValid)
            {
                Console.WriteLine("Książka dodana poprawnie!");
                _fileService.SaveCatalogue();
            }
            else
            {
                Console.WriteLine("Książka nie została dodana, wystąpiły błędy:");
                foreach (string error in addingResult.ErrorList)
                {
                    Console.WriteLine(error);
                }
            }
        }

        private static void BookRemovingByIDProcedure()
        {
            Console.WriteLine("Podaj ID książki z listy pełnej lub nie wpisuj nic, aby zrezygnować");
            if (!int.TryParse(Console.ReadLine(), out var inputBookID) || inputBookID < 1)
            {
                Console.WriteLine("Podano niepoprawny numer lub zrezygnowano");
                return;
            }
            if (_bookService.RemoveBookFromCatalogue(inputBookID))
            {
                Console.WriteLine("Poprawnie usunięto książkę");
                _fileService.SaveCatalogue();
            }
            else Console.WriteLine("Usuwanie książki nie powiodło się (możliwy ID spoza zakresu)");
        }


        private static void BookSearchProcedure()
        {
            Console.WriteLine("[1] ...po tytule");
            Console.WriteLine("[2] ...po autorze");
            Console.WriteLine("[3] ...po ISBN");
            IBookListBuilder bookListBuilder;
            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Podaj fragment tytułu");
                    bookListBuilder = new BookListBuilder(_bookService.SearchBookByName(Console.ReadLine()));
                    break;

                case "2":
                    Console.WriteLine("Podaj fragment autora");
                    bookListBuilder = new BookListBuilder(_bookService.SearchBookByAuthor(Console.ReadLine()));
                    break;

                case "3":
                    Console.WriteLine("Podaj fragment numeru ISBN");
                    bookListBuilder = new BookListBuilder(_bookService.SearchBookByISBN(Console.ReadLine()));
                    break;

                default:
                    Console.WriteLine("Wybrano niewłaściwą opcję wyszukiwania");
                    return;
            }
            Console.WriteLine(bookListBuilder.BuildList());
            if (bookListBuilder.ListSize == 0) return;

            Console.WriteLine("[1] Wypożycz książkę");
            Console.WriteLine("[2] Zwróć książkę");
            Console.WriteLine("[x] Usuń książkę");
            Console.WriteLine("[q] Wróć do menu");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Podaj ID książki z listy do wypożyczenia...");
                    if (!int.TryParse(Console.ReadLine(), out var inputBookID) || inputBookID < 1 || inputBookID > bookListBuilder.ListSize)
                    {
                        Console.WriteLine("Podano niepoprawny numer lub zrezygnowano");
                        return;
                    }
                    Console.WriteLine("Podaj imię i nazwisko wypożyczającego...");
                    string borrowerName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(borrowerName))
                    {
                        Console.WriteLine("Podano błędne dane wypożyczającego");
                        return;
                    }
                    if (_bookService.LendBook(bookListBuilder.GetBook(inputBookID), borrowerName))
                    {
                        Console.WriteLine("Poprawnie wypożyczono książkę");
                        _fileService.SaveCatalogue();
                    }
                    else Console.WriteLine("Wypożyczanie książki nie powiodło się (może być już wypożyczona)");
                    break;

                case "2":
                    Console.WriteLine("Podaj ID książki z listy do zwrotu...");
                    if (!int.TryParse(Console.ReadLine(), out var inputBookIDToReturn) || inputBookIDToReturn < 1 || inputBookIDToReturn > bookListBuilder.ListSize)
                    {
                        Console.WriteLine("Podano niepoprawny numer lub zrezygnowano");
                        return;
                    }
                    if (_bookService.ReturnBook(bookListBuilder.GetBook(inputBookIDToReturn)))
                    {
                        Console.WriteLine("Poprawnie oddano książkę");
                        _fileService.SaveCatalogue();
                    }
                    else Console.WriteLine("Oddanie książki nie powiodło się (być może nie była wypożyczona)");
                    break;

                case "x":
                    Console.WriteLine("Podaj ID książki z listy do usunięcia...");
                    if (!int.TryParse(Console.ReadLine(), out var inputBookIDToRemove) || inputBookIDToRemove < 1 || inputBookIDToRemove > bookListBuilder.ListSize)
                    {
                        Console.WriteLine("Podano niepoprawny numer lub zrezygnowano");
                        return;
                    }
                    if (_bookService.RemoveBookFromCatalogue(bookListBuilder.GetBook(inputBookIDToRemove)))
                    {
                        Console.WriteLine("Poprawnie usunięto książkę");
                        _fileService.SaveCatalogue();
                    }
                    else Console.WriteLine("Usuwanie książki nie powiodło się");

                    break;

                case "q":
                    return;

                default:
                    Console.WriteLine("Wybrano niewłaściwą opcję");
                    return;
            }
        }

        private static void BookSearchByTimeProcedure()
        {
            Console.WriteLine("Książki niewypożyczane przez ile ostatnich tygodni?");
            if (!int.TryParse(Console.ReadLine(), out var weeksNotBorrowed) || weeksNotBorrowed < 1)
            {
                Console.WriteLine("Podano niepoprawną liczbę tygodni");
                return;
            }
            Console.WriteLine(new BookListBuilder(_bookService.SearchBooksNotBorrowedLately(weeksNotBorrowed)).BuildList());
        }

        private static void BookLendingProcedure()
        {
            Console.WriteLine("Funkcjonalność dostępna z poziomu funkcji 3. Wyszukaj");
        }

        private static void BorrowerListShowingProcedure()
        {
            Console.Write(new BorrowerListBuilder(_bookService.ShowAllBorrowers()).BuildList());
        }

        private static void ShowPossibleOptions()
        {
            Console.WriteLine("\nWybierz opcję:");
            Console.WriteLine("[1] Dodanie książki do katalogu");
            Console.WriteLine("[2] Usunięcie książki z katalogu");
            Console.WriteLine("[3] Wyszukaj książkę...");
            Console.WriteLine("[4] Wyszukaj książki długo niewypożyczane");
            Console.WriteLine("[5] Wypożycz książkę");
            Console.WriteLine("[6] Wyświetl listę wypożyczeń");
            Console.WriteLine("[Q] Wyjście z programu");
            Console.WriteLine("[t] Wyświetl wszystkie książki (opcja do testów)");
        }


    }
}

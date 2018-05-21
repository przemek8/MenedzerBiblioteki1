namespace MenedzerBiblioteki.Books
{
    interface IBookListBuilder
    {
        int ListSize { get; }

        string BuildList();
        Book GetBook(int inputBookID);
    }
}
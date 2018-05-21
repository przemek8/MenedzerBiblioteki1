namespace MenedzerBiblioteki
{
    interface IFileService
    {
        string FileName { get; set; }

        void LoadCatalogue();
        void SaveCatalogue();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using MenedzerBiblioteki.Books;

namespace MenedzerBiblioteki
{
    class FileService : IFileService
    {
        public string FileName { get; set; }
        private readonly IBookService _bookService;

        public FileService(IBookService bookService)
        {
            _bookService = bookService;
        }

        public void SaveCatalogue()
        {
            switch (Path.GetExtension(FileName))
            {
                case ".xml":
                    FileName = Path.GetFullPath(FileName);
                    if (File.Exists(FileName)) File.Delete(FileName);
                    using (Stream outputStream = File.OpenWrite(FileName))
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(IEnumerable<Book>));
                        serializer.WriteObject(outputStream, _bookService.GetAllBooks());
                    }
                    break;
                case ".json":
                    FileName = Path.GetFullPath(FileName);
                    if (File.Exists(FileName)) File.Delete(FileName);
                    using (Stream outputStream = File.OpenWrite(FileName))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<Book>));
                        serializer.WriteObject(outputStream, _bookService.GetAllBooks());
                    }
                    break;
            }
        }

        public void LoadCatalogue()
        {
            switch (Path.GetExtension(FileName))
            {
                case ".xml":
                    if (string.IsNullOrWhiteSpace(FileName) || !File.Exists(FileName)) return;
                    using (Stream inputStream = File.OpenRead(FileName))
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(IEnumerable<Book>));
                        _bookService.LoadBooksCatalogue(serializer.ReadObject(inputStream) as IEnumerable<Book>);
                    }
                    break;
                case ".json":
                    if (string.IsNullOrWhiteSpace(FileName) || !File.Exists(FileName)) return;
                    using (Stream inputStream = File.OpenRead(FileName))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<Book>));
                        _bookService.LoadBooksCatalogue(serializer.ReadObject(inputStream) as IEnumerable<Book>);
                    }
                    break;
            }
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenedzerBiblioteki.Books
{
    class BookValidationResult
    {
        public bool IsValid { get; set; }
        public IEnumerable<string> ErrorList { get; set; }
    }
}

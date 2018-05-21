using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MenedzerBiblioteki.Books
{
    [DataContract]
    class Book
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Author { get; set; }
        [DataMember]
        public string ISBN { get; set; }
        [DataMember]
        public DateTime LastLendingTime { get; set; }
        [DataMember]
        public string NameOfCurrentHolder { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenedzerBiblioteki.Books
{
    class BorrowerListBuilder : IBorrowerListBuilder
    {
        private readonly IEnumerable<BorrowerListItem> _borrowerList;

        public BorrowerListBuilder(IEnumerable<BorrowerListItem> borrowerList)
        {
            _borrowerList = borrowerList;
        }

        public string BuildList()
        {
            var listBuilder = new StringBuilder();
            if (_borrowerList.Count() == 0) return "Brak wypożyczeń.";
            foreach (var borrower in _borrowerList)
            {
                if (!string.IsNullOrWhiteSpace(borrower.NameOfBorrower))
                {
                    listBuilder.AppendLine($"{borrower.NameOfBorrower} - {borrower.NumberOfBooksBorrowed}");
                }
            }
            return listBuilder.ToString();
        }
    }
}

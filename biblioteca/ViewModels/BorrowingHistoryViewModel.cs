using System;
using System.Collections.ObjectModel;
using biblioteca.Models;

namespace biblioteca.ViewModels
{
    internal class BorrowingHistoryViewModel
    {
        public string Title => "Borrowing History";

        public ObservableCollection<Loan> Loans { get; set; }

        public BorrowingHistoryViewModel()
        {
            Loans = new ObservableCollection<Loan>
            {
                new Loan
                {
                    BookTitle = "Władca Pierścieni",
                    UserName = "Jan Kowalski",
                    BorrowDate = DateTime.Now.AddDays(-10),
                    ReturnDate = DateTime.Now.AddDays(-2)
                },
                new Loan
                {
                    BookTitle = "Harry Potter",
                    UserName = "Anna Nowak",
                    BorrowDate = DateTime.Now.AddDays(-5),
                    ReturnDate = null
                },
                new Loan
                {
                    BookTitle = "1984",
                    UserName = "Piotr Zieliński",
                    BorrowDate = DateTime.Now.AddDays(-3),
                    ReturnDate = null
                }
            };
        }
    }
}
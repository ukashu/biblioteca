using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using biblioteca.Models;

namespace biblioteca.ViewModels
{
    internal class BorrowingHistoryViewModel
    {
        public string Title => "Borrowing History";

        public ObservableCollection<Loan> Loans { get; set; }

        public BorrowingHistoryViewModel()
        {
            using var db = new Data.LibraryContext();

            var loansFromDb = db.Loans.ToList();

            if (!db.Loans.Any())
            {
                db.Loans.Add(new Loan
                {
                    BookTitle = "1984",
                    UserName = "Test User",
                    BorrowDate = DateTime.Now
                });

                db.SaveChanges();
            }

            Loans = new ObservableCollection<Loan>(loansFromDb);
        }
    }
}
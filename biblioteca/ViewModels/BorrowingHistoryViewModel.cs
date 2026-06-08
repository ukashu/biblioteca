using System;
using System.Collections.ObjectModel;
using biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace biblioteca.ViewModels
{
    internal class BorrowingHistoryViewModel
    {
        public string Title => "Borrowing History";

        public ObservableCollection<Loan> Loans { get; set; }

        public BorrowingHistoryViewModel()
        {
            using var db = new Data.LibraryContext();

            var loansFromDb = db.Loans
                .Include(l => l.User)
                .Include(l => l.Book)
                .ToList();
            Loans = new ObservableCollection<Loan>(loansFromDb);
        }
    }
}
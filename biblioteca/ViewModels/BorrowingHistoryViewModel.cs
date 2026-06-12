using System;
using System.Collections.ObjectModel;
using biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace biblioteca.ViewModels
{
    internal class BorrowingHistoryViewModel
    {
        public string Title => "Historia wypożyczeń";

        public ObservableCollection<Loan> Loans { get; set; }

        public BorrowingHistoryViewModel()
        {
            Loans = new ObservableCollection<Loan>();
            try
            {
                using var db = new Data.LibraryContext();
                var loansFromDb = db.Loans
                    .Include(l => l.User)
                    .Include(l => l.Book)
                    .ToList();
                Loans = new ObservableCollection<Loan>(loansFromDb);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas wczytywania historii wypożyczeń: {ex.Message}", "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
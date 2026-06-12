using biblioteca.Data;
using biblioteca.Models;
using biblioteca.MVVM;
using biblioteca.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace biblioteca.ViewModels
{
    public class ScanBooksViewModel : INotifyPropertyChanged
    {
        private string _inputValue = string.Empty;
        private ScanBookItem? _selectedSuggestion;

        public string Title => "Scan Books";

        public string InputValue
        {
            get => _inputValue;
            set
            {
                _inputValue = value ?? string.Empty;
                OnPropertyChanged(nameof(InputValue));

                RefreshSuggestions();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<ScanBookItem> BookSuggestions { get; } = new ObservableCollection<ScanBookItem>();

        public ObservableCollection<ScanBookItem> LoansToReturn { get; } = new ObservableCollection<ScanBookItem>();

        public ObservableCollection<ScanBookItem> ReturnedBooks => LoansToReturn;

        public bool HasSuggestions => BookSuggestions.Any();

        public ScanBookItem? SelectedSuggestion
        {
            get => _selectedSuggestion;
            set
            {
                _selectedSuggestion = value;
                OnPropertyChanged(nameof(SelectedSuggestion));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public RelayCommand AddBookCommand => new RelayCommand(
            execute => AddBook(),
            canExecute => SelectedSuggestion != null || !string.IsNullOrWhiteSpace(InputValue)
        );

        public RelayCommand ReturnBooksCommand => new RelayCommand(
            execute => ReturnBooks(),
            canExecute => LoansToReturn.Any()
        );

private void RefreshSuggestions()
        {
            BookSuggestions.Clear();
            SelectedSuggestion = null;
            OnPropertyChanged(nameof(HasSuggestions));

            string filter = InputValue.Trim();
            if (string.IsNullOrWhiteSpace(filter))
                return;

            try
            {
                using var db = new LibraryContext();

                var matches = db.Loans
                    .AsNoTracking()
                    .Where(loan => loan.ReturnDate == null)
                    .Join(
                        db.Books.AsNoTracking(),
                        loan => loan.BookTitle,
                        book => book.Title,
                        (loan, book) => new { Loan = loan, Book = book })
                    .AsEnumerable()
                    .Where(match => MatchesFilter(match.Book, filter))
                    .Where(match => LoansToReturn.All(item => item.LoanId != match.Loan.Id))
                    .OrderBy(match => match.Book.Title)
                    .ThenBy(match => match.Loan.UserName)
                    .Take(10)
                    .Select(match => new ScanBookItem
                    {
                        LoanId = match.Loan.Id,
                        BookId = match.Book.Id,
                        BookTitle = match.Book.Title,
                        BookSignature = match.Book.Signature,
                        UserFullName = match.Loan.UserName,
                        BorrowDate = match.Loan.BorrowDate
                    })
                    .ToList();

                foreach (var item in matches)
                {
                    BookSuggestions.Add(item);
                }

                SelectedSuggestion = FindExactSuggestion(filter) ??
                                     (BookSuggestions.Count == 1 ? BookSuggestions.First() : null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to search books: {ex.Message}",
                    "Search Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                OnPropertyChanged(nameof(HasSuggestions));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void AddBook()
        {
            var itemToAdd = SelectedSuggestion ?? FindExactSuggestion(InputValue.Trim());

            if (itemToAdd == null && BookSuggestions.Count == 1)
                itemToAdd = BookSuggestions.First();

            if (itemToAdd == null)
            {
                MessageBox.Show(
                    "Select a book from the suggestions.",
                    "Selection Required",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            if (LoansToReturn.Any(item => item.LoanId == itemToAdd.LoanId))
            {
                MessageBox.Show(
                    "This book has already been added to the return list.",
                    "Already Added",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                ClearInput();
                return;
            }

            LoansToReturn.Add(itemToAdd.Clone());
            ClearInput();
        }

        private void ReturnBooks()
        {
            if (!LoansToReturn.Any())
            {
                MessageBox.Show(
                    "The return list is empty.",
                    "Nothing to Return",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            int returnedCount = LoansToReturn.Count;

            using var db = new LibraryContext();
            using var transaction = db.Database.BeginTransaction();
            try
            {
                DateTime returnDate = DateTime.Now;

                foreach (var item in LoansToReturn.ToList())
                {
                    var loan = db.Loans.FirstOrDefault(currentLoan => currentLoan.Id == item.LoanId);
                    if (loan == null)
                        throw new InvalidOperationException($"Loan for \"{item.BookTitle}\" was not found.");

                    if (loan.ReturnDate != null)
                        throw new InvalidOperationException($"\"{item.BookTitle}\" has already been returned.");

                    var book = db.Books.FirstOrDefault(currentBook => currentBook.Id == item.BookId) ??
                               db.Books.FirstOrDefault(currentBook => currentBook.Title == loan.BookTitle);

                    if (book == null)
                        throw new InvalidOperationException($"Book record for \"{item.BookTitle}\" was not found.");

                    loan.ReturnDate = returnDate;
                    book.IsAvailable = true;
                }

                db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                try { transaction.Rollback(); } catch { }

                MessageBox.Show(
                    $"Unable to return books: {ex.Message}",
                    "Return Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            finally
            {
                CommandManager.InvalidateRequerySuggested();
            }

            LoansToReturn.Clear();
            ClearInput();

            try { EventBus.NotifyNewLoan(); } catch { }

            MessageBox.Show(
                returnedCount == 1
                    ? "The book has been returned successfully."
                    : $"{returnedCount} books have been returned successfully.",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void ClearInput()
        {
            InputValue = string.Empty;
            BookSuggestions.Clear();
            SelectedSuggestion = null;
            OnPropertyChanged(nameof(HasSuggestions));
            CommandManager.InvalidateRequerySuggested();
        }

        private ScanBookItem? FindExactSuggestion(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            string normalizedValue = NormalizeIdentifier(value);

            return BookSuggestions.FirstOrDefault(item =>
                string.Equals(item.BookSignature, value, StringComparison.OrdinalIgnoreCase) ||
                NormalizeIdentifier(item.BookSignature) == normalizedValue);
        }

        private bool MatchesFilter(Book book, string filter)
        {
            if (book == null)
                return false;

            return Contains(book.Signature, filter) ||
                   NormalizeIdentifier(book.Signature).Contains(NormalizeIdentifier(filter)) ||
                   Contains(book.Title, filter);
        }

        private bool Contains(string? value, string filter)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                   value.Contains(filter, StringComparison.OrdinalIgnoreCase);
        }

        private string NormalizeIdentifier(string? value)
        {
            return (value ?? string.Empty)
                .Replace("-", string.Empty)
                .Replace(" ", string.Empty)
                .Trim()
                .ToLowerInvariant();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ScanBookItem
    {
        public int LoanId { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookSignature { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }

        public ScanBookItem Clone()
        {
            return new ScanBookItem
            {
                LoanId = LoanId,
                BookId = BookId,
                BookTitle = BookTitle,
                BookSignature = BookSignature,
                UserFullName = UserFullName,
                BorrowDate = BorrowDate
            };
        }
    }
}
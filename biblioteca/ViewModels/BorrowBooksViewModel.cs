using biblioteca.Models;
using biblioteca.MVVM;
using biblioteca.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace biblioteca.ViewModels
{
    public class BorrowBooksViewModel : INotifyPropertyChanged
    {
        public class BookSuggestion
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Signature { get; set; } = string.Empty;
            public string Display => $"{Title} ({Signature})";
        }

        public class BorrowItem
        {
            public string Signature { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public override string ToString() => $"{Title} ({Signature})";
        }

        private readonly User _user;
        private string _inputValue = string.Empty;
        private BookSuggestion? _selectedSuggestion;
        private bool _suppressSuggestionRefresh;

        public string Header => $"Borrow books for: {_user.FirstName} {_user.LastName}";

        public bool HasSuggestions => Suggestions.Any();

        public string InputValue
        {
            get => _inputValue;
            set
            {
                _inputValue = value;
                OnPropertyChanged(nameof(InputValue));
                if (!_suppressSuggestionRefresh)
                {
                    RefreshSuggestions();
                }

                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<BookSuggestion> Suggestions { get; } = new ObservableCollection<BookSuggestion>();

        public BookSuggestion? SelectedSuggestion
        {
            get => _selectedSuggestion;
            set
            {
                _selectedSuggestion = value;
                OnPropertyChanged(nameof(SelectedSuggestion));

                if (_selectedSuggestion != null)
                {
                    _suppressSuggestionRefresh = true;
                    InputValue = _selectedSuggestion.Signature;
                    _suppressSuggestionRefresh = false;
                }

                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<BorrowItem> BooksToBorrow { get; } = new ObservableCollection<BorrowItem>();

        public RelayCommand AddIsbnCommand => new RelayCommand(
            execute => AddIsbn(),
            canExecute => !string.IsNullOrWhiteSpace(InputValue) || SelectedSuggestion != null
        );

        public RelayCommand BorrowBooksCommand => new RelayCommand(
            execute => BorrowBooks(),
            canExecute => BooksToBorrow.Any()
        );

        public BorrowBooksViewModel(User user)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        public void ApplySuggestion(BookSuggestion suggestion)
        {
            if (suggestion == null) return;

            _suppressSuggestionRefresh = true;
            InputValue = suggestion.Signature;
            _suppressSuggestionRefresh = false;

            Suggestions.Clear();
            OnPropertyChanged(nameof(HasSuggestions));
            CommandManager.InvalidateRequerySuggested();
        }

        private void AddIsbn()
        {
            var signature = InputValue.Trim();
            if (signature.Length == 0) return;

            try
            {
                using var db = new Data.LibraryContext();
                var signatureLower = signature.ToLower();
                var book = db.Books.FirstOrDefault(b => b.Signature.ToLower() == signatureLower);
                if (book == null)
                {
                    MessageBox.Show($"Nie znaleziono książki o sygnaturze/ISBN: {signature}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (BooksToBorrow.Any(b => b.Signature.ToLower() == signatureLower))
                {
                    InputValue = string.Empty;
                    SelectedSuggestion = null;
                    return;
                }

                BooksToBorrow.Add(new BorrowItem { Signature = book.Signature, Title = book.Title });
                InputValue = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania książki: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BorrowBooks()
        {
            try
            {
                using var db = new Data.LibraryContext();
                using var tx = db.Database.BeginTransaction();

                try
                {
                    var userInDb = db.Users.FirstOrDefault(u => u.Id == _user.Id);
                    if (userInDb == null)
                    {
                        MessageBox.Show("Nie znaleziono użytkownika w bazie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    foreach (var item in BooksToBorrow)
                    {
                        var signature = item.Signature;
                        var book = db.Books.FirstOrDefault(b => b.Signature.ToLower() == signature.ToLower());
                        if (book == null || !book.IsAvailable)
                        {
                            tx.Rollback();
                            MessageBox.Show($"Książka {item.Title} ({signature}) jest niedostępna lub nie istnieje.", "Błąd wypożyczania", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        book.IsAvailable = false;

                        db.Loans.Add(new Loan
                        {
                            BookId = book.Id,
                            UserId = userInDb.Id,
                            BorrowDate = DateTime.Now,
                            ReturnDate = null
                        });
                    }

                    db.SaveChanges();
                    tx.Commit();

                    EventBus.NotifyNewLoan();

                    MessageBox.Show("Wypożyczono książki pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    BooksToBorrow.Clear();
                }
                catch (Exception ex)
                {
                    try { tx.Rollback(); } catch { }
                    MessageBox.Show($"Wystąpił błąd podczas wypożyczania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd połączenia z bazą danych: {ex.Message}", "Błąd krytyczny", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshSuggestions()
        {
            Suggestions.Clear();

            var q = InputValue?.Trim();
            if (string.IsNullOrWhiteSpace(q) || q.Length < 1)
            {
                OnPropertyChanged(nameof(HasSuggestions));
                return;
            }

            try
            {
                using var db = new Data.LibraryContext();

                // Match either by title or by signature (ISBN/sygnatura) and only show available books.
                var like = $"%{q}%";
                var qStripped = q.Replace("-", "").Replace(" ", "");
                var likeStripped = $"%{qStripped}%";

                var matches = db.Books
                    .Where(b =>
                        b.IsAvailable &&
                        (
                            EF.Functions.Like(b.Title, like) ||
                            EF.Functions.Like(b.Signature, like) ||
                            EF.Functions.Like(b.Signature.Replace("-", "").Replace(" ", ""), likeStripped)
                        ))
                    .OrderBy(b => b.Title)
                    .Take(8)
                    .Select(b => new BookSuggestion
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Signature = b.Signature
                    })
                    .ToList();

                foreach (var m in matches)
                {
                    Suggestions.Add(m);
                }
            }
            catch (Exception ex)
            {
                // Optionally log error, but probably shouldn't show a messagebox on every key press
                System.Diagnostics.Debug.WriteLine($"Error fetching suggestions: {ex.Message}");
            }

            OnPropertyChanged(nameof(HasSuggestions));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

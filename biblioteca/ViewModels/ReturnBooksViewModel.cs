using biblioteca.Data;
using biblioteca.Models;
using biblioteca.MVVM;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using biblioteca.Services;

namespace biblioteca.ViewModels
{
public class ReturnBooksViewModel : INotifyPropertyChanged
{
private readonly LibraryContext _context;

private string _userFilter;
public string UserFilter
{
    get => _userFilter;
    set
    {
        _userFilter = value;
        OnPropertyChanged(nameof(UserFilter));
        LoadLoans();
    }
}

private string _isbnFilter;
public string IsbnFilter
{
    get => _isbnFilter;
    set
    {
        _isbnFilter = value;
        OnPropertyChanged(nameof(IsbnFilter));
        LoadLoans();
    }
}

    public string Title => "Return Books";

    private ObservableCollection<Loan> _loans;
    public ObservableCollection<Loan> Loans
    {
        get => _loans;
        set
        {
            _loans = value;
            OnPropertyChanged(nameof(Loans));
        }
    }

    private Loan _selectedLoan;
    public Loan SelectedLoan
    {
        get => _selectedLoan;
        set
        {
            _selectedLoan = value;
            OnPropertyChanged(nameof(SelectedLoan));
        }
    }

    public RelayCommand ReturnBookCommand { get; }

    public ReturnBooksViewModel()
    {
        _context = new LibraryContext();

        EventBus.NewLoan += OnNewLoan;

        ReturnBookCommand = new RelayCommand(
            execute => ReturnBook(),
            canExecute => SelectedLoan != null && SelectedLoan.ReturnDate == null
        );

        LoadLoans();
    }

 public void LoadLoans()
{
    try
    {
        var loans = _context.Loans
            .Where(l => l.ReturnDate == null)
            .ToList();

        // filtrowanie po użytkowniku
        if (!string.IsNullOrWhiteSpace(UserFilter))
        {
            loans = loans
                .Where(l => l.UserName != null &&
                            l.UserName.ToLower().Contains(UserFilter.ToLower()))
                .ToList();
        }

        // filtrowanie po ISBN (Signature)
        if (!string.IsNullOrWhiteSpace(IsbnFilter))
        {
            loans = loans
                .Where(l =>
                {
                    var book = _context.Books
                        .FirstOrDefault(b => b.Title == l.BookTitle);

                    return book != null &&
                           book.Signature != null &&
                           book.Signature.ToLower().Contains(IsbnFilter.ToLower());
                })
                .ToList();
        }

        Loans = new ObservableCollection<Loan>(loans);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Błąd podczas ładowania wypożyczeń: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

    private void ReturnBook()
    {
        if (SelectedLoan == null) return;

        try
        {
            SelectedLoan.ReturnDate = DateTime.Now;

            var book = _context.Books.FirstOrDefault(b => b.Title == SelectedLoan.BookTitle);
            if (book != null)
            {
                book.IsAvailable = true;
            }

            _context.SaveChanges();

            MessageBox.Show("Książka została zwrócona!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            LoadLoans(); 
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd podczas zwrotu książki: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private void OnNewLoan()
        {
            LoadLoans();
        }
}

}

using biblioteca.Data;
using biblioteca.Models;
using biblioteca.MVVM;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using biblioteca.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

    public ObservableCollection<Loan> SelectedLoans { get; }
    = new ObservableCollection<Loan>();

    public RelayCommand ReturnBookCommand { get; }

    public ReturnBooksViewModel()
    {
        _context = new LibraryContext();

        EventBus.NewLoan += OnNewLoan;

        ReturnBookCommand = new RelayCommand(
            execute => ReturnBook(),
            canExecute => SelectedLoans.Any()
        );

        LoadLoans();
    }

 public void LoadLoans()
{
    var loans = _context.Loans
        .Where(l => l.ReturnDate == null)
        .Include(l => l.Book)
        .ToList();

    if (!string.IsNullOrWhiteSpace(UserFilter))
    {
        loans = loans
            .Where(l => l.User.LastName != null &&
                        l.User.LastName.ToLower().Contains(UserFilter.ToLower()))
            .ToList();
    }

    if (!string.IsNullOrWhiteSpace(IsbnFilter))
    {
        loans = loans
            .Where(l =>
                l.Book != null &&
                l.Book.Signature != null &&
                l.Book.Signature.ToLower().Contains(IsbnFilter.ToLower())
            )
            .ToList();
    }

    Loans = new ObservableCollection<Loan>(loans);
}

    private void ReturnBook()
    {
        if (!SelectedLoans.Any())
            return;

        var returnedBooks = new List<string>();

        foreach (var loan in SelectedLoans.ToList())
        {
            loan.ReturnDate = DateTime.Now;

            var book = _context.Books.FirstOrDefault(b => b.Title == loan.Book.Title);

            if (book != null)
            {
                book.IsAvailable = true;
            }

            returnedBooks.Add(book.Title);
        }

        _context.SaveChanges();

        MessageBox.Show(
            "Zwrócono książki:\n\n" +
            string.Join("\n", returnedBooks),
            "Sukces",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        SelectedLoans.Clear();

        LoadLoans();
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

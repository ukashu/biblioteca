using biblioteca.Data;
using biblioteca.Models;
using biblioteca.MVVM;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace biblioteca.ViewModels
{
public class ReturnBooksViewModel : INotifyPropertyChanged
{
private readonly LibraryContext _context;

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

        ReturnBookCommand = new RelayCommand(
            execute => ReturnBook(),
            canExecute => SelectedLoan != null && SelectedLoan.ReturnDate == null
        );

        LoadLoans();
    }

    public void LoadLoans()
    {
        var loans = _context.Loans
            .Where(l => l.ReturnDate == null)
            .ToList();

        Loans = new ObservableCollection<Loan>(loans);
    }

    private void ReturnBook()
    {
        if (SelectedLoan == null) return;

        SelectedLoan.ReturnDate = DateTime.Now;

        _context.SaveChanges();

        MessageBox.Show("Książka została zwrócona!");

        LoadLoans(); 
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

}

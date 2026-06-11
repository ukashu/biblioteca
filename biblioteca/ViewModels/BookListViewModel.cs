using biblioteca.Data;
using biblioteca.Models;
using biblioteca.MVVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace biblioteca.ViewModels
{
    public class BookListViewModel : INotifyPropertyChanged
    {
        public string Title => "Books";

        public ObservableCollection<Book> Books { get; set; }

        public RelayCommand AddBookCommand => new RelayCommand(execute => AddBook());
        public RelayCommand AddBookWithDialogCommand => new RelayCommand(execute => AddBookWithDialog());

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Book _selectedBook;
        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
                OnPropertyChanged(nameof(CurrentBorrower));
            }
        }

        public string CurrentBorrower
        {
            get
            {
                var loan = SelectedBook?
                    .Loans?
                    .FirstOrDefault(l => l.ReturnDate == null);

                if (loan?.User == null)
                    return "Available";

                return loan.UserFullName;
            }
        }

        public BookListViewModel()
        {
            using var db = new Data.LibraryContext();

            if (!db.Books.Any())
            {
                db.Books.AddRange(
                    new Book("The Great Gatsby", "F. Scott Fitzgerald", 1925, "Novel", "GAT123", "A novel set in the Roaring Twenties."),
                    new Book("To Kill a Mockingbird", "Harper Lee", 1960, "Novel", "LEE456", "A novel about racial injustice in the Deep South."),
                    new Book("1984", "George Orwell", 1949, "Dystopian", "ORW789", "A dystopian novel about totalitarianism.")
                );

                db.SaveChanges();
            }

            var booksFromDb = db.Books
                .Include(b=>b.Loans)
                    .ThenInclude(l => l.User)
                .ToList();
            Books = new ObservableCollection<Book>(booksFromDb);
        }

        private void AddBook()
        {
            var newBook = new Book("New Book", "Author Name", 2024, "Genre", "ISBN123", "Description of the new book.");
        }

        private void AddBookWithDialog()
        {
            var addBookWindow = new Views.AddBook();
            if (addBookWindow.ShowDialog() == true)
            {
                var newBook = addBookWindow.CreatedBook;

                using var db = new LibraryContext();
                db.Books.Add(newBook);
                db.SaveChanges();

                Books.Add(newBook);
            }
        }

        public void DeleteBook(Book book)
        {
            if (book == null) return;

            using var db = new LibraryContext();

            db.Books.Remove(book);
            db.SaveChanges();

            Books.Remove(book);
        }

        public void UpdateBook(Book updatedBook)
        {
            using var db = new LibraryContext();

            var bookInDb = db.Books.Find(updatedBook.Id);
            if (bookInDb == null) return;

            bookInDb.CopyFrom(updatedBook);

            db.SaveChanges();
        }
    }
}

using biblioteca.Models;
using biblioteca.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace biblioteca.ViewModels
{
    internal class BookListViewModel
    {
        public string Title => "Books";

        public ObservableCollection<Book> Books { get; set; }

        public RelayCommand AddBookCommand => new RelayCommand(execute => AddBook());

        public BookListViewModel()
        {
            Books = new ObservableCollection<Book>
            {
                new Book("The Great Gatsby", "F. Scott Fitzgerald", 1925, "Novel", "GAT123", "A novel set in the Roaring Twenties."),
                new Book("To Kill a Mockingbird", "Harper Lee", 1960, "Novel", "LEE456", "A novel about racial injustice in the Deep South."),
                new Book("1984", "George Orwell", 1949, "Dystopian", "ORW789", "A dystopian novel about totalitarianism."),
            };
        }

        private void AddBook()
        {
            Books.Add(new Book("New Book", "Author Name", 2024, "Genre", "ISBN123", "Description of the new book."));
        }
    }
}

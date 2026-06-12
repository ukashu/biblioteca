using biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace biblioteca.Views
{
    /// <summary>
    /// Interaction logic for BookDetails.xaml
    /// </summary>
    public partial class BookDetails : Window
    {
        private Book _originalBook;
        private Book _editedBook;
        private readonly Action<Book> _deleteBookAction;
        private readonly Func<Book, bool> _updateBookAction;
        public BookDetails(Models.Book book, Action<Book> deleteBookAction, Func<Book, bool> updateBookAction)
        {
            InitializeComponent();
            DataContext = book;
            _originalBook = book;
            _deleteBookAction = deleteBookAction;
            _updateBookAction = updateBookAction;

            EnterViewMode();
        }

        private void LoadEditValues()
        {
            TitleBox.Text = _editedBook.Title;
            AuthorBox.Text = _editedBook.Author;
            PublicationYearBox.Text = _editedBook.PublicationYear.ToString();
            GenreBox.Text = _editedBook.Genre;
            SignatureBox.Text = _editedBook.Signature;
            DescriptionBox.Text = _editedBook.Description;
            IsAvailableBox.IsChecked = _editedBook.IsAvailable;
        }

        private void ReadEditValues()
        {
            _editedBook.Title = TitleBox.Text;
            _editedBook.Author = AuthorBox.Text;
            _editedBook.Genre = GenreBox.Text;
            _editedBook.Signature = SignatureBox.Text;
            _editedBook.Description = DescriptionBox.Text;
            _editedBook.IsAvailable = IsAvailableBox.IsChecked == true;
        }

        private void EnterViewMode()
        {
            ViewDetailsPanel.Visibility = Visibility.Visible;
            EditDetailsPanel.Visibility = Visibility.Collapsed;
        }

        private void EnterEditMode()
        {
            ViewDetailsPanel.Visibility = Visibility.Collapsed;
            EditDetailsPanel.Visibility = Visibility.Visible;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _editedBook = _originalBook.Clone();
            LoadEditValues();
            EnterEditMode();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Tytuł jest wymagany.");
                return;
            }

            if (string.IsNullOrWhiteSpace(AuthorBox.Text))
            {
                MessageBox.Show("Autor jest wymagany.");
                return;
            }

            if (!int.TryParse(PublicationYearBox.Text, out int publicationYear))
            {
                MessageBox.Show("Niewłaściwy rok publikacji. Podaj poprawną liczbę całkowitą.");
                return;
            }

            string signature = SignatureBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(signature) || !System.Text.RegularExpressions.Regex.IsMatch(signature, @"^[a-zA-Z0-9\-]+$"))
            {
                MessageBox.Show("Sygnatura/ISBN jest wymagana i może składać się tylko z liter, cyfr i myślników.");
                return;
            }

            ReadEditValues();
            _editedBook.PublicationYear = publicationYear;

            bool success = _updateBookAction?.Invoke(_originalBook) ?? false;

            if (!success)
            {
                return;
            }

            _originalBook.CopyFrom(_editedBook);

            EnterViewMode();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _editedBook = _originalBook.Clone();
            EnterViewMode();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"Usunąć książkę \"{_originalBook.Title}\"?",
                "Potwierdź",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _deleteBookAction?.Invoke(_originalBook);
                Close();
            }
        }
    }
}

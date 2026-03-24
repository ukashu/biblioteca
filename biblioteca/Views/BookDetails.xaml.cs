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
        public BookDetails(Models.Book book)
        {
            InitializeComponent();
            DataContext = book;
            _originalBook = book;

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
            TitleText.Visibility = Visibility.Visible;
            AuthorText.Visibility = Visibility.Visible;
            PublicationYearText.Visibility = Visibility.Visible;
            GenreText.Visibility = Visibility.Visible;
            SignatureText.Visibility = Visibility.Visible;
            DescriptionText.Visibility = Visibility.Visible;
            IsAvailableText.Visibility = Visibility.Visible;

            TitleBox.Visibility = Visibility.Collapsed;
            AuthorBox.Visibility = Visibility.Collapsed;
            PublicationYearBox.Visibility = Visibility.Collapsed;
            GenreBox.Visibility = Visibility.Collapsed;
            SignatureBox.Visibility = Visibility.Collapsed;
            DescriptionBox.Visibility = Visibility.Collapsed;
            IsAvailableBox.Visibility = Visibility.Collapsed;

            EditButton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Collapsed;
            CancelButton.Visibility = Visibility.Collapsed;
        }

        private void EnterEditMode()
        {
            TitleText.Visibility = Visibility.Collapsed;
            AuthorText.Visibility = Visibility.Collapsed;
            PublicationYearText.Visibility = Visibility.Collapsed;
            GenreText.Visibility = Visibility.Collapsed;
            SignatureText.Visibility = Visibility.Collapsed;
            DescriptionText.Visibility = Visibility.Collapsed;
            IsAvailableText.Visibility = Visibility.Collapsed;

            TitleBox.Visibility = Visibility.Visible;
            AuthorBox.Visibility = Visibility.Visible;
            PublicationYearBox.Visibility = Visibility.Visible;
            GenreBox.Visibility = Visibility.Visible;
            SignatureBox.Visibility = Visibility.Visible;
            DescriptionBox.Visibility = Visibility.Visible;
            IsAvailableBox.Visibility = Visibility.Visible;

            EditButton.Visibility = Visibility.Collapsed;
            SaveButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Visible;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _editedBook = _originalBook.Clone();
            LoadEditValues();
            EnterEditMode();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(PublicationYearBox.Text, out int year))
            {
                MessageBox.Show("Publication Year must be a number.");
                return;
            }

            ReadEditValues();
            _editedBook.PublicationYear = year;

            _originalBook.CopyFrom(_editedBook);
            EnterViewMode();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _editedBook = _originalBook.Clone();
            EnterViewMode();
        }
    }
}

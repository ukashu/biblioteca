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
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        public Book CreatedBook { get; private set;}
        public AddBook()
        {
            InitializeComponent();
        }

        public void AddBook_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(YearBox.Text, out int publicationYear))
            {
                MessageBox.Show("Niewłaściwy rok publikacji.");
                return;
            }

            CreatedBook = new Book
            {
                Title = TitleBox.Text,
                Author = AuthorBox.Text,
                PublicationYear = publicationYear,
                Genre = GenreBox.Text,
                Signature = SignatureBox.Text,
                Description = DescriptionBox.Text,
                IsAvailable = AvailableBox.IsChecked == true
            };

            DialogResult = true;
            Close();
        }
    }
}

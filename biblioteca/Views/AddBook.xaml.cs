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
            ErrorTextBlock.Text = "";

            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                ErrorTextBlock.Text = "Tytuł jest wymagany.";
                return;
            }

            if (string.IsNullOrWhiteSpace(AuthorBox.Text))
            {
                ErrorTextBlock.Text = "Autor jest wymagany.";
                return;
            }

            if (!int.TryParse(YearBox.Text, out int publicationYear))
            {
                ErrorTextBlock.Text = "Niewłaściwy rok publikacji. Podaj poprawną liczbę całkowitą.";
                return;
            }

            string signature = SignatureBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(signature) || !System.Text.RegularExpressions.Regex.IsMatch(signature, @"^[a-zA-Z0-9\-]+$"))
            {
                ErrorTextBlock.Text = "Sygnatura/ISBN jest wymagana i może składać się tylko z liter, cyfr i myślników.";
                return;
            }

            CreatedBook = new Book
            {
                Title = TitleBox.Text.Trim(),
                Author = AuthorBox.Text.Trim(),
                PublicationYear = publicationYear,
                Genre = GenreBox.Text?.Trim(),
                Signature = signature,
                Description = DescriptionBox.Text?.Trim(),
                IsAvailable = AvailableBox.IsChecked == true
            };

            DialogResult = true;
            Close();
        }
    }
}

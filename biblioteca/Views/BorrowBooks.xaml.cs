using biblioteca.Models;
using biblioteca.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace biblioteca.Views
{
    public partial class BorrowBooks : Window
    {
        public BorrowBooks(User user)
        {
            InitializeComponent();
            DataContext = new BorrowBooksViewModel(user);
        }

        private void SuggestionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is not BorrowBooksViewModel vm) return;
            if (sender is not ListBox list) return;
            if (list.SelectedItem is not BorrowBooksViewModel.BookSuggestion suggestion) return;

            vm.ApplySuggestion(suggestion);
            list.SelectedItem = null;

            InputBox.Focus();
            InputBox.CaretIndex = InputBox.Text.Length;
        }
    }
}


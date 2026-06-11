using biblioteca.Helpers;
using biblioteca.ViewModels;
using Castle.DynamicProxy.Generators;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Documents;


namespace biblioteca.Views
{
    public partial class BookList : UserControl
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public ICollectionView _booksView;

        public BookList()
        {
            InitializeComponent();
            DataContext = new BookListViewModel();

            Loaded += BookList_Loaded;
        }

        private void BookList_Loaded(object sender, RoutedEventArgs e)
        {
            _booksView = CollectionViewSource.GetDefaultView(booksListView.ItemsSource);
        }

        private void BooksList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView booksList &&
                booksList.SelectedItem is biblioteca.Models.Book item)
            {
                var selectedBook = item;

                if (DataContext is BookListViewModel viewModel)
                {
                    var detailsWindow =
                        new BookDetails(
                            selectedBook,
                            viewModel.DeleteBook,
                            viewModel.UpdateBook);

                    detailsWindow.ShowDialog();
                }
            }
        }
        private void BookItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ListViewItem item)
                return;

            if (item.DataContext is not Models.Book selectedBook)
                return;

            if (DataContext is BookListViewModel viewModel)
            {
                var detailsWindow = new BookDetails(selectedBook, viewModel.DeleteBook, viewModel.UpdateBook);
                detailsWindow.ShowDialog();
            }
        }

        private void booksListViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = sender as GridViewColumnHeader;

            if (column?.Tag == null)
                return;

            string sortBy = column.Tag.ToString();

            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol)
                    .Remove(listViewSortAdorner);

                CollectionViewSource.GetDefaultView(booksListView.ItemsSource)
                    .SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;

            if (listViewSortCol == column &&
                listViewSortAdorner.Direction == newDir)
            {
                newDir = ListSortDirection.Descending;
            }

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);

            AdornerLayer.GetAdornerLayer(listViewSortCol)
                .Add(listViewSortAdorner);

            CollectionViewSource.GetDefaultView(booksListView.ItemsSource)
                .SortDescriptions.Add(
                    new SortDescription(sortBy, newDir));
        }

        private void ApplyFilters()
        {
            if (_booksView == null)
                return;

            _booksView.Filter = item =>
            {
                if (item is not biblioteca.Models.Book book)
                    return false;

                if (!string.IsNullOrWhiteSpace(TitleFilterTextBox.Text) &&
                    !book.Title.Contains(
                        TitleFilterTextBox.Text,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(AuthorFilterTextBox.Text) &&
                    !book.Author.Contains(
                        AuthorFilterTextBox.Text,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(SignatureFilterTextBox.Text) &&
                    !book.Signature.Contains(
                        SignatureFilterTextBox.Text,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (OverdueOnlyCheckBox.IsChecked == true &&
                    !book.IsOverdue)
                {
                    return false;
                }

                return true;
            };

            _booksView.Refresh();
        }

        private void TitleFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void AuthorFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SignatureFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void OverdueOnlyCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }
    }
}
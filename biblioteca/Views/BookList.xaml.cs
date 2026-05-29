using biblioteca.Helpers;
using biblioteca.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace biblioteca.Views
{
    /// <summary>
    /// Interaction logic for BookList.xaml
    /// </summary>
    public partial class BookList : UserControl
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public ICollectionView _booksView;
        public BookList()
        {
            InitializeComponent();
            DataContext = new ViewModels.BookListViewModel();

            Loaded += BookList_Loaded;
        }

        private void BookList_Loaded(object sender, RoutedEventArgs e)
        {
            _booksView = CollectionViewSource.GetDefaultView(booksListView.ItemsSource);
        }

        private void BooksList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView booksList && booksList.SelectedItem is Models.Book selectedBook)
            {
                if (DataContext is BookListViewModel viewModel)
                {
                    var detailsWindow = new BookDetails(selectedBook, viewModel.DeleteBook, viewModel.UpdateBook);
                    detailsWindow.ShowDialog();
                }
            }
        }

        private void booksListViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                CollectionViewSource.GetDefaultView(booksListView.ItemsSource).SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if(listViewSortCol == column && listViewSortAdorner.Direction == newDir)
            {
                newDir = ListSortDirection.Descending;
            }

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            CollectionViewSource.GetDefaultView(booksListView.ItemsSource).SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
        private void TitleFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_booksView == null) return;

            _booksView.Filter = item =>
            {
                if (item is not Models.Book book) return false;

                string filter = TitleFilterTextBox.Text;

                if (string.IsNullOrWhiteSpace(filter)) return true;

                return book.Title.Contains(filter, StringComparison.OrdinalIgnoreCase);
            };

            _booksView.Refresh();
        }
        private void AuthorFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_booksView == null) return;

            _booksView.Filter = item =>
            {
                if (item is not Models.Book book) return false;
                string filter = AuthorFilterTextBox.Text;

                if (string.IsNullOrWhiteSpace(filter)) return true;

                return book.Author.Contains(filter, StringComparison.OrdinalIgnoreCase);
            };

            _booksView.Refresh();
        }

        private void SignatureFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_booksView == null) return;

            _booksView.Filter = item =>
            {
                if (item is not Models.Book book) return false;

                string filter = SignatureFilterTextBox.Text;

                if (string.IsNullOrWhiteSpace(filter)) return true;

                return book.Signature.Contains(filter, StringComparison.OrdinalIgnoreCase);
            };

            _booksView.Refresh();
        }
    }
}

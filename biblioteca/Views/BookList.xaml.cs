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
        public BookList()
        {
            InitializeComponent();
            DataContext = new ViewModels.BookListViewModel();
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
    }
}

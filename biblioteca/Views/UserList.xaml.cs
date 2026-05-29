using biblioteca.Helpers;
using biblioteca.ViewModels;
using System.ComponentModel;
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
    /// Interaction logic for UserList.xaml
    /// </summary>
    public partial class UserList : UserControl
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public UserList()
        {
            InitializeComponent();
            DataContext = new UserListViewModel();
        }

        private void UsersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView usersList && usersList.SelectedItem is Models.User selectedUser)
            {
                if (DataContext is UserListViewModel viewModel)
                {
                    var detailsWindow = new UserDetails(selectedUser, viewModel.DeleteUser, viewModel.UpdateUser);
                    detailsWindow.ShowDialog();
                }
            }
        }

        private void usersListViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                CollectionViewSource.GetDefaultView(usersListView.ItemsSource).SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if(listViewSortCol == column && listViewSortAdorner.Direction == newDir)
            {
                newDir = ListSortDirection.Descending;
            }

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            CollectionViewSource.GetDefaultView(usersListView.ItemsSource).SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
    }
}

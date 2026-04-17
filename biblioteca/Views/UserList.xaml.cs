using biblioteca.ViewModels;
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
                    var detailsWindow = new UserDetails(selectedUser, viewModel.DeleteUser);
                    detailsWindow.ShowDialog();
                }
            }
        }
    }
}

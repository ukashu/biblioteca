using biblioteca.Models;
using biblioteca.MVVM;
using System.Collections.ObjectModel;

namespace biblioteca.ViewModels
{
    public class UserListViewModel
    {
        public string Title => "Users";

        public ObservableCollection<User> Users { get; set; }

        public RelayCommand AddUserWithDialogCommand => new RelayCommand(execute => AddUserWithDialog());

        public UserListViewModel()
        {
            Users = new ObservableCollection<User>
            {
                new User("Ola", "Kowalska", "ola.kowalska@example.com", "+48 600 100 200", "CARD-0001", "Czytelnik aktywny."),
                new User("Wiktoria", "Nowak", "wiktoria.nowak@example.com", "+48 600 200 300", "CARD-0002", "Lubi kryminały."),
                new User("Natalia", "Wiśniewska", "natalia.wisniewska@example.com", "+48 600 300 400", "CARD-0003", "Prosi o przypomnienia e-mail."),
            };
        }

        private void AddUserWithDialog()
        {
            var addUserWindow = new Views.AddUser();
            if (addUserWindow.ShowDialog() == true)
            {
                Users.Add(addUserWindow.CreatedUser);
            }
        }

        public void DeleteUser(User user)
        {
            if (user != null)
            {
                Users.Remove(user);
            }
        }
    }
}

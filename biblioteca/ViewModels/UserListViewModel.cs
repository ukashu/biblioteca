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
            using var db = new Data.LibraryContext();

            var usersFromDb = db.Users.ToList();

            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new User("Ola", "Kowalska", "ola.kowalska@example.com", "+48 600 100 200", "CARD-0001", "Czytelnik aktywny."),
                    new User("Wiktoria", "Nowak", "wiktoria.nowak@example.com", "+48 600 200 300", "CARD-0002", "Lubi kryminały."),
                    new User("Natalia", "Wiśniewska", "natalia.wisniewska@example.com", "+48 600 300 400", "CARD-0003", "Prosi o przypomnienia e-mail.")
                );

                db.SaveChanges();
            }

            Users = new ObservableCollection<User>(usersFromDb);
        }

        private void AddUserWithDialog()
        {
            var addUserWindow = new Views.AddUser();
            if (addUserWindow.ShowDialog() == true)
            {
                var newUser = addUserWindow.CreatedUser;

                using var db = new Data.LibraryContext();
                db.Users.Add(newUser);
                db.SaveChanges();

                Users.Add(newUser);
            }
        }

        public void DeleteUser(User user)
        {
            if (user == null) return;
            using var db = new Data.LibraryContext();
            var userToDelete = db.Users.FirstOrDefault(u => u.Id == user.Id);
            if (userToDelete != null)
            {
                db.Users.Remove(userToDelete);
                db.SaveChanges();
                Users.Remove(user);
            }
        }
    }
}

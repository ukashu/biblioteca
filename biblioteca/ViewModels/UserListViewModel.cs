using biblioteca.Models;
using biblioteca.MVVM;
using biblioteca.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace biblioteca.ViewModels
{
    public class UserListViewModel : INotifyPropertyChanged
    {
        public string Title => "Users";

        public ObservableCollection<User> Users { get; } = new();

        public RelayCommand AddUserWithDialogCommand => new RelayCommand(execute => AddUserWithDialog());
        public RelayCommand BorrowBooksForUserCommand => new RelayCommand(
            execute => OpenBorrowBooksWindow(execute as User),
            canExecute => canExecute is User
        );

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public UserListViewModel()
        {
            LoadUsers();

            EventBus.NewLoan += LoadUsers;
        }
        
        private void LoadUsers()
        {
            using var db = new Data.LibraryContext();

            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new User("Ola", "Kowalska", "ola.kowalska@example.com", "+48 600 100 200", "CARD-0001", "Czytelnik aktywny."),
                    new User("Wiktoria", "Nowak", "wiktoria.nowak@example.com", "+48 600 200 300", "CARD-0002", "Lubi kryminały."),
                    new User("Natalia", "Wiśniewska", "natalia.wisniewska@example.com", "+48 600 300 400", "CARD-0003", "Prosi o przypomnienia e-mail.")
                );

                db.SaveChanges();
            }

            var usersFromDb = db.Users
                .Include(u => u.Loans)
                .ToList();

            Users.Clear();

            foreach (var user in usersFromDb)
            {
                Users.Add(user);
            }
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

        public void UpdateUser(User updatedUser)
        {
            using var db = new Data.LibraryContext();

            var userInDb = db.Users.Find(updatedUser.Id);
            if (userInDb == null) return;

            userInDb.CopyFrom(updatedUser);

            db.SaveChanges();
        }

        private void OpenBorrowBooksWindow(User? user)
        {
            if (user == null) return;

            var borrowWindow = new Views.BorrowBooks(user);
            borrowWindow.ShowDialog();
        }
    }
}

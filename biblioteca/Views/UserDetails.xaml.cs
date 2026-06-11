using biblioteca.Models;
using System;
using System.Windows;

namespace biblioteca.Views
{
    /// <summary>
    /// Interaction logic for UserDetails.xaml
    /// </summary>
    public partial class UserDetails : Window
    {
        private User _originalUser;
        private User _editedUser = new User();
        private readonly Action<User> _deleteUserAction;
        private readonly Action<User> _updateUserAction;

        public UserDetails(User user, Action<User> deleteUserAction, Action<User> updateUserAction)
        {
            InitializeComponent();
            DataContext = user;
            _originalUser = user;
            _deleteUserAction = deleteUserAction;
            _updateUserAction = updateUserAction;

            EnterViewMode();
        }

        private void LoadEditValues()
        {
            FirstNameBox.Text = _editedUser.FirstName;
            LastNameBox.Text = _editedUser.LastName;
            EmailBox.Text = _editedUser.Email;
            PhoneBox.Text = _editedUser.PhoneNumber;
            CardBox.Text = _editedUser.CardNumber;
            NotesBox.Text = _editedUser.Notes;
            IsActiveBox.IsChecked = _editedUser.IsActive;
        }

        private void ReadEditValues()
        {
            _editedUser.FirstName = FirstNameBox.Text;
            _editedUser.LastName = LastNameBox.Text;
            _editedUser.Email = EmailBox.Text;
            _editedUser.PhoneNumber = PhoneBox.Text;
            _editedUser.CardNumber = CardBox.Text;
            _editedUser.Notes = NotesBox.Text;
            _editedUser.IsActive = IsActiveBox.IsChecked == true;
        }

        private void EnterViewMode()
        {
            ViewDetailsPanel.Visibility = Visibility.Visible;
            EditDetailsPanel.Visibility = Visibility.Collapsed;
        }

        private void EnterEditMode()
        {
            ViewDetailsPanel.Visibility = Visibility.Collapsed;
            EditDetailsPanel.Visibility = Visibility.Visible;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _editedUser = _originalUser.Clone();
            LoadEditValues();
            EnterEditMode();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ReadEditValues();

            if (string.IsNullOrWhiteSpace(_editedUser.FirstName) || string.IsNullOrWhiteSpace(_editedUser.LastName))
            {
                MessageBox.Show("First name and last name are required.");
                return;
            }

            _originalUser.CopyFrom(_editedUser);

            _updateUserAction?.Invoke(_originalUser);

            EnterViewMode();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _editedUser = _originalUser.Clone();
            EnterViewMode();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"Usunąć użytkownika \"{_originalUser.FirstName} {_originalUser.LastName}\"?",
                "Potwierdź",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _deleteUserAction?.Invoke(_originalUser);
                Close();
            }
        }
    }
}

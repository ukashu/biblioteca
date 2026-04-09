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

        public UserDetails(User user, Action<User> deleteUserAction)
        {
            InitializeComponent();
            DataContext = user;
            _originalUser = user;
            _deleteUserAction = deleteUserAction;

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
            FirstNameText.Visibility = Visibility.Visible;
            LastNameText.Visibility = Visibility.Visible;
            EmailText.Visibility = Visibility.Visible;
            PhoneText.Visibility = Visibility.Visible;
            CardText.Visibility = Visibility.Visible;
            NotesText.Visibility = Visibility.Visible;
            IsActiveText.Visibility = Visibility.Visible;

            FirstNameBox.Visibility = Visibility.Collapsed;
            LastNameBox.Visibility = Visibility.Collapsed;
            EmailBox.Visibility = Visibility.Collapsed;
            PhoneBox.Visibility = Visibility.Collapsed;
            CardBox.Visibility = Visibility.Collapsed;
            NotesBox.Visibility = Visibility.Collapsed;
            IsActiveBox.Visibility = Visibility.Collapsed;

            EditButton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Collapsed;
            CancelButton.Visibility = Visibility.Collapsed;
        }

        private void EnterEditMode()
        {
            FirstNameText.Visibility = Visibility.Collapsed;
            LastNameText.Visibility = Visibility.Collapsed;
            EmailText.Visibility = Visibility.Collapsed;
            PhoneText.Visibility = Visibility.Collapsed;
            CardText.Visibility = Visibility.Collapsed;
            NotesText.Visibility = Visibility.Collapsed;
            IsActiveText.Visibility = Visibility.Collapsed;

            FirstNameBox.Visibility = Visibility.Visible;
            LastNameBox.Visibility = Visibility.Visible;
            EmailBox.Visibility = Visibility.Visible;
            PhoneBox.Visibility = Visibility.Visible;
            CardBox.Visibility = Visibility.Visible;
            NotesBox.Visibility = Visibility.Visible;
            IsActiveBox.Visibility = Visibility.Visible;

            EditButton.Visibility = Visibility.Collapsed;
            SaveButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Visible;
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

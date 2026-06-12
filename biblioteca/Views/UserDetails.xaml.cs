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
        private readonly Func<User, bool> _updateUserAction;

        public UserDetails(User user, Action<User> deleteUserAction, Func<User, bool> updateUserAction)
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

            if (string.IsNullOrWhiteSpace(FirstNameBox.Text))
            {
                MessageBox.Show("Imię jest wymagane.", "Błąd");
                return;
            }
            if (string.IsNullOrWhiteSpace(LastNameBox.Text))
            {
                MessageBox.Show("Nazwisko jest wymagane.", "Błąd");
                return;
            }

            string email = EmailBox.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(email) && !System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Niepoprawny format adresu email.", "Błąd");
                return;
            }

            string phone = PhoneBox.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(phone) && !System.Text.RegularExpressions.Regex.IsMatch(phone, @"^[0-9+\-\s]+$"))
            {
                MessageBox.Show("Niepoprawny format numeru telefonu.", "Błąd");
                return;
            }

            string cardNumber = CardBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(cardNumber) || !System.Text.RegularExpressions.Regex.IsMatch(cardNumber, @"^[a-zA-Z0-9\-]+$"))
            {
                MessageBox.Show("Nr karty jest wymagany i może składać się tylko z liter, cyfr i myślników.", "Błąd");
                return;
            }


            bool updated = _updateUserAction?.Invoke(_originalUser) ?? false;

            if (!updated)
            {
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

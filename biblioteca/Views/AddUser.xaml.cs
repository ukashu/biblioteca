using biblioteca.Models;
using System.Windows;

namespace biblioteca.Views
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public User CreatedUser { get; private set; } = new User();

        public AddUser()
        {
            InitializeComponent();
        }

        public void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameBox.Text) || string.IsNullOrWhiteSpace(LastNameBox.Text))
            {
                MessageBox.Show("Imię i nazwisko są wymagane.");
                return;
            }

            CreatedUser = new User
            {
                FirstName = FirstNameBox.Text.Trim(),
                LastName = LastNameBox.Text.Trim(),
                Email = EmailBox.Text.Trim(),
                PhoneNumber = PhoneBox.Text.Trim(),
                CardNumber = CardNumberBox.Text.Trim(),
                Notes = NotesBox.Text?.Trim() ?? string.Empty,
                IsActive = true
            };

            DialogResult = true;
            Close();
        }
    }
}

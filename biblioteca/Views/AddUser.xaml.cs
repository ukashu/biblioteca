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
            ErrorTextBlock.Text = "";

            if (string.IsNullOrWhiteSpace(FirstNameBox.Text))
            {
                ErrorTextBlock.Text = "Imię jest wymagane.";
                return;
            }
            if (string.IsNullOrWhiteSpace(LastNameBox.Text))
            {
                ErrorTextBlock.Text = "Nazwisko jest wymagane.";
                return;
            }

            string email = EmailBox.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(email) && !System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ErrorTextBlock.Text = "Niepoprawny format adresu email.";
                return;
            }

            string phone = PhoneBox.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(phone) && !System.Text.RegularExpressions.Regex.IsMatch(phone, @"^[0-9+\-\s]+$"))
            {
                ErrorTextBlock.Text = "Niepoprawny format numeru telefonu.";
                return;
            }

            string cardNumber = CardNumberBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(cardNumber) || !System.Text.RegularExpressions.Regex.IsMatch(cardNumber, @"^[a-zA-Z0-9\-]+$"))
            {
                ErrorTextBlock.Text = "Nr karty jest wymagany i może składać się tylko z liter, cyfr i myślników.";
                return;
            }

            CreatedUser = new User
            {
                FirstName = FirstNameBox.Text.Trim(),
                LastName = LastNameBox.Text.Trim(),
                Email = email ?? string.Empty,
                PhoneNumber = phone ?? string.Empty,
                CardNumber = cardNumber,
                Notes = NotesBox.Text?.Trim() ?? string.Empty,
                IsActive = true
            };

            DialogResult = true;
            Close();
        }
    }
}

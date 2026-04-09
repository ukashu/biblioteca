using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace biblioteca.Models
{
    public class User : INotifyPropertyChanged
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _cardNumber = string.Empty;
        private string _notes = string.Empty;
        private bool _isActive = true;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        public string CardNumber
        {
            get => _cardNumber;
            set { _cardNumber = value; OnPropertyChanged(); }
        }

        public string Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }

        public bool IsActive
        {
            get => _isActive;
            set { _isActive = value; OnPropertyChanged(); }
        }

        public User() { }

        public User(string firstName, string lastName, string email, string phoneNumber, string cardNumber, string notes)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            CardNumber = cardNumber;
            Notes = notes;
        }

        public User Clone()
        {
            return new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                CardNumber = CardNumber,
                Notes = Notes,
                IsActive = IsActive
            };
        }

        public void CopyFrom(User other)
        {
            FirstName = other.FirstName;
            LastName = other.LastName;
            Email = other.Email;
            PhoneNumber = other.PhoneNumber;
            CardNumber = other.CardNumber;
            Notes = other.Notes;
            IsActive = other.IsActive;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

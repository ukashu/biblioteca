using biblioteca.MVVM;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace biblioteca.ViewModels
{
    public class ReturnBooksViewModel : INotifyPropertyChanged
    {
        private string _inputValue;

        public string Title => "Return Books";

        public string InputValue
        {
            get => _inputValue;
            set
            {
                _inputValue = value;
                OnPropertyChanged(nameof(InputValue));
            }
        }

        public ObservableCollection<string> ReturnedBooks { get; set; }

        public RelayCommand AddBookCommand => new RelayCommand(
            execute => AddBook(),
            canExecute => !string.IsNullOrWhiteSpace(InputValue)
        );

        public RelayCommand ReturnBooksCommand => new RelayCommand(
            execute => ReturnBooks()
        );

        public ReturnBooksViewModel()
        {
            ReturnedBooks = new ObservableCollection<string>();
        }

        private void AddBook()
        {
            ReturnedBooks.Add(InputValue);
            InputValue = string.Empty;
        }

        private void ReturnBooks()
        {
            MessageBox.Show("Zwrócono książki");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
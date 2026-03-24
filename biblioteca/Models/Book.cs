using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace biblioteca.Models
{
    public class Book : INotifyPropertyChanged
    {
        private string _title;
        private string _author;
        private int _publicationYear;
        private string _genre;
        private string _signature;
        private string _description;
        private bool _isAvailable = true;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Author
        {
            get => _author;
            set { _author = value; OnPropertyChanged(); }
        }

        public int PublicationYear
        {
            get => _publicationYear;
            set { _publicationYear = value; OnPropertyChanged(); }
        }

        public string Genre
        {
            get => _genre;
            set { _genre = value; OnPropertyChanged(); }
        }

        public string Signature
        {
            get => _signature;
            set { _signature = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            set { _isAvailable = value; OnPropertyChanged(); }
        }
        public Book() { }

        public Book(string title, string author, int publicationYear, string genre, string signature, string description)
        {
            Title = title;
            Author = author;
            PublicationYear = publicationYear;
            Genre = genre;
            Signature = signature;
            Description = description;
        }

        public Book Clone()
        {
            return new Book
            {
                Title = Title,
                Author = Author,
                PublicationYear = PublicationYear,
                Genre = Genre,
                Signature = Signature,
                Description = Description,
                IsAvailable = IsAvailable
            };
        }

        public void CopyFrom(Book other)
        {
            Title = other.Title;
            Author = other.Author;
            PublicationYear = other.PublicationYear;
            Genre = other.Genre;
            Signature = other.Signature;
            Description = other.Description;
            IsAvailable = other.IsAvailable;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

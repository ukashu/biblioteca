using biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace biblioteca.ViewModels
{
    public class AddBookViewModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }
        public string Genre { get; set; }
        public string Signature { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; } = true;

        public string ErrorMessage { get; set; }

        public Book CreatedBook { get; private set; }

        private void AddBook()
        {
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(Title))
            {
                ErrorMessage = "Tytuł jest wymagany.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Author))
            {
                ErrorMessage = "Autor jest wymagany.";
                return;
            }

            if (!int.TryParse(Year, out int publicationYear))
            {
                ErrorMessage = "Niewłaściwy rok publikacji.";
                return;
            }

            string signature = Signature?.Trim();
            if (string.IsNullOrWhiteSpace(signature) || !System.Text.RegularExpressions.Regex.IsMatch(signature, @"^[a-zA-Z0-9\-]+$"))
            {
                ErrorMessage = "Sygnatura/ISBN jest wymagana i może składać się tylko z liter, cyfr i myślników.";
                return;
            }

            CreatedBook = new Book
            {
                Title = Title.Trim(),
                Author = Author.Trim(),
                PublicationYear = publicationYear,
                Genre = Genre.Trim(),
                Signature = Signature?.Trim(),
                Description = Description.Trim(),
                IsAvailable = IsAvailable
            };

        }
    }
}

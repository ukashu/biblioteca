using System;
using System.Collections.Generic;
using System.Text;

namespace biblioteca.Models
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
        public string Genre { get; set; }
        public string Signature { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; } = true;

        public Book(string title, string author, int publicationYear, string genre, string signature, string description)
        {
            Title = title;
            Author = author;
            PublicationYear = publicationYear;
            Genre = genre;
            Signature = signature;
            Description = description;
        }
    }
}

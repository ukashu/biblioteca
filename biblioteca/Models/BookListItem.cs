using System;

namespace biblioteca.Models
{
    public class BookListItem
    {
        public Book Book { get; set; }

        public int Id => Book.Id;
        public string Title => Book.Title;
        public string Author => Book.Author;
        public int PublicationYear => Book.PublicationYear;
        public bool IsAvailable => Book.IsAvailable;
        public string Signature => Book.Signature;
        public string Description => Book.Description;
        public string Genre => Book.Genre;

        public DateTime? BorrowDate { get; set; }

        public bool IsOverdue =>
            BorrowDate.HasValue &&
            BorrowDate.Value < DateTime.Today.AddDays(-30);
    }
}
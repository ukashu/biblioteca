using System;

namespace biblioteca.Models
{
    public class Loan
    {
        public int Id { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;

        public int BookId { get; set; }
        public virtual Book Book { get; set; } = null!;

        public string UserFullName => $"{User?.FirstName} {User?.LastName} ".Trim();
    }
}
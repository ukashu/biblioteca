using System;

namespace biblioteca.Models
{
    public class Loan
    {
        public string BookTitle { get; set; }
        public string UserName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
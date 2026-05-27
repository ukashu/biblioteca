using biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace biblioteca.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "library.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}

using biblioteca.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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

            optionsBuilder.UseSqlite("Data Source=library.db");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}

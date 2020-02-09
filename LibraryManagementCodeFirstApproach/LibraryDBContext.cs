using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementCodeFirstApproach
{
    class LibraryDBContext : DbContext
    {
        public LibraryDBContext() : base("LibraryDataBase")
        {
            
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BookRepository> BookRepositories { get; set; }
        public DbSet<CustomerRepository> CustomerRepositories { get; set; }
    }
}

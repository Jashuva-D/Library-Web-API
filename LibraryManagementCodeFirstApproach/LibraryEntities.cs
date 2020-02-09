using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementCodeFirstApproach
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public string BookID { get; set; }
        public string Title { get; set; }
        public string GenreType { get; set; }

        public virtual List<Author> Authors { get; set; }
        public Book()
        {
            Authors = new List<Author>();
        }
    }
    [Table("Author")]
    public class Author
    {
        [Key]
        public string AuthorID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public virtual List<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }
    }
    [Table("Publisher")]
    public class Publisher
    {
        [Key]
        public string PublisherID { get; set; }
        public string Name { get; set; }
        public long ContactNumber { get; set; }
    }
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }
    [Table("BookRepository")]
    public class BookRepository
    {
        [Key,Column(Order =1), ForeignKey("Book")]
        public  string BookID { get; set; }
        public virtual Book Book { get; set; }

        [Key,Column(Order =2),ForeignKey("Publisher")]
        public string PublisherID { get; set; }
        public virtual Publisher Publisher { get; set; }

        [Key,Column(Order =3)]
        public string Edition { get; set; }

        public int NumberOfCopies { get; set; }
    }
    [Table("CustomerRepository")]
    public class CustomerRepository
    {
        [Key,Column(Order =1),ForeignKey("Customer")]
        public string CustomerID { get; set; }
        public Customer Customer { get; set; }

        [Key,Column(Order =2),ForeignKey("Book")]
        public string BookID { get; set; }
        public Book Book { get; set; }

        public DateTime IssuedDate { get; set; }
        public DateTime ReceivedDate { get; set; }
    }
}

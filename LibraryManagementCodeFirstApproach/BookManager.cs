using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementCodeFirstApproach
{
    public class BookManager
    {
        public string AddBook(BookDTO bookDTO)
        {
            if (string.IsNullOrEmpty(bookDTO.GenreType))
                throw new InvalidBookException("Invalid GenreType");
            if (string.IsNullOrEmpty(bookDTO.Title))
                throw new InvalidBookException("Invalid Title");
            if (bookDTO.publisherID == null)
                throw new InvalidBookException("Invalid Publisher");
            if (bookDTO.AuthorIDlist.Count() == 0)
                throw new InvalidBookException("Book Must contain atleast one author");


            Book book;
            using (var context = new LibraryDBContext())
            {
                book = context.Books.Include("Authors").Where(bookL => bookL.Title == bookDTO.Title).Select(bookL => bookL).SingleOrDefault();
                if (book == null)
                {
                    book = new Book();
                    book.Title = bookDTO.Title;
                    book.GenreType = bookDTO.GenreType;
                    book.BookID = bookDTO.generateID();
                }

                foreach (var authorid in bookDTO.AuthorIDlist)
                {
                    Author author = context.Authors.Include("Books").Where(authorL => authorL.AuthorID == authorid).Select(authorL => authorL).Single();
                    book.Authors.Add(author);
                }
                BookRepository bookrepo = new BookRepository();
                bookrepo.Book = book;
                Publisher publisher = context.Publishers.Where(publisherL => publisherL.PublisherID == bookDTO.publisherID).Select(publisherL => publisherL).Single();
                bookrepo.Publisher = publisher;
                bookrepo.Edition = bookDTO.Edition;
                bookrepo.NumberOfCopies = bookDTO.NumberOfCopies;

                context.BookRepositories.Add(bookrepo);
                context.SaveChanges();
            }

            return book.BookID;
        }

        /// <summary>
        /// To get the book from the database by reading the title of the book
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Book getBook(string title)
        {
            Book book;
            using (var context = new LibraryDBContext())
            {
                book = context.Books.Include("Authors").Where(bookL => bookL.Title == title).Select(bookL => bookL).FirstOrDefault();
            }
            return book;
        }
        /// <summary>
        /// To get the book Repositories of the given book 
        /// </summary>
        /// <param name="BookID"></param>
        /// <returns></returns>
        public IEnumerable<BookRepository> getBookRepositories(string BookID)
        {
            IEnumerable<BookRepository> bookrepositories;
            using (var context = new LibraryDBContext())
            {
                bookrepositories = (from bookrepo in context.BookRepositories
                                    where bookrepo.BookID == BookID
                                    select bookrepo).ToList();
                context.SaveChanges();
            }

            return bookrepositories;
        }
        public IEnumerable<Book> GetBYGenre(string genretype)
        {
            IEnumerable<Book> books;
            var context = new LibraryDBContext();
            books = (from book in context.Books
                     where book.GenreType == genretype
                     select book).ToList();

            return books;
        }
        public IEnumerable<Book> GetAllBooks()
        {
            IEnumerable<Book> books;
            using (var context = new LibraryDBContext())
            {
                books = context.Books.Include("Authors").ToList();
                context.SaveChanges();
            }
            return books;
        }
        public IEnumerable<Book> GetByAuthor(string authorID)
        {
            //IEnumerable<Book> books = GetAllBooks().SelectMany(book => book.Authors.Where(author => author.AuthorID == name), (book, author) => book).ToList();

            //return books;
            var context = new LibraryDBContext();
            IEnumerable<Book> books = context.Books.Include("Authors").SelectMany(book => book.Authors.Where(author => author.AuthorID == authorID), (book, author) => book);
            context.SaveChanges();

            return books;

        }
        public IEnumerable<Book> getByPublisher(string publisherID)
        {
            IEnumerable<BookRepository> bookrepos;
            var context = new LibraryDBContext();
            {
                bookrepos = context.BookRepositories.Include("Book").Where(bookrepo => bookrepo.Publisher.PublisherID == publisherID).Select(book => book).ToList();
            }
            IEnumerable<Book> books = bookrepos.Where(bookrepo => bookrepo.Publisher.PublisherID == publisherID).Select(bookrepo => bookrepo.Book);
            return books;
        }
        public bool AddEdition(string BookID, string PublisherID, string edition, int numberOfCopies)
        {
            using (var context = new LibraryDBContext())
            {
                BookRepository bookrepo = new BookRepository();
                bookrepo.Book = context.Books.Include("Authors").Where(book => book.BookID == BookID).Single();
                bookrepo.Publisher = context.Publishers.Where(publisher => publisher.PublisherID == PublisherID).Single();
                bookrepo.Edition = edition;
                bookrepo.NumberOfCopies = numberOfCopies;
                context.BookRepositories.Add(bookrepo);
                context.SaveChanges();
            }
            return true;
        }
        public BookRepository GetBookByEdition(string bookID, string Edition)
        {
            var context = new LibraryDBContext();
            BookRepository book = context.BookRepositories.Include("Book").Where(bookrepo => (bookrepo.BookID == bookID && bookrepo.Edition == Edition))
                                                                .Select(bookrepo => bookrepo).SingleOrDefault();
            return book;
        }

        public bool deleteBookRepository(string bookID, string Edition)
        {

            var context = new LibraryDBContext();
            BookRepository bookRepository = context.BookRepositories.Include("Book")
                                            .Where(bookrepo => (bookrepo.BookID == bookID && bookrepo.Edition == Edition))
                                            .Select(bookrepo => bookrepo)
                                            .SingleOrDefault();
            if (bookRepository != null)
            {
                context.BookRepositories.Remove(bookRepository);
                context.SaveChanges();
                return true;
            }
            else
                return false;
        }
        public bool deleteBookwithAllEditions(string bookID)
        {
            var context = new LibraryDBContext();
            Book book = context.Books.Include("Authors").Where(bookL => bookL.BookID == bookID)
                                                      .Select(bookL => bookL)
                                                      .SingleOrDefault();
            if (book != null)
            {
                context.Books.Remove(book);
                context.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}

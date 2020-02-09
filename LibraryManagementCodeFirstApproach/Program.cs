using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementCodeFirstApproach
{
    class Program
    {
        static BookManager bookmanager = new BookManager();
        static AuthorManager authormanager = new AuthorManager();
        static PublisherManager publishermanager = new PublisherManager();
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter your option");
                Console.WriteLine("1.AddBook");
                Console.WriteLine("2.GetAllBooks");
                Console.WriteLine("3.GetByGenre");
                Console.WriteLine("4.GetByAuthor");
                Console.WriteLine("5.GetByPublisher");
                Console.WriteLine("6.Add Edition to the existing book");
                Console.WriteLine("7.Delete Book By Edition");
                Console.WriteLine("8.Delete Book with all its editions");
                Console.WriteLine("9.Exit");
                Console.Write("Enter your choice : ");
                int choice = 0;
                int.TryParse(Console.ReadLine(), out choice);
                bool continuecheck = true;
                switch (choice)
                {
                    case 1:
                        try
                        {
                            AddBookOption();
                        }
                        catch(InvalidBookException exception)
                        {
                            Console.WriteLine(exception.Message);
                        }
                        catch (InvalidPublisherException exception)
                        {
                            Console.WriteLine(exception.Message);
                        }
                        catch (InvalidAuthorException exception)
                        {
                            Console.WriteLine(exception.Message);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception.Message);
                        }

                        break;
                    case 2:
                        listAllBooksOption();
                        break;
                    case 3:
                        GetBookByGenreOption();
                        break;
                    case 4:
                        GetByAuthorOption();
                        break;
                    case 5:
                        GetByPublisherOption();
                        break;
                    case 6:
                        AddEditionOption();
                        break;
                    case 7:
                        DeleteBookByEdition();
                        break;
                    case 8:
                        DeleteBookWithAllEditions();
                        break;
                    case 9:
                        Console.WriteLine("THANK YOU");
                        continuecheck = false;
                        break;
                    default:
                        Console.WriteLine("\n You have entered wrong option");
                        break;

                }
                if (continuecheck)
                {
                    Console.WriteLine("Do you want to continue Yes/No");
                    if (string.Equals("No", Console.ReadLine(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        Console.WriteLine("THANKYOU");
                        break;
                    }
                }
                else
                    break;
                
            }
           
        }
        /// <summary>
        /// This method reads the data of the book and sends the data to dto classes and call addbook method in bookmanager
        /// </summary>

        public static void AddBookOption()
        {
            BookDTO bookdto = new BookDTO();
            AuthorDTO authorDTO = new AuthorDTO();
            PublisherDTO publisherDTO = new PublisherDTO();

            Console.Write("\nEnter the title of the book : ");
            bookdto.Title = Console.ReadLine();

            Console.Write("\nEnter GenreType : ");
            bookdto.GenreType = Console.ReadLine();
            while (true)
            {
                Console.Write("\nEnter Author firstName :");
                authorDTO.FirstName = Console.ReadLine();
                Console.Write("\nEnter Author lastName :");
                authorDTO.LastName = Console.ReadLine();

                Author author = authormanager.GetAuthorByName(authorDTO.FirstName);
                if (author == null)
                {
                   bookdto.AuthorIDlist.Add( authormanager.AddAuthor(authorDTO));
                }
                else
                    bookdto.AuthorIDlist.Add(author.AuthorID);
                Console.Write("\n Do you want to enter more authors : ");
                if (string.Equals("No", Console.ReadLine(), StringComparison.CurrentCultureIgnoreCase))
                    break;
            }

            Console.Write("\n Enter name of the Publisher : ");
            publisherDTO.Name = Console.ReadLine();
            Console.Write("\n Enter mobile number of the publisher : ");
            publisherDTO.ContactNumber = long.Parse(Console.ReadLine());

            Publisher publisher=publishermanager.GetPublisher(publisherDTO.Name);
            if (publisher == null)
            {
                bookdto.publisherID = publishermanager.AddPublisher(publisherDTO);
            }
            else
                  bookdto.publisherID = publisher.PublisherID;
            Console.Write("\n Enter edition of the Book : ");
            bookdto.Edition = Console.ReadLine();
            Console.Write("\n Enter number of copies of the book : ");
            int numberofbooks = 0;
            int.TryParse(Console.ReadLine(), out numberofbooks);
            bookdto.NumberOfCopies = numberofbooks;

            if (bookmanager.AddBook(bookdto) != null)
                Console.WriteLine("Book added successfully");
            else
                Console.WriteLine("SOMETHING WENT WRONG IN ADDING BOOK");
        }
        /// <summary>
        /// This method get the all books from the list and displays the books
        /// </summary>
        public static void GetBookByGenreOption()
        {
            
            Console.WriteLine("Enter genre type");
            string genreType = Console.ReadLine();
            IEnumerable<Book> books= bookmanager.GetBYGenre(genreType);
            if(!displaybookRepository(books))
                Console.WriteLine("No books found");
                
        }
        public static bool displaybookRepository(IEnumerable<Book> books)
        {
            bool booksFound = false;
            foreach (var book in books)
            {
                booksFound = true;
                Console.WriteLine("\t Title \t: " + book.Title);
                Console.WriteLine("\t Genre \t: " + book.GenreType);
                Console.Write("\t Authors\t: ");
                for (int i = 0; i < book.Authors.Count(); i++)
                {
                    if (i != 0)
                        Console.Write(",");
                    Console.Write(authormanager.GetAuthorByID(book.Authors[i].AuthorID).firstName);
                }


                IEnumerable<BookRepository> bookrepos = bookmanager.getBookRepositories(book.BookID);
                foreach (var bookrepo in bookrepos)
                {

                    Console.WriteLine("\n\t Publisher\t: " + publishermanager.GetPublisherByID(bookrepo.PublisherID).Name);
                    Console.WriteLine("\t Edition \t: " + bookrepo.Edition);
                    Console.WriteLine("\t No.Of Copies\t: " + bookrepo.NumberOfCopies);
                }
            }
            return booksFound;
        }
        public static void listAllBooksOption()
        {
            IEnumerable<Book> books=bookmanager.GetAllBooks();
            if (!displaybookRepository(books))
                Console.WriteLine("No Books Found");
        }
        /// <summary>
        /// This method is for getting the books that are available in the database by using author name
        /// </summary>
        public static void GetByAuthorOption()
        {
            Console.WriteLine("Enter name of the author ");
            string name = Console.ReadLine();
            Author author=authormanager.GetAuthorByName(name);

            var books = bookmanager.GetByAuthor(author.AuthorID);
            if (!displaybookRepository(books))
                Console.WriteLine("No books Found");
        }
        public static void GetByPublisherOption()
        {
            Console.WriteLine("Enter name of the Publisher");
            string name=Console.ReadLine();
            Publisher publisher=publishermanager.GetPublisher(name);
            IEnumerable<Book> books = bookmanager.getByPublisher(publisher.PublisherID);
            if (!displaybookRepository(books))
                Console.WriteLine("No books Found");

        }
        /// <summary>
        /// This method is for adding the new edition of the already available book
        /// </summary>
        public static void AddEditionOption()
        {
            Console.WriteLine("Enter name of the Book");
            string bookname = Console.ReadLine();
            Book book = bookmanager.getBook(bookname);
            Console.WriteLine("Enter edition of the book");
            string edition = Console.ReadLine();
            Console.WriteLine("Enter number of Books");
            int numberofbooks = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter name of the publisher");
            string name = Console.ReadLine();
            Publisher publisher = publishermanager.GetPublisher(name);
            if (publisher == null)
            {
                PublisherDTO publisherdto = new PublisherDTO();
                publisherdto.Name = name;
                Console.WriteLine("Enter contact number");
                publisherdto.ContactNumber = long.Parse(Console.ReadLine());
                string publisherID=publishermanager.AddPublisher(publisherdto);
                bookmanager.AddEdition(book.BookID, publisherID, edition, numberofbooks);
            }
            if (bookmanager.AddEdition(book.BookID, publisher.PublisherID, edition, numberofbooks))
                Console.WriteLine("Book added successfully");
        }
        /// <summary>
        /// This method deletes the book by specific edition
        /// </summary>
        public static void DeleteBookByEdition()
        {
            Console.WriteLine("Enter title of the book");
            string name = Console.ReadLine();
            Book book = bookmanager.getBook(name);
            if (book == null)
            {
                Console.WriteLine("You entered book is not available in the list");
                return;
            }
            Console.WriteLine("Enter edition of the book");
            string edition = Console.ReadLine();
            if (bookmanager.deleteBookRepository(book.BookID, edition))
                Console.WriteLine("Book Deleted Successfully");
            else
                Console.WriteLine("Book does not found");

        }
        /// <summary>
        /// This method deletes the entire book
        /// </summary>
        public static void DeleteBookWithAllEditions()
        {
            Console.WriteLine("Enter title of the book");
            string title = Console.ReadLine();
            Book book = bookmanager.getBook(title);
            if (book == null)
            {
                Console.WriteLine("Book does not exist");
                return;
            }
            else
            {
                if (bookmanager.deleteBookwithAllEditions(book.BookID))
                    Console.WriteLine("Book deleted successfully");
            }

        }
    }
}

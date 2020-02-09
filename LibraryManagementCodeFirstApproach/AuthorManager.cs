using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementCodeFirstApproach
{
    public class AuthorManager
    {
        public string AddAuthor(AuthorDTO authorDTO)
        {
            if (string.IsNullOrEmpty(authorDTO.FirstName))
                throw new InvalidAuthorException("Invalid FirstName");
            if (string.IsNullOrEmpty(authorDTO.LastName))
                throw new InvalidAuthorException("Invalid LastName");
            Author author = new Author();
            author.firstName = authorDTO.FirstName;
            author.lastName = authorDTO.LastName;
            author.AuthorID = authorDTO.generateID();

            using(var context=new LibraryDBContext())
            {
                context.Authors.Add(author);
                context.SaveChanges();
            }
            return author.AuthorID;
        }
        public Author GetAuthorByName(string name)
        {
            Author author;
            using(var context=new LibraryDBContext())
            {
                author = context.Authors.Include("Books").SingleOrDefault(auth => (auth.firstName + auth.lastName).Contains(name));
            }
            return author;
        }
        public Author GetAuthorByID(string ID)
        {
            Author author;
            using(var context=new LibraryDBContext())
            {
                IEnumerable<Author> authors = (from authorL in context.Authors
                                              where authorL.AuthorID == ID
                                             select authorL).ToList();
                author = authors.FirstOrDefault();
            }
            return author;
        }
        public IEnumerable<Book> GetBooks(string authorID)
        {
            IEnumerable<Book> books;
            using(var context=new LibraryDBContext())
            {
                books = context.Authors.Include("Books").SelectMany(author => author.Books.Select(book => book)).ToList();
                
            }
            return books;
        }
    }
}

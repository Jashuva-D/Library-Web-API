using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementCodeFirstApproach
{
    class PublisherManager
    {
        public string AddPublisher(PublisherDTO publisherDTO)
        {
            if (string.IsNullOrEmpty(publisherDTO.Name))
                throw new InvalidPublisherException("INVALID PUBLISHER NAME ");
            if (publisherDTO.ContactNumber == 0)
                throw new InvalidPublisherException("INVALID CONTACT NUMBER");
            Publisher publisher = new Publisher();
            publisher.Name = publisherDTO.Name;
            publisher.ContactNumber = publisherDTO.ContactNumber;

            publisher.PublisherID = publisherDTO.generateID();

            using (var context = new LibraryDBContext())
            {
                context.Publishers.Add(publisher);
                context.SaveChanges();
            }
            return publisher.PublisherID;
        }

        public Publisher GetPublisher(string name)
        {
            Publisher publisher;
            using(var context=new LibraryDBContext())
            {
                IEnumerable<Publisher> publishers = from pblr in context.Publishers
                                                    where pblr.Name.Contains(name)
                                                    select pblr;
                publisher = publishers.FirstOrDefault();
            }
            return publisher;
        }
        public Publisher GetPublisherByID(string ID)
        {
            Publisher publisher;
            using (var context = new LibraryDBContext())
            {
                IEnumerable<Publisher> publishers = from pblr in context.Publishers
                                                    where pblr.PublisherID==ID
                                                    select pblr;
                publisher = publishers.FirstOrDefault();
            }
            return publisher;
        }
    }
}

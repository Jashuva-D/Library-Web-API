using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementCodeFirstApproach
{
    class InvalidBookException : Exception
    {
        public InvalidBookException(string msg):base(msg)
        {

        }
    }
    class InvalidAuthorException : Exception
    {
        public InvalidAuthorException(string msg)  : base(msg)
        {

        }   
    }
    class InvalidPublisherException : Exception
    {
        public InvalidPublisherException(string msg) : base(msg)
        {
                
        }
        
    }
    class InvalidMemberException : Exception
    {
        public InvalidMemberException(string msg) : base(msg)
        {

        }
    }
}

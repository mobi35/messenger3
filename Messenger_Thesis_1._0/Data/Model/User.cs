using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Model
{
    public class User
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Role { get; set; }
        public string EmailAddress { get; set; }
        public int LoginTries { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime DateRegistered { get; set; }

    }
}

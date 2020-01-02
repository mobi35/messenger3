using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Model
{
    public class Client
    {
        public int ClientNumber { get; set; }
        public string Name { get; set; }
        public string Representative { get; set; }
        public string Email { get; set; }
        public int ContactNumber { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime DateRegistered { get; set; }
        public string Password { get; set; }
        public string PaymentTerms { get; set; }

    }
}

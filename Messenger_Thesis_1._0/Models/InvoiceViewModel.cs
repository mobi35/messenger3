using Messenger_Thesis_1._0.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Models
{
    public class InvoiceViewModel
    {
        public User Users { get; set; }

        public Letter Letters { get; set; }

        public Project Projects { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Model
{
    public class Project
    {

        public int ProjectID { get; set; }
        public string ClientID { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Model
{
    public class Letter
    {
        public string ProjectID { get; set; }
        public string ReceiverName { get; set; }
        public string SenderName { get; set; }
        public string LocationOfDelivery { get; set; }
        public int Price { get; set; }
        public string MessengerName { get; set; }

        public DateTime DateOfDelivery { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Model
{
    public class Letter
    {
        [Key]
        public int LetterID { get; set; }
        public int ProjectID { get; set; }
        public string ReceiverName { get; set; }
        public string SenderName { get; set; }
        public string LocationOfDelivery { get; set; }
        public float Price { get; set; }
        public string MessengerName { get; set; }

        public DateTime DateOfDelivery { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime DateIns { get; set; }

        public int DeliveryID { get; set; }
    }
}

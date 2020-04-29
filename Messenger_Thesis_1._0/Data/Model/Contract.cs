using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Model
{
    public class Contract
    {
        public int ContractID { get; set; }
        public string CompanyName { get; set; }

        public int Quantity { get; set; }
        public DateTime StartDuration { get; set; }
        public DateTime EndDuration { get; set; }

        public int YearsOfDuration { get; set; }

        public float PricePerQuantity { get; set; }
        public string ClientID { get; set; }

        public bool Archived { get; set; }

        public bool UserArchive { get; set; }

    }
}

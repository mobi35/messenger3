using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Model
{
    public class Project
    {

        [Key]
        public int ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public string ClientName { get; set; }
        public string Status { get; set; }

        public string Area { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string ProjectName { get; set; }
        [NotMapped]
        public IFormFile DepositImage { get; set; }

        public string ImageName { get; set; }

        public string Email { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime LastPaymentDate { get; set; }

        public DateTime CurrentDateStart { get; set; }

        public string PaymentTerms { get; set; }

        public int TotalLettersPerMonth { get; set; }

        public string TypeOfTask { get; set; }

        public int  ContractID { get; set; }

        public int Messenger  { get; set; }

        public string ListOfMessenger { get; set; }
    }
}

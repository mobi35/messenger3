﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Model
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }


        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public string Role { get; set; }

        public string Address { get; set; }

        public string Phonenumber { get; set; }

        public string CompanyName { get; set; }


        //MESSENGER THINGS

        public int DeliveryID { get; set; }

        public string DeliveryStatus { get; set; }

        public int SOAToDeliver { get; set; }

        public int TotalNumberOfLetters { get; set; }

        public string AccountStatus { get; set; }

        public int PickupDay { get; set; }

        public int TaskNumber { get; set; }

    }
}

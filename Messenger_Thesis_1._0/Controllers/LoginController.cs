using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Messenger_Thesis_1._0.Models;
using Messenger_Thesis_1._0.Data.Model.Interface;
using System.Net.Mail;

namespace Messenger_Thesis_1._0.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository userRepo;

        public LoginController(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [HttpPost]
        public string Logon(string email, string password)
        {

            List<string> errors = new List<string>();
            Utilities util = new Utilities();
            string encodePassword = "";

            if (password == "" || password == null)
            {
                errors.Add("password_required");
             
            }else
            {
                encodePassword = util.base64Encode(password);
            }


            //Check if the password matched
            var user =  userRepo.FindUser(a => a.Email == email && a.Password == encodePassword);

            //validations..

           

            if (email == null || email == "")
            {
                errors.Add( "email_required" );
            }
            else if (!IsValidEmail(email))
            {
                errors.Add("email_error");
            }

         

            if (user == null)
            {
                errors.Add("error");
            }

            if (errors.Count == 0)
            {


                if(user.Role == "Admin")
                RedirectToAction("", "");
                
                else if (user.Role == "Messenger")
                 RedirectToAction("", "");

                else if (user.Role == "Accountant")
                    RedirectToAction("", "");

                else if (user.Role == "Client")
                    RedirectToAction("", "");

            }else
            {
                string errorList = string.Join(",", errors);
                return errorList;
            }


            return "";
        }

     
    }
}

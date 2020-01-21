using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Messenger_Thesis_1._0.Models;
using Messenger_Thesis_1._0.Data.Model.Interface;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;

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

      

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("Email", "");
            HttpContext.Session.SetString("Image", "");
            HttpContext.Session.SetString("FullName", "");
            HttpContext.Session.SetString("Role", "");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logon(string email, string password)
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


         
            if (user != null)
            {
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("FullName", user.FirstName + " " + user.LastName);
                HttpContext.Session.SetString("Image", user.ImageName);
                HttpContext.Session.SetString("Role", user.Role);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index","Home");
            }

          
        }

     
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Messenger_Thesis_1._0.Models;
using Messenger_Thesis_1._0.Data.Model;
using Messenger_Thesis_1._0.Data;
using Messenger_Thesis_1._0.Data.Model.Interface;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;

namespace Messenger_Thesis_1._0.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserRepository _userRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        public UserController(IUserRepository userRepo, IHostingEnvironment hostingEnvironment)
        {
            _userRepo = userRepo;
            _hostingEnvironment = hostingEnvironment;
        }
        MessengerDBContext database;

        public List<User> Users()
        {
            return _userRepo.GetAll().ToList();
        }

        public IActionResult Index()
        {


            return View(Users());
        }

        public IActionResult Create()
        {
            return View(Users());
        }

        [HttpGet]
        public JsonResult GetUserList(int page = 1 , string query = ""   )
        {


            int count = page * 10;
          
            var users = _userRepo.GetAll().OrderByDescending(a => a.UserID).ToList();

           
            List<User> sortedUser = new List<User>();
            for (int i = 0; i < users.Count();i++)
            {
                if(page == 1 && i < 10) {
                    sortedUser.Add(users[i]);
                }
                else if (i > (count - 10) && count >= i)
                {
                    sortedUser.Add(users[i]);
                }
            }

            return Json(sortedUser);
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

            public string SendRegisteredData( User user)
            {
                List<string> errors = new List<string>();
                if(user.Email == null)
                    errors.Add("no_email");
                else if (!IsValidEmail(user.Email))
                    errors.Add("invalid_email");
                else if(_userRepo.FindUser(a => a.Email == user.Email) != null)
                    errors.Add("existing_email");

                if (user.Password ==  null|| user.ConfirmPassword == null)
                    errors.Add("no_password");
                else if (user.ConfirmPassword != user.Password)
                    errors.Add("password_not_match");
             

                if (user.FirstName == null)
                    errors.Add("firstname_required");
                else if (user.FirstName.Any(char.IsDigit) ) 
                    errors.Add("firstname_not_letter");
                else if (user.FirstName.Length > 50)
                    errors.Add("firstname_max_letter");

                if (user.LastName == null)
                    errors.Add("lastname_required");
                else if (user.LastName.Any(char.IsDigit))
                    errors.Add("lastname_not_letter");
                else if (user.LastName.Length > 50)
                    errors.Add("lastname_max_letter");


                int birthdayDifference = DateTime.Now.Year - user.BirthDate.Year;
                if (user.BirthDate.Year == 0001)
                    errors.Add("no_birthdate");
               else  if (birthdayDifference <= 17 )
                    errors.Add("invalid_birthdate");

                if (user.Image == null)
                    errors.Add("no_picture");


                if (errors.Count != 0)
                {
                    string errorList = string.Join(",", errors);
                    return errorList;
                }


                if (user.Image != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    var uniqueName = Guid.NewGuid().ToString() + "_" + user.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueName);
                    user.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                    user.ImageName = uniqueName;
                }
                Utilities util = new Utilities();
                user.Password = util.base64Encode(user.Password);
         
                _userRepo.Create(user);


                return "";
            }


        public IActionResult Login()
        {
            return View();
        }


        public User GetUserDetails(int id)
        {

            var user = _userRepo.FindUser(a => a.UserID == id);

            return user;
        }

        [HttpPost]
        public string UpdateData(int userId, string password, string cpassword, string firstName, string lastName, string role, IFormFile image )
        {
            var user = _userRepo.FindUser(a => a.UserID == userId);

            List<string> errors = new List<string>();
         

            if (password == null || cpassword == null)
                errors.Add("no_password");
            else if (cpassword != password)
                errors.Add("password_not_match");


            if (firstName == null)
                errors.Add("firstname_required");
            else if (firstName.Any(char.IsDigit))
                errors.Add("firstname_not_letter");
            else if (firstName.Length > 50)
                errors.Add("firstname_max_letter");

            if (lastName == null)
                errors.Add("lastname_required");
            else if (lastName.Any(char.IsDigit))
                errors.Add("lastname_not_letter");
            else if (lastName.Length > 50)
                errors.Add("lastname_max_letter");

         

            if (errors.Count != 0)
            {
                string errorList = string.Join(",", errors);
                return errorList;
            }


            if (image != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                var uniqueName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueName);
                image.CopyTo(new FileStream(filePath, FileMode.Create));
                user.ImageName = uniqueName;
            }

            if (errors.Count == 0) {
                Utilities util = new Utilities();
                user.Password = util.base64Encode(password);
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Role = role;

                _userRepo.Update(user);

            }
            

            return "";
        }


    }
}

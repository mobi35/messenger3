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
            var users = _userRepo.GetAll().Where(a => a.Role != "Client").OrderByDescending(a => a.UserID).ToList();
           
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

        [HttpGet]

        public JsonResult SessionProfile()
        {
            string email = HttpContext.Session.GetString("Email").ToString();
            var profile = _userRepo.GetAll().Where(a => a.Email == email).ToList();

            return Json(profile);
        }

        [HttpGet]

        public JsonResult UserProfile(int id)
        {
            
            var profile = _userRepo.GetAll().Where(a => a.UserID == id).ToList();

            return Json(profile);
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
                    HttpContext.Session.SetString("Image", user.ImageName);
            }
                Utilities util = new Utilities();
                user.Password = util.base64Encode(user.Password);
            if(_userRepo.FindUser(a => a.Email == user.Email) == null) { 
                _userRepo.Create(user);
            }


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
        public string UpdateData(User userModel )
        {
            var user = new User();

            List<string> errors = new List<string>();
         
            if(userModel.UserID != 0)
            {
                user = _userRepo.FindUser(a => a.UserID == userModel.UserID);
            }else
            {
                user = _userRepo.FindUser(a => a.Email  == userModel.Email);
            }

             if (userModel.ConfirmPassword != userModel.Password)
                errors.Add("password_not_match");


            if (userModel.FirstName == null)
                errors.Add("firstname_required");
            else if (userModel.FirstName.Any(char.IsDigit))
                errors.Add("firstname_not_letter");
            else if (userModel.FirstName.Length > 50)
                errors.Add("firstname_max_letter");

            if (userModel.LastName == null)
                errors.Add("lastname_required");
            else if (userModel.LastName.Any(char.IsDigit))
                errors.Add("lastname_not_letter");
            else if (userModel.LastName.Length > 50)
                errors.Add("lastname_max_letter");

         

            if (errors.Count != 0)
            {
                string errorList = string.Join(",", errors);
                return errorList;
            }


            if (userModel.Image != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                var uniqueName = Guid.NewGuid().ToString() + "_" + userModel.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueName);
                userModel.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                user.ImageName = uniqueName;
            }

            if (errors.Count == 0 && userModel.Password != null) {
                Utilities util = new Utilities();
                user.Password = util.base64Encode(userModel.Password);
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Role = userModel.Role;

                _userRepo.Update(user);

            }
            else if (errors.Count == 0 )
            {
                Utilities util = new Utilities();
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
             

                _userRepo.Update(user);
            }
            

            return "";
        }

        public IActionResult Profile()
        {

            return View();
        }


    }
}

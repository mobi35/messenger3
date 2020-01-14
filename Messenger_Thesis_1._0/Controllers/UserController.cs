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

        [HttpPost]

        public string SendRegisteredData( User user)
        {

            if (user.Image != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                var uniqueName = Guid.NewGuid().ToString() + "_" + user.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueName);
                user.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                user.ImageName = uniqueName;
            }
            _userRepo.Create(user);


            return "burat"+ user.Email;
        }


        public IActionResult Login()
        {
            return View();
        }

    


    }
}

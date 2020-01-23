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
    public class BillingController : Controller
    {

        private readonly IUserRepository _userRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IProjectRepository projectRepo;

        public BillingController(IUserRepository userRepo, IHostingEnvironment hostingEnvironment, IProjectRepository projectRepo)
        {
            _userRepo = userRepo;
            _hostingEnvironment = hostingEnvironment;
            this.projectRepo = projectRepo;
        }
        MessengerDBContext database;

        
        public IActionResult Billings()
        {
            return View();
        }

        public string SendDeposit(Project p)
        {
            var project = projectRepo.FindProject(a => a.ProjectID == p.ProjectID);

            if (p.DepositImage != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                var uniqueName = Guid.NewGuid().ToString() + "_" + p.DepositImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueName);
                p.DepositImage.CopyTo(new FileStream(filePath, FileMode.Create));
                project.ImageName = uniqueName;

                projectRepo.Update(project);
                return "";
            }
            else
            {
                return "no_image";
            }






           
        }

        [HttpPost]
        public string Deposit(int id, bool action)
        {

            var project = projectRepo.FindProject(a => a.ProjectID == id);

            if (action)
            {
                project.Status = "Paid";
            }else
            {
                project.ImageName = null;
                project.Status = "Declined";
            }

            projectRepo.Update(project);

            return "";
        }


        public IActionResult Client()
        {

            return View();
        }

        [HttpGet]
        public JsonResult GetBillingList(int page = 1, string query = "")
        {


            int count = page * 10;

            var project = projectRepo.GetAll().OrderByDescending(a => a.ProjectID).ToList();


            List<Project> sortedProject = new List<Project>();
            for (int i = 0; i < project.Count(); i++)
            {
                if (page == 1 && i < 10)
                {
                    sortedProject.Add(project[i]);
                }
                else if (i > (count - 10) && count >= i)
                {
                    sortedProject.Add(project[i]);
                }
            }

            return Json(sortedProject);
        }



    }
}

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
        private readonly ILetterRepository letterRepo;
        private readonly IUserRepository _userRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IProjectRepository projectRepo;

        public BillingController(ILetterRepository letterRepo, IUserRepository userRepo, IHostingEnvironment hostingEnvironment, IProjectRepository projectRepo)
        {
            this.letterRepo = letterRepo;
            _userRepo = userRepo;
            _hostingEnvironment = hostingEnvironment;
            this.projectRepo = projectRepo;
        }
        MessengerDBContext database;

        
        public IActionResult Billings()
        {
            var project = projectRepo.GetAll().ToList();

            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                project = projectRepo.GetAll().OrderByDescending(a => a.ProjectID).ToList();
            }
            else
            {
                var name = HttpContext.Session.GetString("FullName").ToString();
                project = projectRepo.GetAll().Where(a => a.ClientName == name).OrderByDescending(a => a.ProjectID).ToList();
            }


            return View(project);
        }

        [HttpPost]
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



            var date = DateTime.Now;

            if(project.PaymentTerms == "semi")
            {
               date = project.InvoiceDate.AddDays(15);
            }
            else
            {
                date = project.InvoiceDate.AddMonths(1);
            }



            var letters = letterRepo.GetAll().Where(a => a.ProjectID == id && a.DateIns <= date).ToList();



            if (action)
            {
                foreach (var l in letters)
                {
                    l.PaymentDate = DateTime.Now;
                    letterRepo.Update(l);
                }
                project.Status = "Paid";
                project.LastPaymentDate = DateTime.Now;

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
            var userID = int.Parse(HttpContext.Session.GetString("UserID").ToString());
            var getUser = _userRepo.FindUser(a => a.UserID == userID);


            return View(projectRepo.GetAll().Where(a => a.Email == getUser.Email));
        }


        public JsonResult DateFilter(DateTime startDate, DateTime endDate)
        {
            return Json(projectRepo.GetAll().Where(a => a.InvoiceDate >= startDate && a.InvoiceDate <= endDate).OrderByDescending(a => a.ProjectID).ToList());
        }


    }
}

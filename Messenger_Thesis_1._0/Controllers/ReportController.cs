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
using Newtonsoft.Json;

using System.Text;

namespace Messenger_Thesis_1._0.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILetterRepository letterRepo;
        private readonly IProjectRepository projectRepo;
        private readonly IUserRepository _userRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ReportController( ILetterRepository letterRepo , IProjectRepository projectRepo, IUserRepository userRepo, IHostingEnvironment hostingEnvironment)
        {
            this.letterRepo = letterRepo;
            this.projectRepo = projectRepo;
            _userRepo = userRepo;
            _hostingEnvironment = hostingEnvironment;
        }
        MessengerDBContext database;

    
        public IActionResult Index()
        {


            return View();
        }

        //project id
        public IActionResult Invoice(int id)
        {
            var email = projectRepo.GetIdBy(id).Email;
            InvoiceViewModel invoiceVM = new InvoiceViewModel
            {
                Projects = projectRepo.GetIdBy(id),
                Users = _userRepo.FindUser(a => a.Email == email)
            };

            return View(invoiceVM);
        }




    }
}

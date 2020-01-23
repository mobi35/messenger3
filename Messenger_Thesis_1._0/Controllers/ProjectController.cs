using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Messenger_Thesis_1._0.Models;
using Messenger_Thesis_1._0.Data.Model.Interface;
using Messenger_Thesis_1._0.Data.Model;
using Microsoft.AspNetCore.Http;

namespace Messenger_Thesis_1._0.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository projectRepo;
        private readonly IUserRepository userRepo;

        public ProjectController(IProjectRepository projectRepo, IUserRepository userRepo )
        {
            this.projectRepo = projectRepo;
            this.userRepo = userRepo;
        }

        public List<Project> GetProjects()
        {
            return projectRepo.GetAll().ToList();
        }
        public IActionResult Index()
        {
            return View(GetProjects());
        }

        public IActionResult Privacy()
        {
            return View();
        }

   
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpGet]
        public JsonResult GetProjectList(int page = 1, string query = "")
        {


            int count = page * 10;

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


        [HttpPost]
        public string SendNewDelivery(Project project)
        {
            List<string> errors = new List<string>();

            if (project.Quantity == 0)
            {
                errors.Add("no_zero_quantity");
            }   else if (project.Quantity.ToString().Any(char.IsLetter) && project.Quantity.ToString().Any(char.IsPunctuation))
                errors.Add("invalid_quantity");

            if (project.ProjectName == "")
            {
                errors.Add("project_name_required");
            }

            project.ClientName = HttpContext.Session.GetString("FullName").ToString();

            project.Status = "pending";
            project.Price = project.Quantity * 5;
            if (errors.Count != 0)
            {
                string errorList = string.Join(",", errors);
                return errorList;
            }

            projectRepo.Create(project);

            return "";
        }

        public IActionResult Client()
        {

            return View();
        }

        public IActionResult Admin()
        {
            var user = userRepo.GetAll().Where(a => a.Role == "Messenger").ToList();
            return View(user);
        }

    }
}

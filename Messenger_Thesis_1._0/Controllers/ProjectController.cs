using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Messenger_Thesis_1._0.Models;
using Messenger_Thesis_1._0.Data.Model.Interface;
using Messenger_Thesis_1._0.Data.Model;

namespace Messenger_Thesis_1._0.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository projectRepo;

        public ProjectController(IProjectRepository projectRepo)
        {
            this.projectRepo = projectRepo;
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
    }
}

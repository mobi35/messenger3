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
    public class FeedbackController : Controller
    {
        private readonly IProjectRepository projectRepo;
        private readonly IFeedbackRepository feedRepo;

        public FeedbackController(IProjectRepository projectRepo, IFeedbackRepository feedRepo)
        {
            this.projectRepo = projectRepo;
            this.feedRepo = feedRepo;
        }

        public List<Feedback> GetFeedbackList()
        {

            return feedRepo.GetAll().ToList();
        }

        public IActionResult Index()
        {
            return View(GetFeedbackList());
        }

        [HttpPost]
        public string MemberFeedback(Feedback feeds)
        {
            feeds.Name = HttpContext.Session.GetString("Email");
            feeds.DateFeedbacked = DateTime.Now;
            feeds.Overall = (feeds.Promptness + feeds.Behaviour + feeds.Responsiveness + feeds.Quality) / 4;
            feedRepo.Create(feeds);

            return "done";
        }
        

    
        [HttpPost]
        public string SendMessage(string subject, string name, string message, string email )
        {
            var projectID = int.Parse(subject);

            var checkProject = projectRepo.FindProject(a => a.ProjectID == projectID);

            if(checkProject == null)
            {
                return "error";
            }

            Feedback feedback = new Feedback();
            feedback.Subject = subject;
            feedback.Name = name;
            feedback.Message = message;

            feedRepo.Create(feedback);
         
            return "";
        }

        public IActionResult Services()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Client()
        {
            var email = HttpContext.Session.GetString("Email").ToString();
            return View(feedRepo.GetAll().Where(a => a.Name == email).OrderByDescending(a => a.FeedbackID).ToList());
        }

        public IActionResult Admin()
        {
            return View(feedRepo.GetAll().OrderByDescending(a => a.FeedbackID).ToList());
        }

        public string AdminResponse(int id, string message)
        {
           var feedback =  feedRepo.FindFeedback(a => a.FeedbackID == id);
           feedback.Respond = message;
            feedRepo.Update(feedback);
            return "";
        }
    }
}

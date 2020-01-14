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
    public class FeedbackController : Controller
    {
        private readonly IFeedbackRepository feedRepo;

        public FeedbackController(IFeedbackRepository feedRepo)
        {
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
        public IActionResult SendMessage(string subject, string name, string message, string email )
        {
            Feedback feedback = new Feedback();
            feedback.Subject = subject;
            feedback.Name = name;
            feedback.Message = message;

            feedRepo.Create(feedback);
         
            return View();
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
    }
}

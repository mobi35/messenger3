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
using System.Net;

namespace Messenger_Thesis_1._0.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository projectRepo;
        private readonly IUserRepository userRepo;
        private readonly ILetterRepository letterRepo;

        public ProjectController(IProjectRepository projectRepo, IUserRepository userRepo, ILetterRepository letterRepo )
        {
            this.projectRepo = projectRepo;
            this.userRepo = userRepo;
            this.letterRepo = letterRepo;
        }

        public List<Project> GetProjects(DateTime? startDate, DateTime? endDate)
        {

            if(startDate != null & endDate != null)
            {
                return projectRepo.GetAll().Where(a => a.Archived == false && a.CurrentDateStart >= startDate && a.CurrentDateStart <= endDate).ToList();
            }
            else
            {
                return projectRepo.GetAll().Where(a => a.Archived == false).ToList();
            }
           
        }
        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {

            return View(GetProjects(startDate, endDate));
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
        public JsonResult GetLetterProject(int id)
        {

            if (HttpContext.Session.GetString("Role").ToString() == "Messenger")
            {
                var userID = int.Parse( HttpContext.Session.GetString("UserID").ToString() );
            var letterList = letterRepo.GetAll().Where(a => a.ProjectID == id && a.ReceiverName != "Name" && a.MessengerID == userID).ToList();
            return Json(letterList);
            } else { 
            var letterList = letterRepo.GetAll().Where(a => a.ProjectID == id && a.ReceiverName != "Name").ToList();
            return Json(letterList);
            }

        }

        [HttpGet]
        public JsonResult GetLetterDelivery(int id)
        {
            if (HttpContext.Session.GetString("Role").ToString() == "Messenger")
            {
                var userID = int.Parse(HttpContext.Session.GetString("UserID").ToString());
                var letterList = letterRepo.GetAll().Where(a => a.DeliveryID == id && a.ReceiverName != "Name" && a.MessengerID == userID).ToList();
                return Json(letterList);
            }
            else
            {
                var letterList = letterRepo.GetAll().Where(a => a.DeliveryID == id && a.ReceiverName != "Name").ToList();
                return Json(letterList);
            }


        }

        public IActionResult Delete(int id)
        {
            projectRepo.Delete(projectRepo.FindProject(a => a.ProjectID == id));

            var userID = int.Parse(HttpContext.Session.GetString("UserID").ToString());
            var getCompanyName = userRepo.FindUser(a => a.UserID == userID);
            var project = projectRepo.GetAll().Where(a => a.ProjectName == getCompanyName.CompanyName).ToList();
            return View("Client", project);
        }
      
        public string PickUp(Project project)
        {

            var projectModel = projectRepo.FindProject(a => a.ProjectID == project.ProjectID);
            projectModel.Status = "On-going";
            projectModel.Messenger = project.Messenger;
            projectModel.Area = project.Area;
            projectModel.CurrentDateStart = DateTime.Now.AddDays(2);
            projectRepo.Update(projectModel);

            return "";

        }



        public string Delivery(Project project)
        {

            var projectModel = projectRepo.FindProject(a => a.ProjectID == project.ProjectID);
            projectModel.Status = "On-going";

            var getArea = letterRepo.GetAll().GroupBy(a => a.Area).Select(a => new { 
            Key = a.Key,
            Area = a.FirstOrDefault().Area
            }).ToList();

            var getLetter = letterRepo.GetAll().Where(a => a.DeliveryID == project.ProjectID).ToList();

            //GET ALL MESSENGER

            var getMessenger = projectRepo.GetAll().Where(a => a.Status == "On-going" && a.Messenger != 0).GroupBy(a => a.Messenger).Select(a => new
            {
                Customer = a.Key,
                Count = a.Count(),
                Messenger =  a.FirstOrDefault().Messenger
            }).OrderByDescending(a => a.Count).ToList();

            List<User> userList = new List<User>();

            foreach (var getM in getMessenger)
            {
                userList.Add(userRepo.FindUser(a => a.UserID == getM.Messenger));
            }
            foreach (var user in userRepo.GetAll().Where(a => a.Role == "Messenger").ToList())
            {
                foreach (var mess in getMessenger)
                {
                    if (user.UserID != mess.Messenger)
                        userList.Add(user);
                }
            }

          


            int count = 0;
            while(getArea.Count() > userList.Count())
            {
                userList.Add(userList[count]);
                count++;
            }

            var messengerCount = 0;
            foreach (var a in getArea)
            {
                foreach(var l in getLetter)
                {
                    if (a.Area == l.Area)
                    {
                        l.MessengerID = userList[messengerCount].UserID;
                        letterRepo.Update(l);
                    }
                }
                messengerCount++;
            }

            projectModel.CurrentDateStart = DateTime.Now.AddDays(2);

            var messengerStr = "";

            foreach (var l in getLetter.GroupBy(a => a.MessengerID).Select(a => a.FirstOrDefault().MessengerID).ToList())
            {
                messengerStr += l + ",";
            }

            projectModel.ListOfMessenger = messengerStr;

            projectModel.Area = project.Area;
            projectRepo.Update(projectModel);

            return "";
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
            project.Email = HttpContext.Session.GetString("Email").ToString();
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
           var userID = int.Parse( HttpContext.Session.GetString("UserID").ToString() );
            var getCompanyName = userRepo.FindUser(a => a.UserID == userID);
            var project = projectRepo.GetAll().Where(a => a.ProjectName == getCompanyName.CompanyName && a.UserArchive == false).ToList();
            return View(project);
        }

        public IActionResult Admin()
        {
            var user = userRepo.GetAll().Where(a => a.Role == "Messenger" ).ToList();
            return View(user);
        }



        public IActionResult ClientArchivePage()
        {
            var userID = int.Parse(HttpContext.Session.GetString("UserID").ToString());
            var getCompanyName = userRepo.FindUser(a => a.UserID == userID);
            var project = projectRepo.GetAll().Where(a => a.ProjectName == getCompanyName.CompanyName && a.UserArchive == true).ToList();
            return View(project);
        }

        public IActionResult AdminArchivePage()
        {
             return View(projectRepo.GetAll().Where(a => a.Archived == true).ToList()); 
        }

        public IActionResult AdminArchived(int id)
        {
            var project = projectRepo.FindProject(a => a.ProjectID == id);
            if (projectRepo.FindProject(a => a.ProjectID == id).Status == "Completed")
            {
                project.Archived = true;
                projectRepo.Update(project);

                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = "<html><script>alert('Project Archived Success.'); window.open('../../../../Project/Index','_self')</script></html>"
                };
            }

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Project Archived Fail.'); window.open('../../../../Project/Index','_self')</script></html>"
            };
        }

        public IActionResult AdminunArchived(int id)
        {
            var project = projectRepo.FindProject(a => a.ProjectID == id);
            if (projectRepo.FindProject(a => a.ProjectID == id).Status == "Completed")
            {
                project.Archived = false;
                projectRepo.Update(project);

                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = "<html><script>alert('Project Archived Success.'); window.open('../../../../Project/Index','_self')</script></html>"
                };
            }

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Project Archived Fail.'); window.open('../../../../Project/Index','_self')</script></html>"
            };
        }

     

        public IActionResult ClientArchived(int id)
        {
           var project = projectRepo.FindProject(a => a.ProjectID == id);
            if (projectRepo.FindProject(a => a.ProjectID == id).Status == "Completed")
            {
                project.UserArchive = true;
                projectRepo.Update(project);

                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = "<html><script>alert('Project Archived Success.'); window.open('../../../../Project/Client','_self')</script></html>"
                };
            }

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Project Archived Fail.'); window.open('../../../../Project/Client','_self')</script></html>"
            };
        }

        public IActionResult ClientUnArchived(int id)
        {
            var project = projectRepo.FindProject(a => a.ProjectID == id);
            if (projectRepo.FindProject(a => a.ProjectID == id).Status == "Completed")
            {
                project.UserArchive = false;
                projectRepo.Update(project);

                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = "<html><script>alert('Project Archived Success.'); window.open('../../../../Project/Client','_self')</script></html>"
                };
            }

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Project Archived Fail.'); window.open('../../../../Project/Client','_self')</script></html>"
            };
        }




        public IActionResult Messenger()
        {

            var userID = int.Parse(HttpContext.Session.GetString("UserID").ToString());
            List<Project> proj = new List<Project>();
            foreach (var l in projectRepo.GetAll().ToList())
            {
                if(l.ListOfMessenger != null) { 
                foreach (var s in l.ListOfMessenger.Split(","))
                {
                    if (s == userID.ToString())
                    {
                        proj.Add(l);
                    }
                }
                }else
                {
                    if (l.Messenger == userID)
                    {
                        proj.Add(l);
                    }
                }
            }
            return View(proj);
          
        }

        public string FinishedPickup(int id)
        {
            var project = projectRepo.FindProject(a => a.ProjectID == id);
            project.Status = "Completed";
            projectRepo.Update(project);


            var code = Guid.NewGuid().ToString("N");

            Project proj = new Project();
            proj.ContractID = project.ContractID;
            proj.ClientName = project.ClientName;
            proj.Email = project.Email;
            proj.Status = "Pending";
            proj.TypeOfTask = "Delivery";
            proj.InvoiceDate = DateTime.Now;
            proj.Quantity = project.Quantity;
            proj.Price = project.Price;
            proj.ProjectName = project.ProjectName;
            proj.ProjectCode = code;

            projectRepo.Create(proj);

            var projID = projectRepo.GetAll().OrderBy(a => a.ProjectID).LastOrDefault().ProjectID;

            foreach (var m in letterRepo.GetAll().Where(a => a.ProjectID == project.ProjectID).ToList())
            {
                m.DeliveryID = projID;
                letterRepo.Update(m);
            }


            return "";
        }

        public string FinishedDelivery(int id)
        {
            var project = projectRepo.FindProject(a => a.ProjectID == id);
           
            var userID = int.Parse(HttpContext.Session.GetString("UserID").ToString());
            foreach (var letter in letterRepo.GetAll().Where(a => a.DeliveryID == id && a.MessengerID == userID).ToList())
            {
                letter.DateOfDelivery = DateTime.Now;
                letter.Status = "Delivered";
                letterRepo.Update(letter);
            }

            var count = letterRepo.GetAll().Where(a => a.DeliveryID == id).ToList().Count();
            var tryCount = 0;
            foreach (var letter in letterRepo.GetAll().Where(a => a.DeliveryID == id ).ToList())
            {
                if(letter.Status == null )
                {
                    break;
                }else
                {
                    tryCount++;
                }
            }

            if(count == tryCount)
            {
                project.Status = "Completed";
                projectRepo.Update(project);
            }

           
            return "";
        }

        public JsonResult DateFilter(DateTime startDate, DateTime endDate)
        {
            return Json( projectRepo.GetAll().Where(a => a.Archived == false && a.CurrentDateStart >= startDate && a.CurrentDateStart <= endDate).ToList());
        }

    }
}

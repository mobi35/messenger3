using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Messenger_Thesis_1._0.Models;
using Messenger_Thesis_1._0.Models.Dashboard;
using Messenger_Thesis_1._0.Data.Model.Interface;

namespace Messenger_Thesis_1._0.Controllers
{
    public class AdminController : Controller
    {
        private readonly IFeedbackRepository feedRepo;
        private readonly IProjectRepository projectRepo;
        private readonly IUserRepository userRepo;
        private readonly ILetterRepository letterRepo;

        public AdminController(IFeedbackRepository feedRepo, IProjectRepository projectRepo, IUserRepository userRepo, ILetterRepository letterRepo)
        {
            this.feedRepo = feedRepo;
            this.projectRepo = projectRepo;
            this.userRepo = userRepo;
            this.letterRepo = letterRepo;
        }

        public IActionResult Index()
        {

            DashboardViewModel dashVM = new DashboardViewModel();

            Sale sale = new Sale();


           //THIS IS FOR SALE CHART
            List<float> sales = new List<float>();

            float y1 = 0, y2 = 0, y3 = 0, y4 = 0, y5 = 0, y6 = 0, y7 = 0, y8 = 0, y9 = 0, y10 = 0, y11 = 0, y12 = 0;
            foreach (var s in letterRepo.GetAll().ToList() )
            {

                if (s.PaymentDate.Month == 1)
                    y1 += s.Price;
                if (s.PaymentDate.Month == 2)
                    y2 += s.Price;
                if (s.PaymentDate.Month == 3)
                    y3 += s.Price;
                if (s.PaymentDate.Month == 4)
                    y4 += s.Price;
                if (s.PaymentDate.Month == 5)
                    y5 += s.Price;
                if (s.PaymentDate.Month == 6)
                    y6 += s.Price;
                if (s.PaymentDate.Month == 7)
                    y7 += s.Price;
                if (s.PaymentDate.Month == 8)
                    y8 += s.Price;
                if (s.PaymentDate.Month == 9)
                    y9 += s.Price;
                if (s.PaymentDate.Month == 10)
                    y10 += s.Price;
                if (s.PaymentDate.Month == 11)
                    y11 += s.Price;
                if (s.PaymentDate.Month == 12)
                    y12 += s.Price;

            }
            sales.Add(y1);
            sales.Add(y2);
            sales.Add(y3);
            sales.Add(y4);
            sales.Add(y5);
            sales.Add(y6);
            sales.Add(y7);
            sales.Add(y8);
            sales.Add(y9);
            sales.Add(y10);
            sales.Add(y11);
            sales.Add(y12);
            dashVM.Sales = sales;

            // THIS IS FOR SALE



            //THIS IS FOR ORDER CHART





            List<int> order = new List<int>();
            List<int> dateAvailable = new List<int>();
            List<string> orderLabel = new List<string>();
            var dateNow = DateTime.Now.Month;
        
            dateAvailable.Add(dateNow - 2);
            dateAvailable.Add(dateNow - 1);
            dateAvailable.Add(dateNow);
            dateAvailable.Add(dateNow + 1);
            dateAvailable.Add(dateNow + 2);
            dateAvailable.Add(dateNow + 3);
            int o1 = 0, o2 = 0, o3 = 0, o4 = 0, o5 = 0, o6 = 0;

            foreach (var o in projectRepo.GetAll().ToList())
            {
                if (o.InvoiceDate.Month == dateAvailable.ElementAtOrDefault(0))
                    o1++;
                if (o.InvoiceDate.Month == dateAvailable.ElementAtOrDefault(1))
                    o2++;
                if (o.InvoiceDate.Month == dateAvailable.ElementAtOrDefault(2))
                    o3++;
                if (o.InvoiceDate.Month == dateAvailable.ElementAtOrDefault(3))
                    o4++;
                if (o.InvoiceDate.Month == dateAvailable.ElementAtOrDefault(4))
                    o5++;
                if (o.InvoiceDate.Month == dateAvailable.ElementAtOrDefault(5))
                    o6++;

            }
            order.Add(o1);
            order.Add(o2);
            order.Add(o3);
            order.Add(o4);
            order.Add(o5);
            order.Add(o6);

            foreach (var l in dateAvailable)
            {
                if (l == 1)
                    orderLabel.Add("Jan");
                if (l == 2)
                    orderLabel.Add("Feb");
                if (l == 3)
                    orderLabel.Add("Mar");
                if (l == 4)
                    orderLabel.Add("Apr");
                if (l == 5)
                    orderLabel.Add("May");
                if (l == 6)
                    orderLabel.Add("Jun");
                if (l == 7)
                    orderLabel.Add("Jul");
                if (l == 8)
                    orderLabel.Add("Aug");
                if (l == 9)
                    orderLabel.Add("Sep");
                if (l == 10)
                    orderLabel.Add("Oct");
                if (l == 11)
                    orderLabel.Add("Nov");
                if (l == 12)
                    orderLabel.Add("Dec");

            }

            dashVM.OrderLabel = orderLabel;
            dashVM.Order = order;

            // USER


            dashVM.Users = userRepo.GetAll().Where(a => a.Role == "Client").OrderByDescending(a => a.UserID).Take(5).ToList();

            // PROJECT

            dashVM.Projects = projectRepo.GetAll().OrderByDescending(a => a.ProjectID).Take(5).ToList();


            dashVM.Feedbacks = feedRepo.GetAll().OrderByDescending(a => a.FeedbackID).Take(5).ToList();

            return View(dashVM);
        }
        

    }
}

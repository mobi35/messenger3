using Messenger_Thesis_1._0.Data.Model;
using Messenger_Thesis_1._0.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Models
{
    public class DashboardViewModel
    {
        public List<float> Sales { get; set; }

        public List<int> Order { get; set; }

        public List<string> OrderLabel { get; set; }

        public List<Project> Projects { get; set; }

        public List<Feedback> Feedbacks { get; set; }
        public List<User> Users { get; set; }

        public RatingModel RatingModel { get; set; }
    }
}

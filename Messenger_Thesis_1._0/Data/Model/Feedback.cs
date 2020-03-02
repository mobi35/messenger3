using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Model
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime DateFeedbacked { get; set; }

        public string Respond { get; set; }

        public float Quality { get; set; }

        public float Promptness { get; set; }

        public int ProjectID { get; set; }

        public float Behaviour { get; set; }

        public float Responsiveness { get; set; }

        public float Overall { get; set; }
    }
}

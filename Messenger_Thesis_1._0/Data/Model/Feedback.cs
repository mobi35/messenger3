using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Model
{
    public class Feedback
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
        public DateTime DateFeedbacked { get; set; }
    }
}

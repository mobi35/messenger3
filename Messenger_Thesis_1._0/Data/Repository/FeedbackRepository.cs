using Messenger_Thesis_1._0.Data.Model;
using Messenger_Thesis_1._0.Data.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Repository
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        private readonly MessengerDBContext _context;
        public FeedbackRepository(MessengerDBContext context) : base(context)
        {
            _context =  context;
        }

        public Feedback FindFeedback(Func<Feedback, bool> predicate)
        {
            return _context.Feedbacks
                   .FirstOrDefault(predicate);
        }
    }
}

using Messenger_Thesis_1._0.Data.Model;
using Messenger_Thesis_1._0.Data.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Repository
{
    public class LetterRepository : Repository<Letter>, ILetterRepository
    {
        private readonly MessengerDBContext _context;
        public LetterRepository(MessengerDBContext context) : base(context)
        {
            _context =  context;
        }

        public Letter FindLetter(Func<Letter, bool> predicate)
        {
            return _context.Letters
                   .FirstOrDefault(predicate);
        }
    }
}

using Messenger_Thesis_1._0.Data.Model;
using Messenger_Thesis_1._0.Data.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly MessengerDBContext _context;
        public UserRepository(MessengerDBContext context) : base(context)
        {
            _context =  context;
        }

        public User FindUser(Func<User, bool> predicate)
        {
            return _context.Users
                   .FirstOrDefault(predicate);
        }
    }
}

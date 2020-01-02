using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Model.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        User FindUser(Func<User, bool> predicate);
    }
}

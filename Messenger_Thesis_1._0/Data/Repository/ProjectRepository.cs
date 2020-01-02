using Messenger_Thesis_1._0.Data.Model;
using Messenger_Thesis_1._0.Data.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Repository
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly MessengerDBContext _context;
        public ProjectRepository(MessengerDBContext context) : base(context)
        {
            _context =  context;
        }

        public Project FindProject(Func<Project, bool> predicate)
        {
            return _context.Projects
                   .FirstOrDefault(predicate);
        }
    }
}

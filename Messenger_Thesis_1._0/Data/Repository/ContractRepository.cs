using Messenger_Thesis_1._0.Data.Model;
using Messenger_Thesis_1._0.Data.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Repository
{
    public class ContractRepository : Repository<Contract>, IContractRepository
    {
        private readonly MessengerDBContext _context;
        public ContractRepository(MessengerDBContext context) : base(context)
        {
            _context =  context;
        }

        public Contract FindContract(Func<Contract, bool> predicate)
        {
            return _context.Contracts
                   .FirstOrDefault(predicate);
        }
    }
}

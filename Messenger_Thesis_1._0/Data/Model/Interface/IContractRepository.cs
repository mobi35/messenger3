using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Model.Interface
{
    public interface IContractRepository : IRepository<Contract>
    {
        Contract FindContract(Func<Contract, bool> predicate);
    }
}

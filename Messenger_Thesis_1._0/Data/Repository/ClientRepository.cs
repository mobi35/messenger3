﻿using Messenger_Thesis_1._0.Data.Model;
using Messenger_Thesis_1._0.Data.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data.Repository
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly MessengerDBContext _context;
        public ClientRepository(MessengerDBContext context) : base(context)
        {
            _context =  context;
        }

        public Client FindClient(Func<Client, bool> predicate)
        {
            return _context.Clients
                   .FirstOrDefault(predicate);
        }
    }
}

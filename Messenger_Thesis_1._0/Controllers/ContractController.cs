using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messenger_Thesis_1._0.Data.Model;
using Messenger_Thesis_1._0.Data.Model.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Messenger_Thesis_1._0.Controllers
{
    public class ContractController : Controller
    {
        private readonly IUserRepository userRepo;
        private readonly IContractRepository contractRepo;

        public ContractController(IUserRepository userRepo, IContractRepository contractRepo)
        {
            this.userRepo = userRepo;
            this.contractRepo = contractRepo;
        }
        public IActionResult Index()
        {
            return View(contractRepo.GetAll().ToList());
        }

        public string Add(Contract contract)
        {
            contract.EndDuration = contract.StartDuration.AddYears(contract.YearsOfDuration);
            contractRepo.Create(contract);
            return "";
        }

        public IActionResult Client()
        {
            var userID = int.Parse( HttpContext.Session.GetString("UserID").ToString() );
             var userModel =   userRepo.FindUser(a => a.UserID == userID);
            return View(contractRepo.GetAll().Where(a => a.CompanyName == userModel.CompanyName).ToList()); ;
        }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            return View(contractRepo.GetAll().Where(a => a.Archived == false).ToList());
        }

        public string Add(Contract contract)
        {
            
            contractRepo.Create(contract);
            return "";
        }

        public IActionResult Client()
        {
            var userID = int.Parse( HttpContext.Session.GetString("UserID").ToString() );
             var userModel =   userRepo.FindUser(a => a.UserID == userID);
            return View(contractRepo.GetAll().Where(a => a.CompanyName == userModel.CompanyName && a.UserArchive == false).ToList()); 
        }
        public IActionResult AdminArchivePage()
        {
            return View(contractRepo.GetAll().Where(a => a.Archived == true).ToList());
        }
        public IActionResult ClientArchivedPage()
        {
            var userID = int.Parse(HttpContext.Session.GetString("UserID").ToString());
            var userModel = userRepo.FindUser(a => a.UserID == userID);
            return View(contractRepo.GetAll().Where(a => a.CompanyName == userModel.CompanyName && a.UserArchive == true).ToList()); ;

        }

        public IActionResult AdminArchived(int id) {
            var contr = contractRepo.FindContract(a => a.ContractID ==  id);
            if (contractRepo.FindContract(a => a.ContractID == id).EndDuration <= DateTime.Now )
            {

                contr.Archived = true;
                contractRepo.Update(contr);

                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = "<html><script>alert('Contract Archived Success.'); window.open('../../../../Contract/Index','_self')</script></html>"
                };
            }

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Contract Archived Fail.'); window.open('../../../../Contract/Index','_self')</script></html>"
            };


        }



        public IActionResult ClientArchived(int id)
        {
            var contr = contractRepo.FindContract(a => a.ContractID == id);
            if (contractRepo.FindContract(a => a.ContractID == id).EndDuration <= DateTime.Now)
            {
                contr.UserArchive = true;
                contractRepo.Update(contr);
                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = "<html><script>alert('Contract Archived Success.'); window.open('../../../../Contract/Client','_self')</script></html>"
                };
            }

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Contract Archived Fail.'); window.open('../../../../Contract/Client','_self')</script></html>"
            };


        }




        public IActionResult AdminUnArchived(int id)
        {
            var contr = contractRepo.FindContract(a => a.ContractID == id);
            if (contractRepo.FindContract(a => a.ContractID == id).EndDuration <= DateTime.Now)
            {

                contr.Archived = false;
                contractRepo.Update(contr);

                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = "<html><script>alert('Contract Archived Success.'); window.open('../../../../Contract/Index','_self')</script></html>"
                };
            }

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Contract Archived Fail.'); window.open('../../../../Contract/Index','_self')</script></html>"
            };


        }



        public IActionResult ClientUnArchived(int id)
        {
            var contr = contractRepo.FindContract(a => a.ContractID == id);
            if (contractRepo.FindContract(a => a.ContractID == id).EndDuration <= DateTime.Now)
            {
                contr.UserArchive = false;
                contractRepo.Update(contr);
                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = "<html><script>alert('Contract Archived Success.'); window.open('../../../../Contract/Client','_self')</script></html>"
                };
            }

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Contract Archived Fail.'); window.open('../../../../Contract/Client','_self')</script></html>"
            };


        }







    }
}
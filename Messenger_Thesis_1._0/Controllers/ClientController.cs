using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Messenger_Thesis_1._0.Models;
using Messenger_Thesis_1._0.Data.Model.Interface;
using Messenger_Thesis_1._0.Data.Model;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using OfficeOpenXml;
using System.Net;

namespace Messenger_Thesis_1._0.Controllers
{
    public class ClientController : Controller
    {
        private readonly IContractRepository contractRepo;
        private readonly ILetterRepository _letterRepo;
        private readonly IUserRepository _userRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IProjectRepository _projectRepo;

        public ClientController(IContractRepository contractRepo, ILetterRepository letterRepo, IUserRepository userRepo, IHostingEnvironment hostingEnvironment, IProjectRepository projectRepo)
        {
            this.contractRepo = contractRepo;
            _letterRepo = letterRepo;
            _userRepo = userRepo;
            _hostingEnvironment = hostingEnvironment;
            _projectRepo = projectRepo;
        }

        public IActionResult Index()
        {
            var users = _userRepo.GetAll().Where(a => a.Role == "Client" && a.Archived == false).OrderByDescending(a => a.UserID).ToList();
            return View(users);
        }

        public IActionResult ArchivedClientAccounts()
        {
            return View(_userRepo.GetAll().Where(a => a.Archived == true && a.Role == "Client").ToList());
        }

        public IActionResult ArchiveAccountExecute(int id)
        {

            var user = _userRepo.FindUser(a => a.UserID == id);
            user.Archived = true;
            _userRepo.Update(user);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Client Archived Success.'); window.open('../../../../Client/Index','_self')</script></html>"
            };
        }

        public IActionResult UnArchiveAccountExecute(int id)
        {
            var user = _userRepo.FindUser(a => a.UserID == id);
            user.Archived = false;
            _userRepo.Update(user);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Client UnArchived Success.'); window.open('../../../../Client/Index','_self')</script></html>"
            };
        }


        [HttpPost]
        public async Task<IActionResult> NewProject(Project project)
        {
            var email = HttpContext.Session.GetString("Email");
            var client = _userRepo.FindUser(a => a.Email == email);
            var code = Guid.NewGuid().ToString("N");

            Project proj = new Project();
            proj.ContractID = project.ContractID;
            proj.ClientName = HttpContext.Session.GetString("FullName");

            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, client.PickupDay);
            if (dt <= DateTime.Now.Date)
            {
                proj.CurrentDateStart = dt;
            }else
            {
               proj.CurrentDateStart =  dt.AddDays(15);
            }

            proj.Email = client.Email;
            proj.Status = "Pending";
            proj.TypeOfTask = "Pick-up";

            proj.InvoiceDate = DateTime.Now;

            proj.ProjectName = client.CompanyName;
            proj.ProjectCode = code;

            _projectRepo.Create(proj);


            if (project.DepositImage == null || project.DepositImage.Length == 0)
                return Content("File Not Selected");

            string fileExtension = Path.GetExtension(project.DepositImage.FileName);

            if (fileExtension == ".xls" || fileExtension == ".xlsx")
            {

                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "");
                var fileName = project.DepositImage.FileName;
                var filePath = Path.Combine(uploadsFolder, fileName);
                var fileLocation = new FileInfo(filePath);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await project.DepositImage.CopyToAsync(fileStream);
                }
                var DataList = new List<ExcelFormat>();
              
                using (ExcelPackage package = new ExcelPackage(fileLocation))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
                    //var workSheet = package.Workbook.Worksheets.First();
                    int totalRows = workSheet.Dimension.Rows;

                    for (int i = 1; i <= totalRows; i++)
                    {
                        if (i == 1)
                            continue;
                        DataList.Add(new ExcelFormat
                        {
                            Name = workSheet.Cells[i, 1].Value.ToString(),
                            Address = workSheet.Cells[i, 2].Value.ToString(),
                            Area = workSheet.Cells[i, 3].Value.ToString()
                        });
                    }
                }
                var contractModel = contractRepo.FindContract(a => a.ContractID == project.ContractID);
                int count = 0;
                foreach (var data in DataList)
                {

                    count++;
                    Letter letter = new Letter();
                    letter.ProjectID = _projectRepo.GetAll().OrderBy(a => a.ProjectID).LastOrDefault().ProjectID;
                    letter.ReceiverName = data.Name;
                    letter.LocationOfDelivery = data.Address + " - " + data.Area;
                    letter.Area = data.Area;
                    letter.Price =contractModel.PricePerQuantity / contractModel.Quantity ;
                    _letterRepo.Create(letter);


                }
               
             
                var defQuantity =  contractModel.Quantity;
                var defPrice = contractModel.PricePerQuantity;
                float price = 0;
                if (count <= defQuantity)
                {
                    price = defPrice;
                }else
                {
                    var roundedValue = count / defQuantity;
                    price += roundedValue * defPrice;
                }

                var projID = _projectRepo.GetAll().OrderBy(a => a.ProjectID).LastOrDefault().ProjectID;
                var projectModel = _projectRepo.FindProject(a => a.ProjectID == projID);
                projectModel.Price = price;
                projectModel.Quantity = count;


                _projectRepo.Update(projectModel);


                return UploadSuccess();
            }
            return UploadSuccess();
        }



            [HttpPost]
        public async Task<IActionResult> ReadExcelFileAsync(int projectid, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("File Not Selected");

            string fileExtension = Path.GetExtension(file.FileName);

            if (fileExtension == ".xls" || fileExtension == ".xlsx")
            {

                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "");
                var fileName = file.FileName;
                var filePath = Path.Combine(uploadsFolder, fileName);
                var fileLocation = new FileInfo(filePath);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                var DataList = new List<ExcelFormat>();
                var code = Guid.NewGuid().ToString("N");
                using (ExcelPackage package = new ExcelPackage(fileLocation))
                {   
                    ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
                    //var workSheet = package.Workbook.Worksheets.First();
                    int totalRows = workSheet.Dimension.Rows;

                    for (int i = 1; i <= totalRows; i++)
                    {
                        DataList.Add(new ExcelFormat
                        {
                            Name = workSheet.Cells[i, 1].Value.ToString(),
                            Address = workSheet.Cells[i, 2].Value.ToString(),
                            Area = workSheet.Cells[i, 3].Value.ToString()
                        });
                    }

             

                }

                int count = 0;
                foreach (var data in DataList)
                {

                    count++;
                    Letter letter = new Letter();
                    letter.ProjectID = projectid;
                    letter.ReceiverName = data.Name;
                    letter.LocationOfDelivery = data.Address + " - " + data.Area;
                    letter.Price = 5;
                    
                    _letterRepo.Create(letter);
                    

                }

            }

            return UploadSuccess();
        }
        public IActionResult ActivateAccount(int id)
        {


            var user = _userRepo.FindUser(a => a.UserID == id);
            user.AccountStatus = "Activated";
            _userRepo.Update(user);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Account Activated'); window.open('../../../../User/Index','_self')</script></html>"
            };
        }
        public IActionResult DisableAccount(int id)
        {

            var user = _userRepo.FindUser(a => a.UserID == id);
            user.AccountStatus = "Disabled";
            _userRepo.Update(user);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Account Disabled'); window.open('../../../../User/Index','_self')</script></html>"
            };
        }

        [HttpGet]
        public ContentResult UploadSuccess()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><script>alert('Successfully uploaded'); window.open('../../../../Project/Client','_self')</script></html>"
            };
        }

        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [HttpGet]
        public JsonResult GetUserList(int page = 1, string query = "")
        {


            int count = page * 10;

            var users = _userRepo.GetAll().Where(a => a.Role == "Client").OrderByDescending(a => a.UserID).ToList();


            List<User> sortedUser = new List<User>();
            for (int i = 0; i < users.Count(); i++)
            {
                if (page == 1 && i < 10)
                {
                    sortedUser.Add(users[i]);
                }
                else if (i > (count - 10) && count >= i)
                {
                    sortedUser.Add(users[i]);
                }
            }

            return Json(sortedUser);
        }   

    

        [HttpPost]

        public string SendRegisteredData(User user)
        {
            List<string> errors = new List<string>();
            if (user.Email == null)
                errors.Add("no_email");
            else if (!IsValidEmail(user.Email))
                errors.Add("invalid_email");
            else if (_userRepo.FindUser(a => a.Email == user.Email) != null)
                errors.Add("existing_email");

            if (user.Password == null || user.ConfirmPassword == null)
                errors.Add("no_password");
            else if (user.ConfirmPassword != user.Password)
                errors.Add("password_not_match");


            if (user.CompanyName == null)
             errors.Add("companyname_required");
           

            if (user.FirstName == null)
                errors.Add("firstname_required");
            else if (user.FirstName.Any(char.IsDigit))
                errors.Add("firstname_not_letter");
            else if (user.FirstName.Length > 50)
                errors.Add("firstname_max_letter");

            if (user.LastName == null)
                errors.Add("lastname_required");
            else if (user.LastName.Any(char.IsDigit))
                errors.Add("lastname_not_letter");
            else if (user.LastName.Length > 50)
                errors.Add("lastname_max_letter");

            if (user.Phonenumber == null)
                errors.Add("phone_required");
            else if (!user.Phonenumber.Any(char.IsDigit))
                errors.Add("number_only");

            if (user.Address == null)
                errors.Add("address_required");

       
            if (user.Image == null)
                errors.Add("no_picture");


            if (errors.Count != 0)
            {
                string errorList = string.Join(",", errors);
                return errorList;
            }


            if (user.Image != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                var uniqueName = Guid.NewGuid().ToString() + "_" + user.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueName);
                user.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                user.ImageName = uniqueName;
                HttpContext.Session.SetString("Image", user.ImageName);
            }
            Utilities util = new Utilities();
            user.Password = util.base64Encode(user.Password);
            if (_userRepo.FindUser(a => a.Email == user.Email) == null)
            {
                _userRepo.Create(user);
            }

            try
            {

                EmailClass.GmailUsername = "payoneeers2093@gmail.com";
                EmailClass.GmailPassword = "Asakaboi35";
                EmailClass mailer = new EmailClass();

                mailer.ToEmail = user.Email;
                mailer.Subject = "You are now part of Mail Expert Messengerial";
                mailer.Body += "Hi " + user.FirstName + " " + user.LastName;
                mailer.Body += "<br> Here's your username : " + user.Email;
                mailer.Body += "<br> Here's your password : " + user.Password;

                mailer.Body += "<br> Click here to activate your account. https://localhost:44379/User/EmailActivate/" + user.UserID;
                mailer.IsHtml = true;
                mailer.Send();
            }
            catch (Exception e)
            {


            }


            return "";
        }

        public User GetUserDetails(int id)
        {

            var user = _userRepo.FindUser(a => a.UserID == id);

            return user;
        }

        [HttpPost]
        public string UpdateData(User userModel)
        {
            var user = new User();

            List<string> errors = new List<string>();

            if (userModel.Email == null)
            {
                user = _userRepo.FindUser(a => a.UserID == userModel.UserID);
            }
            else
            {
                user = _userRepo.FindUser(a => a.Email == userModel.Email);
            }

            if (userModel.ConfirmPassword != userModel.Password)
                errors.Add("password_not_match");


            if (userModel.FirstName == null)
                errors.Add("firstname_required");
            else if (userModel.FirstName.Any(char.IsDigit))
                errors.Add("firstname_not_letter");
            else if (userModel.FirstName.Length > 50)
                errors.Add("firstname_max_letter");

            if (userModel.LastName == null)
                errors.Add("lastname_required");
            else if (userModel.LastName.Any(char.IsDigit))
                errors.Add("lastname_not_letter");
            else if (userModel.LastName.Length > 50)
                errors.Add("lastname_max_letter");



            if (errors.Count != 0)
            {
                string errorList = string.Join(",", errors);
                return errorList;
            }


            if (userModel.Image != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                var uniqueName = Guid.NewGuid().ToString() + "_" + userModel.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueName);
                userModel.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                user.ImageName = uniqueName;
            }

            if (errors.Count == 0 && userModel.Password != null)
            {
                Utilities util = new Utilities();
                user.Password = util.base64Encode(userModel.Password);
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Role = userModel.Role;

                _userRepo.Update(user);

            }
            else if (errors.Count == 0)
            {
                Utilities util = new Utilities();
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Role = userModel.Role;

                _userRepo.Update(user);
            }


            return "";
        }



    }
}

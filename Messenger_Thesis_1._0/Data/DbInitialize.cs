using Messenger_Thesis_1._0.Data.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger_Thesis_1._0.Data
{
    public class DbInitialize
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MessengerDBContext>();

               // var user = new User { FirstName = "Raymond", LastName = "Raymond", MiddleName = "Raymond", UserName = "admin", Password = "YWRtaW5hZG1pbg==", EmailAddress = "Maeee@yahoo.com", Role = "employee", Status = "Activated" };

              //  context.Users.Add(user);

                context.SaveChanges();
            }
        }
    }
}

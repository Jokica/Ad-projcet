using System;
using System.Linq;
using System.Threading.Tasks;
using Announcements.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace Announcements.Web
{
    public class DbInitializer
    {
        public static async Task Initialize(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, Data.ApplicationDbContext db)
        {
            if (!db.AppUsers.Any())
            {
                AdminUser user = new AdminUser
                {
                    UserName = "jovan.joksi@hotmail.com",
                    Email = "jovan.joksi@hotmail.com",
                    EmailConfirmed = true,

                };
                await userManager.CreateAsync(user:user, password:"Jovan123@");
                if (!await roleManager.RoleExistsAsync("admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = "admin"
                    });
                }
                await userManager.AddToRoleAsync(user, "admin");

                db.AdTypes.Add(new AdType
                {
                    Type = Constants.AdType.Jobs
                });
                db.AdTypes.Add(new AdType
                {
                    Type = Constants.AdType.Projects

                });
                db.AdTypes.Add(new AdType
                {
                    Type = Constants.AdType.Announcements
                });
                db.SaveChanges();
            }

        }
    }
}

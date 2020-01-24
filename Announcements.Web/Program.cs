using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Announcements.Web.Data;
using Announcements.Web.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Announcements.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();
            using(var scope = host.Services.CreateScope())
            {
                var sp = scope.ServiceProvider;
                var db = sp.GetRequiredService<ApplicationDbContext>();
                var userManager = sp.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
                DbInitializer.Initialize(userManager, roleManager,db).Wait();
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

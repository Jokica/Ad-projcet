using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Web.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {

        }
        public AppUser(string username):base(username)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Web.Models
{
    public class AdminUser : AppUser
    {
        public AdminUser()
        {

        }
        public AdminUser(string username) : base(username)
        {
        }
    }
}

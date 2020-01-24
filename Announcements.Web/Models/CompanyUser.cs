using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Web.Models
{
    public class CompanyUser:AppUser
    {
        public CompanyUser()
        {

        }
        public Company RepresentsCompany { get; set; }
        public List<Ad> Ads { get; set; }

        public CompanyUser(string userName):base(userName)
        {

        }
    }
}

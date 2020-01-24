using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Web.Models
{
    public class Company
    {
        public Company()
        {
            CompanyFiles = new List<CompanyFile>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public List<CompanyFile> CompanyFiles { get; set; }
    }
}

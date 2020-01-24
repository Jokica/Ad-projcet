using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Web.ViewModel
{
    public class IndexAdViewModel
    {
        [Display(Name = "Company Name ")]
        public string CompanyName { get; set; }
        public DateTime Created { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Display(Name = "Logo")]
        public string ImageUrl { get; set; }
        public string Type { get;  set; }
        public bool IsApproved { get;  set; }
        public string CreatedBy { get;  set; }
    }
}

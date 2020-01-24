using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Web.ViewModel
{
    public class IndexListAdViewModel
    {
        public IEnumerable<IndexAdViewModel> JobAds { get; set; }
        public IEnumerable<IndexAdViewModel> ProjectAds { get; set; }
        public IEnumerable<IndexAdViewModel> AnnouncementAds { get; set; }
    }
}

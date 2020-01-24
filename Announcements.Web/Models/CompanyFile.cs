using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Web.Models
{
    public class CompanyFile
    {
        public long Id { get; set; }
        public byte[] File { get; set; }
        public string Type { get; set; }
        public long CompanyId { get; set; }
        public string FileName { get; set; }
    }
}

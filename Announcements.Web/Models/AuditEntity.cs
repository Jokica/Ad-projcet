using System;

namespace Announcements.Web.Models
{
    public class AuditEntity
    {
        public Guid Identifier { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}

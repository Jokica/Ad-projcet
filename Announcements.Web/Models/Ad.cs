namespace Announcements.Web.Models
{
    public class Ad:AuditEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public AdType AdType { get; set; }
        public CompanyUser User { get; set; }

        public bool IsApproved { get; set; }
    }
}

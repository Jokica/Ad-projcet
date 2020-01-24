using Announcements.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Announcements.Web.Data
{
    public interface IApplicationDbContext
    {
        DbSet<AdminUser> AdminUsers { get; set; }
        DbSet<Ad> Ads { get; set; }
        DbSet<AdType> AdTypes { get; set; }
        DbSet<AppUser> AppUsers { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<CompanyUser> CompanyUsers { get; set; }
    }
}
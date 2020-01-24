using System;
using System.Collections.Generic;
using System.Text;
using Announcements.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Announcements.Web.ViewModel;
using System.Threading;
using System.Threading.Tasks;

namespace Announcements.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService currentUserService;

        public DbSet<Ad> Ads { get; set; }
        public DbSet<AdType> AdTypes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<CompanyFile> CompanyFile { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,ICurrentUserService currentUserService)
         : base(options)
        {
            this.currentUserService = currentUserService;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CompanyUser>().HasMany(x => x.Ads).WithOne(x => x.User).HasForeignKey(x => x.CreatedBy);
            builder.Entity<Company>().HasIndex(x => x.Name).IsUnique();
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            Audit();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            Audit();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void Audit()
        {
            foreach (var entity in ChangeTracker.Entries<AuditEntity>())
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Entity.Created = DateTime.Now;
                    entity.Entity.CreatedBy = currentUserService.GetUserId();
                    entity.Entity.Identifier = Guid.NewGuid();
                }
                else if (entity.State == EntityState.Modified)
                {
                    entity.Entity.Modified = DateTime.Now;
                    entity.Entity.ModifiedBy = currentUserService.GetUserId();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Announcements.Web.Data;
using Announcements.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Announcements.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AdminController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var list = await dbContext.Ads.Include(x => x.AdType).Where(x => !x.IsApproved).ToListAsync();
            var data = list.Select(x => new IndexAdViewModel
            {
                Created = x.Created,
                Title = x.Title,
                Description = x.Description,
                Id = x.Id,
                Type = x.AdType.Type,
                IsApproved = x.IsApproved,
                CreatedBy = x.CreatedBy,
                ImageUrl = "/ads/getCompanyImage?CreatedBy=" + x.CreatedBy,
                CompanyName = dbContext.CompanyUsers.Include(r=>r.RepresentsCompany).First(u=>u.Id == x.CreatedBy).RepresentsCompany.Name
            }).ToList();

            return View(data);
        }
        public async Task<IActionResult> Approve(long Id)
        {
            var ad = await dbContext.Ads.FindAsync(Id);
            ad.IsApproved = true;
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(long Id)
        {
            var ad = await dbContext.Ads.FindAsync(Id);
            dbContext.Ads.Remove(ad);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

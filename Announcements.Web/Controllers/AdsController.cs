using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Announcements.Web.Data;
using Announcements.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Announcements.Web.ViewModel;
using System.Security.Claims;

namespace Announcements.Web.Controllers
{
    [Authorize]
    public class AdsController : Controller
    {
        private readonly ApplicationDbContext _context;
    
        public AdsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        // GET: Ads
        public async Task<IActionResult> Index()
        {

            var  list = await _context.Ads.Include(x=>x.AdType).ToListAsync();
            var data = list.Select(x => new IndexAdViewModel
            {
                Created = x.Created,
                Title = x.Title,
                Description = x.Description,
                Id = x.Id,
                Type = x.AdType.Type,
                IsApproved = x.IsApproved,
                CreatedBy = x.CreatedBy,
                ImageUrl = "/ads/getCompanyImage?CreatedBy=" + x.CreatedBy
            }).ToList();
            IndexListAdViewModel model = new IndexListAdViewModel
            {
                AnnouncementAds = data.Where(x => x.Type == Constants.AdType.Announcements && x.IsApproved),
                JobAds = data.Where(x => x.Type == Constants.AdType.Jobs && x.IsApproved),
                ProjectAds = data.Where(x => x.Type == Constants.AdType.Projects && x.IsApproved),
            };
    
            return base.View(model);
        }
        [AllowAnonymous]

        public IActionResult GetCompanyImage([FromQuery]string CreatedBy)
        {
            var user = _context.CompanyUsers
                                .Include(x=>x.RepresentsCompany)
                                .First(x => x.Id == CreatedBy);
            var file = _context.CompanyFile.First(x => x.CompanyId == user.RepresentsCompany.Id && x.Type == "Logo");

            return File(file.File, "application/octet-stream", file.FileName);
        }
        [AllowAnonymous]
        // GET: Ads/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ad = await _context.Ads.Include(x=>x.AdType).FirstOrDefaultAsync(m => m.Id == id);
            if (ad == null)
            {
                return NotFound();
            }

            return View(new IndexAdViewModel
            {
                Created = ad.Created,
                Title = ad.Title,
                Description = ad.Description,
                Id = ad.Id,
                Type = ad.AdType.Type,
                IsApproved = ad.IsApproved,
                CreatedBy = ad.CreatedBy,
                ImageUrl = "/ads/getCompanyImage?CreatedBy=" + ad.CreatedBy,
                CompanyName = _context.CompanyUsers.Include(r => r.RepresentsCompany).First(u => u.Id == ad.CreatedBy).RepresentsCompany.Name
            });
        }

        // GET: Ads/Create
        public IActionResult Create()
        {
            ViewBag.AdTypeDropDown = new SelectList(Constants.AdType.AdTypes());
            return View();
        }

        // POST: Ads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,AdType")] CreateAdViewModel advw)
        {
            if (ModelState.IsValid)
            {
                var ad = new Ad
                {
                    Description = advw.Description,
                    Title = advw.Title,
                    AdType = _context.AdTypes.FirstOrDefault(x => x.Type == advw.AdType),
                };
                _context.Add(ad);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(advw);
        }

        private string GetUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        // GET: Ads/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var ad = await _context.Ads.FindAsync(id);
            if (ad == null)
            {
                return NotFound();
            }
            return View(new EditAdViewModel
            {
                Title = ad.Title,
                Description = ad.Description
            });
        }

        // POST: Ads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,[Bind("Title,Description")] EditAdViewModel editAdViewModel)
        {
           var ad = await  _context.Ads.FindAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ad.Title = editAdViewModel.Title;
                    ad.Description = editAdViewModel.Description;
                    ad.Modified = DateTime.Now;
                    _context.Update(ad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdExists(ad.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ad);
        }

        // GET: Ads/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ad = await _context.Ads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ad == null)
            {
                return NotFound();
            }

            return View(ad);
        }

        // POST: Ads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var ad = await _context.Ads.FindAsync(id);
            _context.Ads.Remove(ad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdExists(long id)
        {
            return _context.Ads.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Announcements.Web.Data;
using Announcements.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Announcements.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleMenager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleMenager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this.dbContext = dbContext;
            this.roleMenager = roleMenager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Company name")]
            [Required]
            public string CompanyName { get; set; }

            [Required]
            [Display(Name = "Logo ")]
            public IFormFile Logo { get; set; }
            [Required]
            [Display(Name = "Memo ")]
            public IFormFile Memo { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }
        private byte[] GetLogo()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Input.Logo.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var test = Request;
            if (ModelState.IsValid)
            {
                Company company = await dbContext.Companies.FirstOrDefaultAsync(x => x.Name == Input.CompanyName);
                company = company ?? new Company
                {
                    Name = Input.CompanyName
                };
                company.CompanyFiles.Add(new CompanyFile
                {
                    File = GetLogo(),
                    Type = "Logo",
                    FileName = Input.Logo.FileName
                });
                company.CompanyFiles.Add(new CompanyFile
                {
                    File = GetMemo(),
                    Type = "Memo",
                    FileName = Input.Memo.FileName
                });
                var user = new CompanyUser(Input.Email)
                {
                    Email = Input.Email,
                    RepresentsCompany = company
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    var roleExists = await roleMenager.RoleExistsAsync(Constants.User.CompanyRole);
                    if (!roleExists)
                    {
                        await roleMenager.CreateAsync(new IdentityRole(Constants.User.CompanyRole));
                    }
                    await _userManager.AddToRoleAsync(user, Constants.User.CompanyRole);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private byte[] GetMemo()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Input.Memo.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}

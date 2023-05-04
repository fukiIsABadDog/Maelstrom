using EF_Models.Models;
using Maelstrom.Services;
using Maelstrom.ValidationAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Principal;

namespace Maelstrom.Areas.User.Pages.SiteManager
{
    [Authorize]
    public class EditModel : PageModel
    {

        private readonly EF_Models.MaelstromContext _context;
        private readonly IAppUserService _appUserService;
        public EditModel(EF_Models.MaelstromContext context, IAppUserService appUserService)
        {
            _context = context;
            _appUserService = appUserService;
        }

        [BindProperty]
        public Site Site { get; set; } = default!;
        [DisplayName("Upload New Image")]
        [BindProperty]
        [UploadFileExtensions(Extensions = ".jpeg,.jpg")]
        public IFormFile? Upload { get; set; }
        public string? SiteImage { get; private set; }

        public IIdentity CurrentUser { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound("That Resource could not be located.");
            }
            CurrentUser = User.Identity!;
            var site = await _appUserService.GetCurrentUserSite(CurrentUser, id, true);
            ViewData["SiteTypeID"] = new SelectList(_context.SiteTypes, "SiteTypeID", "Name");
            // maybe think  access denied logic and also think about custom 404 page
            if (site == null)
            {
                return Forbid();
            }
            else
            {
                Site = site;
                if (Site.ImageData != null && Site.ImageData.Length > 1 == true)
                {
                    var base64 = Convert.ToBase64String(Site.ImageData);
                    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    SiteImage = imgSrc;
                }

            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Upload != null && Upload.Length > 1 == true)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Upload.CopyToAsync(memoryStream);


                    if (memoryStream.Length < 4194304)
                    {

                        Site.ImageData = memoryStream.ToArray();

                    }
                    else
                    {
                        ModelState.AddModelError("Upload", "That file is too large. It should be under 4MB.");
                        ViewData["SiteTypeID"] = new SelectList(_context.SiteTypes, "SiteTypeID", "Name");
                        return Page();
                    }
                }
            }
            else
            {
                Site.ImageData = Site.ImageData;
            }

            _context.Attach(Site).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(Site.SiteID))
                {
                    return NotFound();
                }
                //else
                //{
                //    throw;
                //}
            }

            return RedirectToPage("./Index");
        }

        private bool SiteExists(int id)
        {
            return (_context.Sites?.Any(e => e.SiteID == id)).GetValueOrDefault();
        }
    }
}



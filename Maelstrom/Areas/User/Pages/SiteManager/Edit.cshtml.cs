using EF_Models.Models;
using Maelstrom.Services;
using Maelstrom.ValidationAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        [BindProperty]
        [UploadFileExtensions(Extensions = ".jpeg,.jpg")]
        public IFormFile? Upload { get; set; }
        public string? SiteImage { get; private set; }

        public AppUser AppUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            this.AppUser = await _appUserService.FindAppUser(User.Identity);
            var site = await _appUserService.GetAppUserSite(AppUser, id);

            // maybe think  access denied logic and also think about custom 404 page
            if (site == null)
            {
                return NotFound();
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
                ViewData["SiteTypeID"] = new SelectList(_context.SiteTypes, "SiteTypeID", "Name");
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

                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152)
                    {

                        Site.ImageData = memoryStream.ToArray();

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
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SiteExists(int id)
        {
            return (_context.Sites?.Any(e => e.SiteID == id)).GetValueOrDefault();
        }
    }
}



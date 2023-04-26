using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Maelstrom.Areas.User.Pages.SiteManager
{
    //refactor
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;
        private readonly IAppUserService _appUserService;

        public DeleteModel(EF_Models.MaelstromContext context, IAppUserService appUserService)
        {
            _context = context;
            _appUserService = appUserService;
        }

        [BindProperty]
        public Site Site { get; set; } = default!;
        public SiteUser SiteUser { get; set; }
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
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Sites == null)
            {
                return NotFound();
            }
            var site = await _context.Sites.FindAsync(id);

            if (site != null)
            {
                Site = site;

                _context.Sites.Remove(Site);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

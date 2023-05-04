using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Maelstrom.Areas.User.Pages.SiteUserManager
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly MaelstromContext _context;
        private readonly IAppUserService _appUserService;

        public EditModel(MaelstromContext context, IAppUserService appUserService)
        {
            _context = context;
            _appUserService = appUserService;
        }

        // pasted alot from Create Page.. so some it it may go.
        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public Site Site { get; set; }

        [BindProperty]
        public int SiteId { get; set; }
        [BindProperty]
        public int SiteUserId { get; set; }
        public SiteUser Admin { get; set; } = null!;
        [BindProperty]
        public SiteUser NewSiteUser { get; set; } = null!;

        [BindProperty]
        public bool IsAdmin { get; set; }

        [EmailAddress]
        [BindProperty]
        public string Email { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, string? message) //takes SiteUserID
        {

            if (id == null)
            {
                return BadRequest("That ID is not valid");
            }
            if (message != null)
            {
                Message = message;
            }
            var currentUser = User.Identity!;
            var currentSiteUser = await _appUserService.GetSiteUser(currentUser, id);

            if (currentSiteUser == null || currentSiteUser.IsAdmin == false)
            {
                return Forbid();// revisit          
            }
            // add logic here

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            SiteId = SiteId;

            //add logic here

            return RedirectToPage("./Index", new { id = SiteId });
        }
    }
}



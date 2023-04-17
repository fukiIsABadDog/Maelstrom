using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EF_Models.Models;
namespace Maelstrom.Areas.User.Pages.SiteManager
{
    public class CreateModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;
        public CreateModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

        public string Message { get; set; }


        [BindProperty] //[UploadFileExtensions(Extensions = ".jpeg")]
        public IFormFile Upload { get; set; }

        [BindProperty]
        public Site Site { get; set; } = default!;

        public SiteUser SiteUser { get; set; }
        public IActionResult OnGet()
        {
            ViewData["SiteTypeID"] = new SelectList(_context.SiteTypes, "SiteTypeID", "Name");
            return Page();
        }

        public void OnGet()
        {
            //get siteuser == current app user
            // bind to hidden form input element
        }
        public async Task<IActionResult> OnPostAsync()
        {
            using (var memoryStream = new MemoryStream())
            {
                await Upload.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    Site.ImageData = memoryStream.ToArray();
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            _context.Sites.Add(Site);
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch
                    {
                        Message = "There was an issue saving the data as entered.";
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }
            return RedirectToPage("./Index");
        }
    }
}

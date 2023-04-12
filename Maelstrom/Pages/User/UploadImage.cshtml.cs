using EF_Models.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Maelstrom.Pages.User
{
    public class UploadImageModel : PageModel
    {

        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; }


        public async Task<IActionResult> OnPostUploadAsync()
        {
            using (var memoryStream = new MemoryStream())
            {
                await FileUpload.FormFile.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    var file =  memoryStream.ToArray();
                    // call creation service and pass atributes   
                  
                
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }

            return Page();
        }


        //private IWebHostEnvironment _environment;
        //public UploadImageModel(IWebHostEnvironment environment)
        //{
        //    _environment = environment;
        //}

        //[BindProperty]
        //public IFormFile Upload { get; set; }
        //public async Task OnPostAsync()
        //{
        //    var file = Path.Combine(_environment.ContentRootPath, "media", Upload.FileName);
        //    using (var fileStream = new FileStream(file, FileMode.Create))
        //    {
        //        await Upload.CopyToAsync(fileStream);
        //    }

        //    RedirectToPage("/User/Dash");
        //}
    }
    public class BufferedSingleFileUploadDb
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
    }
}


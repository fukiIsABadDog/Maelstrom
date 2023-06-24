using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
//using System.Web.Mvc;

namespace Maelstrom.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SiteManager : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        public SiteManager(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        public IIdentity CurrentUser { get; private set; } = null!;
        public IList<Site> CurrentUserSites { get; private set; } = null!;
    

        [HttpGet(Name = "MySites")]
        public async Task<IList<Site>> OnGetAsync()
        {
            CurrentUser = User.Identity!; //This is going to require more configuration
            var sites = (IList<Site>)await _appUserService.GetCurrentUserSites(CurrentUser);
            CurrentUserSites = sites;

            return CurrentUserSites.ToArray();
        }
    }
}

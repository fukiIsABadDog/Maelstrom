using EF_Models.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Maelstrom.Services
{
    public interface IAppUserService
    {
        AppUser FindAppUser(IIdentity user);
        ICollection<Site> CurrentUserSites(AppUser user);
        //Site SelectedSite (ICollection<Site> sites);
        //TestResult SelectedSiteTestResults(Site site);
    }
}

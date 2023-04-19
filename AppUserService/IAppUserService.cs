﻿using EF_Models.Models;
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
    {// rename methods for consistency and conciseness
        AppUser FindAppUser(IIdentity user);
        ICollection<Site> CurrentUserSites(AppUser user);
        Site SelectedSite(ICollection<Site> sites, Site? currentSite);  
        ICollection<TestResult>  SelectedSiteTestResults(Site site);
        string GetSiteType(Site site);
        (ICollection<Site>, Dictionary<int,String>) CurrentUsersSitesAndTypes(AppUser user);
        Site? GetAppUserSite(AppUser user, int? id);
        ICollection<TestResult>? GetUserSiteTestResults(AppUser user, int? id);
        SiteUser? GetSiteUser(AppUser user, int? id);
    }
}
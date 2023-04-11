using EF_Models.Models;
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
        public AppUser FindAppUser(IIdentity user);
    }
}

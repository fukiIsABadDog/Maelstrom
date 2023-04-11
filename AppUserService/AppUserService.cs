using Microsoft.AspNetCore.Identity;
using EF_Models.Models;
using EF_Models;
using System.Security.Principal;

namespace Maelstrom.Services
{
    public class AppUserService: IAppUserService
    {
        private readonly MaelstromContext _context;

       
        
        
        public AppUserService(MaelstromContext context)
        {

            _context = context;
        }
        
        // this might require middleware... still figuring this one out

        // The idea is to be able to reuse user code accross all user pages


        public AppUser FindAppUser(IIdentity user)
        {
            var currentAppUser = _context.Users.FirstOrDefault(x => x.UserName == "DEFAULT@MAELSTROM.COM");

            if(user != null) 
            {
                currentAppUser = _context.Users.FirstOrDefault(x => x.UserName == user.Name);

               
            }

            return currentAppUser;

        }


    }
}
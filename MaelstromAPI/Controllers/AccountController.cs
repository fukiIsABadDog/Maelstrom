using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EF_Models;
using EF_Models.Models;
using Maelstrom.API.DTO;

namespace Maelstrom.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly MaelstromContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;


        public AccountController(
            MaelstromContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newUser = new AppUser();

                    newUser.FirstName = input.FirstName;
                    newUser.LastName = input.LastName;
                    newUser.Email = input.EmailAddress;

                    var result = await _userManager.CreateAsync(
                        newUser, input.Password);
                    if (result.Succeeded)
                    {
                        return StatusCode(201,
                            $"User '{newUser.UserName}' has been created.");
                    }
                    else
                    {
                        throw new Exception(
                            string.Format("Error:{0}",
                            string.Join(" ",
                            result.Errors.Select(e => e.Description))));
                    }
                }
                else
                {
                    var details = new ValidationProblemDetails(ModelState);
                    details.Status = StatusCodes.Status400BadRequest;

                    return new BadRequestObjectResult(details);
                }
            }
            catch (Exception e)
            {
                var exceptionDetails = new ProblemDetails();
                exceptionDetails.Detail = e.Message;
                exceptionDetails.Status =
                    StatusCodes.Status500InternalServerError;

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    exceptionDetails);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Login()
        {
            throw new NotImplementedException();
        }
    }
}

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
        private readonly IConfiguration _configuration;


        public AccountController(
            MaelstromContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration; 
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
                    newUser.UserName = input.EmailAddress; // setting to email to fit in with existing data access

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
        public async Task<ActionResult> Login(LoginDTO input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(input.UserName);
                    if (user == null || !await _userManager
                        .CheckPasswordAsync(user, input.Password))
                        throw new Exception("Invalid login attempt.");
                    else
                    {
                        var signingCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(
                                System.Text.Encoding.UTF8.GetBytes(
                                    _configuration["JWT:SigningKey"])),
                            SecurityAlgorithms.HmacSha256);

                        var claims = new List<Claim>();
                        claims.Add(
                            new Claim(
                            ClaimTypes.Name,
                            user.UserName));

                        var jwtObject = new JwtSecurityToken(
                            issuer: _configuration["JWT:Issuer"],
                            audience: _configuration["JWT:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddSeconds(600),
                            signingCredentials: signingCredentials);

                        var jwtString = new JwtSecurityTokenHandler() // code fails here
                            .WriteToken(jwtObject);

                        var test = 1;
                        return StatusCode(
                            StatusCodes.Status200OK, jwtString);
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
                exceptionDetails.Status = StatusCodes.Status401Unauthorized;

                return StatusCode(
                    StatusCodes.Status401Unauthorized,
                    exceptionDetails);
            }
        }
    }
}

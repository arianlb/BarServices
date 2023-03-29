using BarServices.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarServices.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDBContext context;

        public AccountController(
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            SignInManager<IdentityUser> signInManager, 
            IConfiguration configuration,
            ApplicationDBContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<string>> SignUp(UserCredentialsWithRole userCredentials)
        {
            if(!await roleManager.RoleExistsAsync(userCredentials.Role))
            {
                return BadRequest($"The role: {userCredentials.Role} does not exist in database.");
            }
            IdentityUser user = new() { UserName = userCredentials.UserName };
            var userNew = await userManager.CreateAsync(user, userCredentials.Password);
            if (userNew.Succeeded)
            {
                await userManager.AddToRoleAsync(user, userCredentials.Role);
                return CreateToken(userCredentials.UserName, userCredentials.Role);
            }
            else
            {
                return BadRequest(userNew.Errors);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(UserCredentials userCredentials)
        {
            var auth = await signInManager.PasswordSignInAsync(userCredentials.UserName, userCredentials.Password, isPersistent: false, lockoutOnFailure: false);
            if (auth.Succeeded)
            {
                var userId = await context.Users
                    .Where(u => u.UserName == userCredentials.UserName)
                    .Select(u => u.Id)
                    .FirstOrDefaultAsync();
                var roleId = await context.UserRoles
                    .Where(r => r.UserId == userId)
                    .Select(r => r.RoleId)
                    .FirstOrDefaultAsync();
                var roleName = await context.Roles
                    .Where(r => r.Id == roleId)
                    .Select(r => r.Name)
                    .FirstOrDefaultAsync();
                
                
                return CreateToken(userCredentials.UserName, roleName);
            }
            else
            {
                return BadRequest("Failed authentication");
            }
        }

        [HttpDelete("user/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var deleted = await context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
            if (deleted == 0) { return NotFound(); }
            return NoContent();
        }

        private string CreateToken(string userName, string role)
        {
            List<Claim> claims = new()
            {
                new("username", userName),
                new("role", role)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTkey"]));
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            //var expiration = DateTime.UtcNow.AddDays(1);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        /*private async void CreateRoles()
        {
            await roleManager.CreateAsync(new IdentityRole("ADMIN"));
            await roleManager.CreateAsync(new IdentityRole("DEPENDENT"));
            await roleManager.CreateAsync(new IdentityRole("COOK"));
        }*/
    }
}

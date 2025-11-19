using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstateAPI.Data;
using RealEstateAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        ApiDbContext dbContext = new ApiDbContext();
        private IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("[action]")]
        public IActionResult Register([FromBody] User user)
        {
            var findID = dbContext.Users.FirstOrDefault(x => x.Email == user.Email);

            if (findID != null)
            {
                return BadRequest("User id already exists.");
            }
            else
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, user);
            }
        }

        [HttpGet("[action]")]

        public async Task<IActionResult> GetUsers()
        {
            var users = await dbContext.Users.ToListAsync();

            if (users is null)
                return BadRequest("No users found.");
            else
                return Ok(users);
        }

        //[HttpPost("[action]")]

        //public IActionResult Login([FromBody] User user)
        //{
        //    var getData = dbContext.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);

        //    if (getData is null)
        //        return NotFound();
        //    else
        //    {
        //        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        //        var credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //        var claims = new[]
        //        {
        //            new Claim(ClaimTypes.Email, user.Email)
        //        };

        //        var token = new JwtSecurityToken(
        //            issuer: _config["JWT:Issuer"],
        //            audience: _config["JWT:Audience"],
        //            claims: claims,
        //            expires : DateTime.Now.AddMinutes(60),
        //            signingCredentials: credintials);

        //        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        //        return Ok(jwtToken);

        //    }
        //}

        [HttpPost("[action]")]

        public IActionResult Login([FromBody] LoginRequest request)
        {
            var getData = dbContext.Users.FirstOrDefault(x => x.Email == request.Email && x.Password == request.Password);

            if (getData is null)
                return NotFound();
            else
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
                var credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, request.Email)
                };

                var token = new JwtSecurityToken(
                    issuer: _config["JWT:Issuer"],
                    audience: _config["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: credintials);

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new {token =  jwtToken });

            }
        }

    }
}

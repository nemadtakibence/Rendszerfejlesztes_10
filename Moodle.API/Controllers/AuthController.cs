using System.Diagnostics.Eventing.Reader;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text.Json;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Moodle.Core;
using Moodle.Core.Communication;
using Moodle.Core.Roles;
using Moodle.Data;
using Moodle.Data.Entities;
using Newtonsoft.Json;
using Moodle.Data.Password;
using Microsoft.IdentityModel.Tokens;

namespace Moodle.API.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase{
        private readonly MoodleDbContext context;
        private readonly IConfiguration config;
        public AuthController(MoodleDbContext ctxt, IConfiguration cnfg){
            context = ctxt;
            config = cnfg;
            ACL.InitializeList(ctxt);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginData loginData){
            if(loginData==null){
                return BadRequest(new { message = "Invalid request body" });
            }
            var user = context.Users.SingleOrDefault(x => x.Username==loginData.Username);
            if (user==null){
                return Unauthorized(new { message = "Invalid credentials- user==null" });
            }
            Console.WriteLine("user username=");
            Console.WriteLine(user.Username);
            Console.WriteLine("user password=");
            Console.WriteLine(user.Password);
            Console.WriteLine("user tostring=");
            Console.WriteLine(user.ToString);
            Console.WriteLine("logindata password=");
            Console.WriteLine(loginData.Password);
            if (Password.VerifyPassword(user.Username, user.Password, loginData.Password)){
                Console.WriteLine("Bennt van a verifypassword if-jeben");
                bool oktato=false;
                if(user.Degree_Id==3){ //HARDCODE
                    oktato=true;
                }
                var claimsArr = new[]{
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Degree_Id.ToString()),
                    new Claim(ClaimTypes.GivenName, user.Username),
                    new Claim(ClaimTypes.Name, user.Name)
                };
                if (config["Jwt:SecretKey"] == null){
                    return BadRequest(new { message = "JWT Secret key is null" });
                }
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Jwt:SecretKey"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: config["Jwt:ValidIssuer"],
                    audience: config["Jwt:ValidAudience"],
                    claims: claimsArr,
                    expires: DateTime.UtcNow.AddSeconds(200),
                    signingCredentials: credentials);
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new {message="Login successful", userName = user.Username, userId = user.Id, isOktato = oktato, token = jwtToken});
            }
            return Unauthorized(new { message = "Invalid credentials" });

        }

    }
}
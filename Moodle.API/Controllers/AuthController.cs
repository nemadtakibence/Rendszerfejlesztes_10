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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginData loginData){
            if(loginData==null){
                return BadRequest("Invalid request body");
            }
            var user = context.Users.SingleOrDefault(x => x.Username==loginData.Username);
            if(user==null){
                return Unauthorized("Invalid credentials");
            }
            if(Password.VerifyPassword(user.Username, user.Password, loginData.Password)){
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
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Jwt:SecretKey"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: claimsArr,
                    expires: DateTime.UtcNow.AddSeconds(60),
                    signingCredentials: credentials);
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new {message="Login successful", userName = user.Username, userId = user.Id, isOktato = oktato, token = jwtToken});
            }
            return Unauthorized("Invalid credentials");

        }

    }
}
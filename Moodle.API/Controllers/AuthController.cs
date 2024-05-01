using System.Diagnostics.Eventing.Reader;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text.Json;
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

namespace Moodle.API.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase{
        private MoodleDbContext context;
        public AuthController(MoodleDbContext ctxt){
            context = ctxt;
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
                return Ok(new {message="Login successful", userName = user.Username, userId = user.Id, isOktato = oktato});
            }
            return Unauthorized("Invalid credentials");

        }

    }
}
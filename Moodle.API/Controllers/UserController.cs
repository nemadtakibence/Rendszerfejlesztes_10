using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text.Json;
using Moodle.Core;

namespace Moodle.API.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase{
        private readonly IWebHostEnvironment _hostingEnvironment;

        //EHHEZ KELL MAJD AUTH
        public UserController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> UpcomingEvent(){

            return Ok();
        }

        [HttpPut("enroll/{id}")]
        public async Task<IActionResult> EnrollCourse(string id){
            
            return Ok();
        }

        [HttpDelete("optout/{id}")]
        public async Task<IActionResult> Optout(string id){
            return Ok();
        }
        
    }
}
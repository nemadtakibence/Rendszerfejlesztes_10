using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text.Json;
using Moodle.Core;
using Moodle.Data;
using Microsoft.EntityFrameworkCore.Internal;
using Moodle.Data.Entities;
using Newtonsoft.Json;

namespace Moodle.API.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase{
        private readonly MoodleDbContext context;
        //private readonly IWebHostEnvironment _hostingEnvironment;

        //EHHEZ KELL MAJD AUTH
        public UserController(MoodleDbContext ctxt)
        {
            context = ctxt;
            ACL.InitializeList(ctxt);
        }
        /*public UserController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }*/

        [HttpGet("nextevent/{username}")]
        public async Task<IActionResult> NextEvent(string username){
            int userId = context.Users.ToList().Where(x => x.Username==username).First().Id;
            var courseConn = context.MyCourses.ToList();
            var allEvents = context.Events.ToList();
            var myCourseId = new List<int>();
            foreach(var el in courseConn){
                if(el.User_Id==userId){
                    myCourseId.Add(el.Course_Id);
                }
            }
            var myEvents = new List<EEvents>();
            foreach(var el in allEvents){
                if(myCourseId.Contains(el.Course_Id)){
                    myEvents.Add(el);
                }
            }
            EEvents nearestEvent = myEvents.OrderBy(x => x.Date).First();
            var jsonEvent = JsonConvert.SerializeObject(nearestEvent);
            return Content(jsonEvent,"application/json");
        }

        /*[HttpGet]
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
        }*/
        
    }
}
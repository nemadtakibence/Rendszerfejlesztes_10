using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Moodle.Core;
using Moodle.Data;
using Moodle.Data.Entities;
using Newtonsoft.Json;

namespace Moodle.API.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase{
        private MoodleDbContext context;
        //private readonly IWebHostEnvironment _hostingEnvironment;

        public CourseController(MoodleDbContext ctxt)
        {
            context=ctxt;
        }

        /*public CourseController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }*/

        [HttpGet]
        public async Task<IActionResult> ListCourses(){
            var coursesList = context.Courses.ToList();
            var coursesJson = JsonConvert.SerializeObject(coursesList);
            return Content(coursesJson, "application/json");
        }

        [HttpGet("id/{code}")]
        public async Task<IActionResult> ListCoursesByCode(string code){
            var coursesList = context.Courses.ToList().Where(x => x.Code.Contains(code));
            var coursesJson = JsonConvert.SerializeObject(coursesList);
            return Content(coursesJson, "application/json");
        }

        [HttpGet("dept/{dept}")]
        public async Task<IActionResult> ListCoursesByDept(string dept){
            var coursesList = context.Courses.ToList().Where(x => x.Department == dept);
            var coursesJson = JsonConvert.SerializeObject(coursesList);
            return Content(coursesJson, "application/json");
        }

        [HttpGet("my/{userid}")]
        public async Task<IActionResult> ListMyCourses(int userid){            
            var myCourses = context.MyCourses.ToList().Where(x => x.User_Id==userid);
            var coursesList = context.Courses.ToList();            
            var newList = new List<ECourses>();
            foreach(var el in coursesList){
                foreach(var el2 in myCourses){
                    if(el.Id==el2.Course_Id){
                        newList.Add(el);
                    }
                }
            }
            var coursesJson = JsonConvert.SerializeObject(newList);
            return Content(coursesJson, "application/json");
        }

        [HttpGet("participants/{code}")]
        public async Task<IActionResult> ListCourseParticipants(string code){            
            int courseId = context.Courses.ToList().Where(x => x.Code == code).First().Id;
            //Console.WriteLine("participants végpontnál courseid: "+courseId);
            var userList = new List<EUsers>();
            var myCourses = context.MyCourses.ToList();
            var allUsers = context.Users.ToList();
            foreach(var el in myCourses){
                if(el.Course_Id==courseId){
                    userList.Add(allUsers.Find(x => x.Id == el.User_Id));
                }
            }
            var usersJson = JsonConvert.SerializeObject(userList);
            return Content(usersJson, "application/json");
        }
        
        /*[HttpGet]
        public async Task<IActionResult> ListCourses()
        {
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "test.json");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var json = System.IO.File.ReadAllText(filePath);
            return Content(json, "application/json");
        }

        [HttpGet("dept/{dept}")]
        public IActionResult DeptCourses(string dept){
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "test.json");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var json = System.IO.File.ReadAllText(filePath);

            var list = JsonSerializer.Deserialize<List<Course>>(json);
            var result = list.Where( x => x.Dept==dept);
            var final = JsonSerializer.Serialize(result);
            return Content(final, "application/json");
        }

        [HttpGet("mycourses/{user}")]
        public async Task<IActionResult> MyCourses(string user){
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "test.json");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var json = System.IO.File.ReadAllText(filePath);

            var list = JsonSerializer.Deserialize<List<Course>>(json);
            List<Course> almost = new List<Course>();
            foreach(var el in list){
                foreach(var st in el.EnrolledStudents){
                    if(st.Neptun.ToLower()==user.ToLower()){
                        almost.Add(el);
                    }
                }
            }
            var final = JsonSerializer.Serialize(almost);
            return Content(final, "application/json");            
        }

        [HttpGet("participants")]
        public async Task<IActionResult> Participants(){
            return Ok();
        }*/
        
    }
}


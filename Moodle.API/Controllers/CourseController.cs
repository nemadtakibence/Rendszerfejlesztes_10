using System.Security.Authentication;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Moodle.Core;
using Moodle.Data;
using Newtonsoft.Json;

namespace Moodle.API.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase{
        private readonly MoodleDbContext context;
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


using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Moodle.Core;
using Moodle.Core.Roles;
using Moodle.Data;
using Moodle.Data.Entities;
using Newtonsoft.Json;
using Moodle.Core.Communication;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Moodle.API.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase{
        private MoodleDbContext context;
        //private readonly IWebHostEnvironment _hostingEnvironment;

        public CourseController(MoodleDbContext ctxt)
        {
            context=ctxt;
            ACL.InitializeList(ctxt);
        }

        /*public CourseController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }*/

        [HttpGet]
        public async Task<IActionResult> ListCourses(){
            //Console.WriteLine(ACL.GetPermission("URK2SP"));
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

        [HttpGet("my/{username}")]
        public async Task<IActionResult> ListMyCourses(string username){
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return NotFound(); // Handle case where user with the specified username is not found
            }
            int userid = user.Id;
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

        [HttpPut("enroll/{courseCode}/{username}")]
        
        public async Task<IActionResult> Enroll(string courseCode, string userName)
        {
            Console.WriteLine(courseCode);
            Console.WriteLine(userName);
            var course = context.Courses.SingleOrDefault(x => x.Code == courseCode);
            if (course == null)
            {
                return BadRequest("Invalid course code");
            }

            var user = context.Users.SingleOrDefault(x => x.Username == userName);
            if (user == null)
            {
                return BadRequest("Invalid user name");
            }
            int courseId = context.Courses.SingleOrDefault(x => x.Code == courseCode).Id;
            int degreeId = user.Degree_Id;
            int userId = user.Id;

            var degreeList = context.Approved_Degrees.ToList().Where(x => x.Degree_Id == degreeId);
            var approvedDegreeCount = degreeList.Count(x => x.Course_Id == courseId);
            if (approvedDegreeCount > 0)
            {
                var existingCourse = context.MyCourses.Any(x => x.Course_Id == course.Id && x.User_Id == userId);
                if (!existingCourse)
                {
                    context.MyCourses.Add(new EMyCourses { Course_Id = course.Id, User_Id = userId });
                    await context.SaveChangesAsync();
                    return Ok();
            }
            else
            {
                return BadRequest("User is already enrolled in this course");
            }
        }
            else
            {
                return BadRequest("Unauthorized: Can't enroll in this course with your degree");
            }
}

        [HttpPut("newcourse")]
        public async Task<IActionResult> NewCourse([FromBody] NewCourse nc){
            if(!ACL.HasPermission(nc.Username, Roles.Teacher)){
                return Unauthorized("Invalid credentials");
            }
            string name = nc.Name;
            string code = nc.Code;
            string dept = nc.Department;
            int credit = nc.Credit;
            foreach(var el in context.Courses.ToList()){
                if(el.Code == code || el.Name == name){
                    return Conflict("Course already exists");
                }
            }
            context.Courses.Add(new ECourses{Name=name, Code=code, Department=dept, Credit=credit});
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("dropcourse/{courseCode}")]
        public async Task<IActionResult> DropCourse(string courseCode, [FromBody] string userName){
            var user = context.Users.ToList().SingleOrDefault(x => x.Username == userName);
            var course = context.Courses.ToList().SingleOrDefault(x => x.Code==courseCode);
            int userId = user.Id;
            int courseId = course.Id;
            var recordToDrop = context.MyCourses.ToList().SingleOrDefault(x => x.Course_Id==courseId && x.User_Id==userId);
            if(recordToDrop==null){
                return Conflict("The course is not in your list of courses.");
            }
            context.MyCourses.Remove(recordToDrop);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("newevent")]
        public async Task<IActionResult> NewEvent([FromBody] NewEvent ne){
            if(!ACL.HasPermission(ne.Username, Roles.Teacher)){
                return Unauthorized("Invalid credentials");
            }
            var course = context.Courses.ToList().SingleOrDefault(x => x.Name == ne.CourseName);
            if(course==null){
                return Conflict("No such course in database.");
            }
            int courseId = course.Id;
            if(ne.Name==null || ne.Date==null){
                return BadRequest("Name and/or date values are null.");
            }
            context.Events.Add(new EEvents{Course_Id=courseId, Name=ne.Name, Description=ne.Desc, Date=ne.Date});
            await context.SaveChangesAsync();
            return Ok();
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


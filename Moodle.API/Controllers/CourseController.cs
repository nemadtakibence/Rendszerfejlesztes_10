using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace MoodleApi.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase{
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CourseController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        
        [HttpGet]
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
        public async Task<IActionResult> DeptCourses(string dept){
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "test.json");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var json = System.IO.File.ReadAllText(filePath);
            return Content(json, "application/json");
        }

        [HttpGet("mycourses/{user}")]
        public async Task<IActionResult> MyCourses(string user){
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "test.json");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var json = System.IO.File.ReadAllText(filePath);
            return Content(json, "application/json");            
        }

        [HttpPost("enroll/{id}")]
        public async Task<IActionResult> EnrollCourse(string id){
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "test.json");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var json = System.IO.File.ReadAllText(filePath);
            return Content(json, "application/json");
        }
    }
}


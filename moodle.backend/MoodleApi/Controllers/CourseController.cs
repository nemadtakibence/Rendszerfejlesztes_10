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

        //[EnableCors("MyAllowSpecificOrigins")] //nem szereti a fordító
        [HttpGet]
        public IActionResult ListCourses()
        {
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "test.json");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var json = System.IO.File.ReadAllText(filePath);
            return Content(json, "application/json");
        }
        [HttpGet("filter/{dept}")]
        public IActionResult FilterCourses(string dept){
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


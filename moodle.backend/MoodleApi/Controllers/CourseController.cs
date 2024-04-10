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
        public IActionResult ListCourses()
        {
            return Ok();
        }
        [HttpGet("filter")]
        public IActionResult FilterCourses(){
            return Ok();
        }
    }
}


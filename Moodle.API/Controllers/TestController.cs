using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace MoodleApi.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase{
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TestController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetJsonFile()
        {
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


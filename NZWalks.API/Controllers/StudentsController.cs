using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //GET: https://localhost:portnumber/api/students
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studentsName = new string[]
                  {
                        "John",
                        "Alice",
                        "Bob",
                        "Emily",
                      // Add more names as needed
                  };
            return Ok(studentsName); // Assuming you want to return the names
        }
    }
}

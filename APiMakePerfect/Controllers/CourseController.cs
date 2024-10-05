using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APiMakePerfect.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace APiMakePerfect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly StudentDbContext _context;

        public CourseController(StudentDbContext context)
        {
            _context = context;
        }

        // Get all courses
        //[HttpGet]
        //public async Task<IActionResult> GetAllCourses()
        //{
        //    var courses = await _context.Courses.ToListAsync();
        //    return Ok(courses);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.StudentCourses)   // Include the StudentCourses
                .ThenInclude(sc => sc.Student)    // Optionally include the Student data
                .ToListAsync();

            return Ok(courses);
        }




        // Get course by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound($"Course with Id = {id} not found");
            }
            return Ok(course);
        }

        // Create a new course
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            if (course == null || string.IsNullOrWhiteSpace(course.CourseName))
            {
                return BadRequest("Course data is invalid.");
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourseById), new { id = course.CourseId }, course);
        }

        // Update a course
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course updatedCourse)
        {
            if (id != updatedCourse.CourseId)
            {
                return BadRequest("Course Id mismatch");
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound($"Course with Id = {id} not found");
            }

            course.CourseName = updatedCourse.CourseName;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Delete a course
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound($"Course with Id = {id} not found");
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return Ok("Course deleted successfully");
        }
    }
}

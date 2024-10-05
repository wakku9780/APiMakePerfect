using APiMakePerfect.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APiMakePerfect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly StudentDbContext _context;  // Updated to StudentDbContext

        public InstructorController(StudentDbContext context)  // Updated constructor
        {
            _context = context;
        }

        // Get all instructors
        [HttpGet]
        public async Task<IActionResult> GetAllInstructors()
        {
            var instructors = await _context.Teachers.ToListAsync();
            return Ok(instructors);
        }

        // Get instructor by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstructorById(int id)
        {
            var instructor = await _context.Teachers.FindAsync(id);
            if (instructor == null) return NotFound();
            return Ok(instructor);
        }

        // Add a new instructor
        [HttpPost]
        public async Task<IActionResult> AddInstructor([FromBody] Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInstructorById), new { id = teacher.TeacherId }, teacher);
        }

        // Update instructor details
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInstructor(int id, [FromBody] Teacher updatedTeacher)
        {
            if (id != updatedTeacher.TeacherId) return BadRequest();

            _context.Entry(updatedTeacher).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Delete an instructor
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return Ok("Instructor deleted successfully");
        }


        // Assign a teacher to a course
        [HttpPost("assign/{teacherId}/course/{courseId}")]
        public async Task<IActionResult> AssignTeacherToCourse(int teacherId, int courseId)
        {
            var teacher = await _context.Teachers.FindAsync(teacherId);
            if (teacher == null)
            {
                return NotFound($"Teacher with Id = {teacherId} not found");
            }

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound($"Course with Id = {courseId} not found");
            }

            course.TeacherId = teacherId; // Assign the teacher to the course
            await _context.SaveChangesAsync();

            return Ok(new { message = "Teacher assigned to the course successfully" });
        }

        // Get all courses assigned to a teacher
        [HttpGet("{teacherId}/courses")]
        public async Task<IActionResult> GetCoursesByTeacher(int teacherId)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.TeacherId == teacherId);

            if (teacher == null)
            {
                return NotFound($"Teacher with Id = {teacherId} not found");
            }

            var courses = teacher.Courses.Select(c => new
            {
                c.CourseId,
                c.CourseName
            }).ToList();

            return Ok(courses);
        }



    }

}

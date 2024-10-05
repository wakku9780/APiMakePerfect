using APiMakePerfect.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APiMakePerfect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseController : ControllerBase
    {
        private readonly StudentDbContext _context;

        public StudentCourseController(StudentDbContext studentDbContext)
        {
            this._context = studentDbContext;
        }


        [HttpPost("assign/{studentId}")]
        public async Task<IActionResult> AssignCoursesToStudent(int studentId, [FromBody] List<int> courseIds)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound($"Student with Id = {studentId} not found");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var courseId in courseIds)
                {
                    var course = await _context.Courses.FindAsync(courseId);
                    if (course == null)
                    {
                        return NotFound($"Course with Id = {courseId} not found");
                    }

                    if (!_context.StudentCourses.Any(sc => sc.StudentId == studentId && sc.CourseId == courseId))
                    {
                        var studentCourse = new StudentCourse
                        {
                            StudentId = studentId,
                            CourseId = courseId
                        };

                        _context.StudentCourses.Add(studentCourse);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Courses successfully assigned to student", assignedCourses = courseIds });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while assigning courses");
            }
        }


        [HttpPost("enroll/{studentId}/{courseId}")]
        public async Task<IActionResult> EnrollStudentInCourse(int studentId, int courseId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                 return NotFound($"Student with Id = {studentId} not found");
            }

        var course = await _context.Courses.FindAsync(courseId);
        if (course == null)
        {
             return NotFound($"Course with Id = {courseId} not found");
        }

        if (_context.StudentCourses.Any(sc => sc.StudentId == studentId && sc.CourseId == courseId))
        {
             return BadRequest($"Student with Id = {studentId} is already enrolled in Course with Id = {courseId}");
        }

        var studentCourse = new StudentCourse
        {
            StudentId = studentId,
            CourseId = courseId
        };

         _context.StudentCourses.Add(studentCourse);
         await _context.SaveChangesAsync();

        return Ok(new { message = "Student successfully enrolled in the course" });
}



        //[HttpPost("assign/{studentId}")]
        //public async Task<IActionResult> AssignCoursesToStudent(int studentId, [FromBody] List<int> courseIds)
        //{
        //    var student = await _context.Students.FindAsync(studentId);
        //    if (student == null)
        //    {
        //        return NotFound($"Student with Id = {studentId} not found");
        //    }

        //    foreach (var courseId in courseIds)
        //    {
        //        var course = await _context.Courses.FindAsync(courseId);
        //        if (course == null)
        //        {
        //            return NotFound($"Course with Id = {courseId} not found");
        //        }

        //        // Check if the course is already assigned
        //        if (!_context.StudentCourses.Any(sc => sc.StudentId == studentId && sc.CourseId == courseId))
        //        {
        //            var studentCourse = new StudentCourse
        //            {
        //                StudentId = studentId,
        //                CourseId = courseId
        //            };

        //            _context.StudentCourses.Add(studentCourse);
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //    return Ok("Courses successfully assigned to student");
        //}


        // 2. Get Courses by Student
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetCoursesByStudent(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound($"Student with Id = {studentId} not found");
            }

            var courses = student.StudentCourses.Select(sc => new
            {
                sc.Course.CourseId,
                sc.Course.CourseName
            }).ToList();

            return Ok(courses);
        }

        // 3. Remove Course from Student
        [HttpDelete("remove/{studentId}/{courseId}")]
        public async Task<IActionResult> RemoveCourseFromStudent(int studentId, int courseId)
        {
            var studentCourse = await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (studentCourse == null)
            {
                return NotFound("Course not assigned to the student");
            }

            _context.StudentCourses.Remove(studentCourse);
            await _context.SaveChangesAsync();

            return Ok("Course assignment removed from student");
        }



    }
}

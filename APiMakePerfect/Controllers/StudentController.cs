using APiMakePerfect.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata.Ecma335;

namespace APiMakePerfect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDbContext studentDbContext;

        public StudentController(StudentDbContext studentDbContext)
        {
            this.studentDbContext = studentDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetStudents()
        {

            var result = await  studentDbContext.Students.ToListAsync();

            return result;
        }

        [HttpGet("id")]
        public async Task<ActionResult<Student>> getStudentById(int id)
        {
            var student =  await studentDbContext.Students.FindAsync(id);


            if (student == null)
            {
                return NotFound();

            }


            return student;
        }

        [HttpPost]
        public async Task<ActionResult> createStudent(Student student)
        {
            if (student == null)
            {
                return NotFound();
            }
            await studentDbContext.Students.AddAsync(student);
            await studentDbContext.SaveChangesAsync();
            return Ok(student);
        }

        [HttpGet("{studentId}/courses")]
        public async Task<IActionResult> GetCoursesByStudentId(int studentId)
        {
            var student = await studentDbContext.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course) // This line includes the Course details
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound($"Student with Id = {studentId} not found");
            }

            var courses = student.StudentCourses.Select(sc => new
            {
                CourseId = sc.CourseId,
                CourseName = sc.Course.CourseName // Now this will include course names
            }).ToList();

            return Ok(new
            {
                StudentId = student.Id,
                StudentName = student.StudentName,
                EnrolledCourses = courses
            });
        }


        [HttpPut("id")]

        public async  Task<IActionResult> updateStudent(int id,Student student)
        {
            if(id != student.Id)
            {
                return BadRequest();
            }

            var ups= await studentDbContext.Students.FindAsync(id);
            ups.StudentName=student.StudentName;
            ups.Age = student.Age;
            await studentDbContext.SaveChangesAsync();

            return NoContent();



        }

        [HttpDelete("id")]

        public async Task<IActionResult> DeleteById(int id)
        {
            var student = await studentDbContext.Students.FindAsync(id);

            if (student == null) return NotFound();

             studentDbContext.Students.Remove(student);

            await studentDbContext.SaveChangesAsync();

            return Ok(student);
        }


        [HttpGet("gender/{gender}")]

        public async Task<ActionResult<List<Student>>> getStudentByGender(String gender)
        {
            var students = await studentDbContext.Students.Where(x => x.StudentGender == gender).ToListAsync();

            return Ok(students);
        }

        [HttpGet("age/{age}")]
        public async Task<ActionResult<List<Student>>> getStudentByAge(int age)
        {
            var students = await studentDbContext.Students.Where(x => x.Age == age).ToListAsync();

            return Ok(students);
        }


        [HttpGet("search/{keyword}")]
        public async Task<ActionResult<List<Student>>> SearchStudents(string keyword)
        {
            var students = await studentDbContext.Students.Where(s => s.StudentName.Contains(keyword) || s.FatherName.Contains(keyword)).ToListAsync();
            return Ok(students);
        }

    }
}

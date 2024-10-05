namespace APiMakePerfect.Model
{
    public class Course
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        // Navigation property for many-to-many relationship
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();




        // Foreign key for the Teacher who teaches this course
        public int? TeacherId { get; set; }  // New foreign key
        public Teacher? Teacher { get; set; } = null!;  // Navigation property for Teacher
    }
}

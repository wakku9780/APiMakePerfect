namespace APiMakePerfect.Model
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string Name { get; set; } = null!;
        public string SubjectExpertise { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        // One-to-Many relation: A teacher can handle many courses
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}

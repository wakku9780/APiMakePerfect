using Microsoft.EntityFrameworkCore;

namespace APiMakePerfect.Model
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext()
        {

        }
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            // Define one-to-many relationship between Teacher and Course
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);  // Use SetNull or Restrict based on your requirement


            modelBuilder.Entity<Course>()
    .HasOne(c => c.Teacher)
    .WithMany(t => t.Courses)
    .HasForeignKey(c => c.TeacherId)
    .OnDelete(DeleteBehavior.SetNull);  // Ensure this matches your migration

        }


        // DbSets
        public DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }




        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Define many-to-many relationship between Student and Course via StudentCourse
        //    modelBuilder.Entity<StudentCourse>()
        //        .HasKey(sc => new { sc.StudentId, sc.CourseId });

        //    modelBuilder.Entity<StudentCourse>()
        //        .HasOne(sc => sc.Student)
        //        .WithMany(s => s.StudentCourses)
        //        .HasForeignKey(sc => sc.StudentId);

        //    modelBuilder.Entity<StudentCourse>()
        //        .HasOne(sc => sc.Course)
        //        .WithMany(c => c.StudentCourses)
        //        .HasForeignKey(sc => sc.CourseId);

        //    // Define one-to-many relationship between Teacher and Course
        //    modelBuilder.Entity<Course>()
        //        .HasOne(c => c.Teacher)
        //        .WithMany(t => t.Courses)
        //        .HasForeignKey(c => c.TeacherId);
        //}

        //// Add DbSet for Teacher
        //public DbSet<Teacher> Teachers { get; set; }

        //// Existing DbSets
        //public virtual DbSet<Student> Students { get; set; }
        //public DbSet<Course> Courses { get; set; }
        //public DbSet<StudentCourse> StudentCourses { get; set; }
    }
}


//using Microsoft.EntityFrameworkCore;

//namespace APiMakePerfect.Model
//{
//    public class StudentDbContext :DbContext
//    {
//        public StudentDbContext()
//        {

//        }
//        public StudentDbContext(DbContextOptions<StudentDbContext> options ):base(options)
//        {


//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<StudentCourse>()
//                .HasKey(sc => new { sc.StudentId, sc.CourseId });

//            modelBuilder.Entity<StudentCourse>()
//                .HasOne(sc => sc.Student)
//                .WithMany(s => s.StudentCourses)
//                .HasForeignKey(sc => sc.StudentId);

//            modelBuilder.Entity<StudentCourse>()
//                .HasOne(sc => sc.Course)
//                .WithMany(c => c.StudentCourses)
//                .HasForeignKey(sc => sc.CourseId);
//        }


//        public virtual DbSet<Student> Students { get; set; }
//        public DbSet<Course> Courses { get; set; }
//        public DbSet<StudentCourse> StudentCourses { get; set; }


//    }
//}

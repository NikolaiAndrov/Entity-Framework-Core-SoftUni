using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Common;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
            
        }

        public StudentSystemContext(DbContextOptions options)
            : base(options) 
        {
            
        }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Homework> Homeworks { get; set; }

        public virtual DbSet<Resource> Resources { get; set; }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<StudentCourse> StudentsCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DBConfig.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(k => new {k.StudentId, k.CourseId});

            modelBuilder.Entity<Homework>()
                .Property(x => x.Content)
                .IsUnicode(false);

            modelBuilder.Entity<Resource>()
                .Property(x => x.Url)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(x => x.PhoneNumber)
                .IsUnicode(false);

            base.OnModelCreating(modelBuilder);
        }
    } 
}
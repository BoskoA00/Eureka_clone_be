using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace epp_be.Data
{
    public class DatabaseContext : DbContext 
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Messages1> FunctionalSkillsMessages { get; set; }
        public DbSet<Messages2> ContactMessages { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options)
        {
            
        }
    }
}

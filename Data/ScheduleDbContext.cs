using Microsoft.EntityFrameworkCore;
using TimejApi.Data.Models;

namespace TimejApi.Data
{
    public class ScheduleDbContext: DbContext
    {
        public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options): base(options) 
        {
        }

        public DbSet<Building> Buildings { get; set; }
        public DbSet<Auditory> Auditories { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonType> LessonTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
    }
}

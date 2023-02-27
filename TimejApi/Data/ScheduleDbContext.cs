using Microsoft.EntityFrameworkCore;
using TimejApi.Data.Entities;

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
        public DbSet<UserEditFacultyPermission> UserEditFacultyPermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<AuthenticationModel> AuthenticationModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.AllowedFaculties)
                .WithMany(f => f.Editors)
                .UsingEntity<UserEditFacultyPermission>();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Moodle.Data.Entities;

namespace Moodle.Data{
    public class MoodleDbContext : DbContext{
        public MoodleDbContext() { }
        public MoodleDbContext(DbContextOptions options) : base(options) { }
        public DbSet<EApproved_Degrees> Approved_Degrees {get;set;}
        public DbSet<ECourses> Courses {get;set;}
        public DbSet<EDegrees> Degrees {get;set;}
        public DbSet<EEvents> Events {get;set;}
        public DbSet<EMyCourses> MyCourses {get;set;}
        public DbSet<EUsers> Users {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            string projectRoot = Directory.GetParent(Environment.CurrentDirectory).FullName;
            string loginInfoPath = Path.Combine(projectRoot, "Moodle.Data/moodleDatabase.db");
            optionsBuilder.UseSqlite(@"Data Source = " + loginInfoPath);
        }
    }
}
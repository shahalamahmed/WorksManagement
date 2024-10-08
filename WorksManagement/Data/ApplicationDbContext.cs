using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorksManagement.Models;

namespace WorksManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<WorkTask> WorkTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for Project
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 1,
                    Name = "Website Development",
                    Code = "WD001",
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 6, 1),
                    Status = ProjectStatus.Ongoing,
                },
                new Project
                {
                    Id = 2,
                    Name = "Mobile App Development",
                    Code = "MA002",
                    StartDate = new DateTime(2024, 3, 15),
                    EndDate = null,
                    Status = ProjectStatus.New,
                }
            );
        }
    }
}

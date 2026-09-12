using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Existing DbSets - DON'T DELETE
        // Keep all your Member 1/2 DbSets here

        public DbSet<EmployerProfile> EmployerProfiles
        { get; set; }

        public DbSet<Job> Jobs
        { get; set; }

        public DbSet<JobSkill> JobSkills
        { get; set; }
    }
}
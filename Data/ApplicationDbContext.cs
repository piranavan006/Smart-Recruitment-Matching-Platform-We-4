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

        // Users
        public DbSet<User> Users { get; set; }

        // Job Seeker
        public DbSet<JobSeekerProfile> JobSeekerProfiles { get; set; }

        // Employer
        public DbSet<EmployerProfile> EmployerProfiles { get; set; }

        // Jobs
        public DbSet<Job> Jobs { get; set; }

        // Applications
        public DbSet<Application> Applications { get; set; }

        // CV
        public DbSet<CV> CVs { get; set; }

        // Skills
        public DbSet<Skill> Skills { get; set; }

        // Job Skills
        public DbSet<JobSkill> JobSkills { get; set; }

        // Job Seeker Skills
        public DbSet<JobSeekerSkill> JobSeekerSkills { get; set; }

        // Contact Requests
        public DbSet<ContactRequest> ContactRequests { get; set; }

        // Notifications
        public DbSet<Notification> Notifications { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<JobSeekerProfile> JobSeekerProfiles { get; set; }

    public DbSet<EmployerProfile> EmployerProfiles { get; set; }

    public DbSet<Job> Jobs { get; set; }

    public DbSet<Application> Applications { get; set; }

    public DbSet<CV> CVs { get; set; }

    public DbSet<Skill> Skills { get; set; }

    public DbSet<JobSkill> JobSkills { get; set; }

    public DbSet<JobSeekerSkill> JobSeekerSkills { get; set; }

    public DbSet<ContactRequest> ContactRequests { get; set; }

    public DbSet<Notification> Notifications { get; set; }
}
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

        // =========================
        // DbSets
        // =========================

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


        // =========================
        // Model Configuration
        // =========================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // =====================================================
            // EMPLOYER PROFILE -> JOBS
            // =====================================================

            modelBuilder.Entity<Job>()
                .HasOne(j => j.EmployerProfile)
                .WithMany(e => e.Jobs)
                .HasForeignKey(j => j.EmployerProfileId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // USER -> JOB SEEKER PROFILE
            // =====================================================

            modelBuilder.Entity<JobSeekerProfile>()
                .HasOne(j => j.User)
                .WithMany()
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // USER -> CV
            // =====================================================

            modelBuilder.Entity<CV>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // JOB -> APPLICATIONS
            // =====================================================

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Job)
                .WithMany()
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // APPLICATION -> JOB SEEKER PROFILE
            // JobSeekerProfileId
            // =====================================================

            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobSeekerProfile)
                .WithMany()
                .HasForeignKey(a => a.JobSeekerProfileId)
                .OnDelete(DeleteBehavior.NoAction);


            // =====================================================
            // APPLICATION -> JOB SEEKER PROFILE
            // JobSeekerId
            // =====================================================

            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobSeeker)
                .WithMany()
                .HasForeignKey(a => a.JobSeekerId)
                .OnDelete(DeleteBehavior.NoAction);


            // =====================================================
            // JOB SKILL -> JOB
            // =====================================================

            modelBuilder.Entity<JobSkill>()
                .HasOne(js => js.Job)
                .WithMany()
                .HasForeignKey(js => js.JobId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // JOB SKILL -> SKILL
            // =====================================================

            modelBuilder.Entity<JobSkill>()
                .HasOne(js => js.Skill)
                .WithMany()
                .HasForeignKey(js => js.SkillId)
                .OnDelete(DeleteBehavior.NoAction);


            // =====================================================
            // JOB SEEKER SKILL -> JOB SEEKER PROFILE
            // =====================================================

            modelBuilder.Entity<JobSeekerSkill>()
                .HasOne(js => js.JobSeekerProfile)
                .WithMany()
                .HasForeignKey(js => js.JobSeekerProfileId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // JOB SEEKER SKILL -> SKILL
            // =====================================================

            modelBuilder.Entity<JobSeekerSkill>()
                .HasOne(js => js.Skill)
                .WithMany()
                .HasForeignKey(js => js.SkillId)
                .OnDelete(DeleteBehavior.NoAction);


            // =====================================================
            // CONTACT REQUEST -> SENDER
            // =====================================================

            modelBuilder.Entity<ContactRequest>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.NoAction);


            // =====================================================
            // CONTACT REQUEST -> RECEIVER
            // =====================================================

            modelBuilder.Entity<ContactRequest>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);


            // =====================================================
            // NOTIFICATION -> USER
            // =====================================================

            modelBuilder.Entity<Notification>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            // =====================================================
            // DECIMAL PRECISION
            // =====================================================

            modelBuilder.Entity<Application>()
                .Property(a => a.MatchScore)
                .HasPrecision(18, 2);


            modelBuilder.Entity<Job>()
                .Property(j => j.SalaryMin)
                .HasPrecision(18, 2);


            modelBuilder.Entity<Job>()
                .Property(j => j.SalaryMax)
                .HasPrecision(18, 2);
        }
    }
}
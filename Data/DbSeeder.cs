using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Helpers;
using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Data
{
    public static class DbSeeder
    {
        /// <summary>
        /// Seeds platform administrator, skills taxonomy, 3 new employer profiles (pending admin approval),
        /// and the 3 weighted jobs for recruitment matching tests.
        /// </summary>
        public static void Seed(ApplicationDbContext context)
        {
            SeedAdminUser(context);
            SeedSkills(context);
            SeedEmployersAndJobs(context);
        }

        /// <summary>
        /// Seeds the default platform administrator.
        /// </summary>
        public static void SeedAdminUser(ApplicationDbContext context)
        {
            const string adminEmail = "admin@smartrecruitment.com";
            const string adminPassword = "Admin@12345";

            var existingAdmin = context.Users
                .FirstOrDefault(u => u.Email.ToLower() == adminEmail.ToLower());

            if (existingAdmin == null)
            {
                var admin = new User
                {
                    FullName = "Platform Administrator",
                    Email = adminEmail,
                    PasswordHash = PasswordHasher.HashPassword(adminPassword),
                    Role = "Administrator",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }
            else
            {
                existingAdmin.Role = "Administrator";
                existingAdmin.IsActive = true;
                existingAdmin.PasswordHash = PasswordHasher.HashPassword(adminPassword);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Seeds standard technical skills for recruitment matching.
        /// </summary>
        public static void SeedSkills(ApplicationDbContext context)
        {
            var skillsToSeed = new[]
            {
                "C#",
                "ASP.NET Core",
                "SQL Server",
                "Angular",
                "Git",
                "REST API",
                "TypeScript",
                "HTML/CSS",
                "JavaScript",
                "Docker",
                "Azure",
                "React"
            };

            var existingSkills = context.Skills.ToList();
            var existingNames = new HashSet<string>(
                existingSkills.Select(s => s.SkillName.ToLower().Trim()),
                StringComparer.OrdinalIgnoreCase);

            bool anyAdded = false;
            foreach (var skillName in skillsToSeed)
            {
                if (!existingNames.Contains(skillName.ToLower().Trim()))
                {
                    context.Skills.Add(new Skill
                    {
                        SkillName = skillName,
                        Description = $"Technical skill competency in {skillName}"
                    });
                    anyAdded = true;
                }
            }

            if (anyAdded)
            {
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Seeds 3 new employers with Pending verification status and adds the 3 user-specified mock jobs with their skill weights.
        /// </summary>
        public static void SeedEmployersAndJobs(ApplicationDbContext context)
        {
            var skillsDict = context.Skills
                .ToDictionary(s => s.SkillName.ToLower().Trim(), s => s);

            const string defaultEmployerPassword = "Employer@12345";

            // =========================================================================
            // EMPLOYER 1: Apex Technologies Ltd
            // =========================================================================
            const string emp1Email = "employer1@apextech.com";
            var user1 = context.Users.FirstOrDefault(u => u.Email.ToLower() == emp1Email.ToLower());
            if (user1 == null)
            {
                user1 = new User
                {
                    FullName = "Apex Technologies Ltd",
                    Email = emp1Email,
                    PasswordHash = PasswordHasher.HashPassword(defaultEmployerPassword),
                    Role = "Employer",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                context.Users.Add(user1);
                context.SaveChanges();
            }

            var profile1 = context.EmployerProfiles.FirstOrDefault(p => p.UserId == user1.UserId.ToString());
            if (profile1 == null)
            {
                profile1 = new EmployerProfile
                {
                    UserId = user1.UserId.ToString(),
                    CompanyName = "Apex Technologies Ltd",
                    CompanyDescription = "We are an innovative enterprise software engineering solutions provider delivering scalable cloud and web applications.",
                    Industry = "Information Technology",
                    Location = "Colombo / Hybrid",
                    Website = "https://apextech.io",
                    IsApproved = false,
                    ApprovalStatus = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                context.EmployerProfiles.Add(profile1);
                context.SaveChanges();
            }

            // -------------------------------------------------------------------------
            // Job 1 — Software Engineer
            // -------------------------------------------------------------------------
            const string job1Title = "Software Engineer";
            var job1 = context.Jobs
                .Include(j => j.JobSkills)
                .FirstOrDefault(j => j.EmployerProfileId == profile1.EmployerProfileId && j.Title == job1Title);

            if (job1 == null)
            {
                job1 = new Job
                {
                    EmployerProfileId = profile1.EmployerProfileId,
                    Title = job1Title,
                    Description = "We are looking for a motivated Software Engineer to design, develop, test and maintain scalable web applications. The successful candidate will work closely with our development team to deliver reliable and high-quality software solutions.",
                    MinExperienceYears = 1,
                    MaxExperienceYears = 3,
                    SalaryMin = 120000m,
                    SalaryMax = 180000m,
                    EmploymentType = "Full Time",
                    Location = "Colombo / Hybrid",
                    Education = "Bachelor's Degree in Computer Science, Software Engineering or related field",
                    ApplicationDeadline = DateTime.UtcNow.AddMonths(2),
                    IsClosed = false,
                    Status = "Open",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                AddJobSkill(job1, context, skillsDict, "C#", 25m);
                AddJobSkill(job1, context, skillsDict, "ASP.NET Core", 25m);
                AddJobSkill(job1, context, skillsDict, "SQL Server", 20m);
                AddJobSkill(job1, context, skillsDict, "Angular", 15m);
                AddJobSkill(job1, context, skillsDict, "Git", 10m);
                AddJobSkill(job1, context, skillsDict, "REST API", 5m);

                context.Jobs.Add(job1);
                context.SaveChanges();
            }
            else if (job1.JobSkills == null || !job1.JobSkills.Any())
            {
                AddJobSkill(job1, context, skillsDict, "C#", 25m);
                AddJobSkill(job1, context, skillsDict, "ASP.NET Core", 25m);
                AddJobSkill(job1, context, skillsDict, "SQL Server", 20m);
                AddJobSkill(job1, context, skillsDict, "Angular", 15m);
                AddJobSkill(job1, context, skillsDict, "Git", 10m);
                AddJobSkill(job1, context, skillsDict, "REST API", 5m);
                context.SaveChanges();
            }

            // =========================================================================
            // EMPLOYER 2: PixelCraft Digital Studios
            // =========================================================================
            const string emp2Email = "employer2@pixelcraft.io";
            var user2 = context.Users.FirstOrDefault(u => u.Email.ToLower() == emp2Email.ToLower());
            if (user2 == null)
            {
                user2 = new User
                {
                    FullName = "PixelCraft Digital Studios",
                    Email = emp2Email,
                    PasswordHash = PasswordHasher.HashPassword(defaultEmployerPassword),
                    Role = "Employer",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                context.Users.Add(user2);
                context.SaveChanges();
            }

            var profile2 = context.EmployerProfiles.FirstOrDefault(p => p.UserId == user2.UserId.ToString());
            if (profile2 == null)
            {
                profile2 = new EmployerProfile
                {
                    UserId = user2.UserId.ToString(),
                    CompanyName = "PixelCraft Digital Studios",
                    CompanyDescription = "Award-winning creative design and frontend engineering agency crafting interactive digital web experiences.",
                    Industry = "Web & Interactive Media",
                    Location = "Colombo / Remote",
                    Website = "https://pixelcraft.io",
                    IsApproved = false,
                    ApprovalStatus = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                context.EmployerProfiles.Add(profile2);
                context.SaveChanges();
            }

            // -------------------------------------------------------------------------
            // Job 2 — Frontend Developer
            // -------------------------------------------------------------------------
            const string job2Title = "Frontend Developer";
            var job2 = context.Jobs
                .Include(j => j.JobSkills)
                .FirstOrDefault(j => j.EmployerProfileId == profile2.EmployerProfileId && j.Title == job2Title);

            if (job2 == null)
            {
                job2 = new Job
                {
                    EmployerProfileId = profile2.EmployerProfileId,
                    Title = job2Title,
                    Description = "We are seeking a creative Frontend Developer to build responsive, accessible and user-friendly web interfaces. The role involves working closely with backend developers and UI designers to deliver modern digital experiences.",
                    MinExperienceYears = 1,
                    MaxExperienceYears = 2,
                    SalaryMin = 100000m,
                    SalaryMax = 160000m,
                    EmploymentType = "Full Time",
                    Location = "Colombo / Remote",
                    Education = "Bachelor's Degree or Diploma in Web Development, IT, or equivalent practical experience",
                    ApplicationDeadline = DateTime.UtcNow.AddMonths(2),
                    IsClosed = false,
                    Status = "Open",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                AddJobSkill(job2, context, skillsDict, "Angular", 30m);
                AddJobSkill(job2, context, skillsDict, "TypeScript", 25m);
                AddJobSkill(job2, context, skillsDict, "HTML/CSS", 20m);
                AddJobSkill(job2, context, skillsDict, "JavaScript", 15m);
                AddJobSkill(job2, context, skillsDict, "Git", 10m);

                context.Jobs.Add(job2);
                context.SaveChanges();
            }
            else if (job2.JobSkills == null || !job2.JobSkills.Any())
            {
                AddJobSkill(job2, context, skillsDict, "Angular", 30m);
                AddJobSkill(job2, context, skillsDict, "TypeScript", 25m);
                AddJobSkill(job2, context, skillsDict, "HTML/CSS", 20m);
                AddJobSkill(job2, context, skillsDict, "JavaScript", 15m);
                AddJobSkill(job2, context, skillsDict, "Git", 10m);
                context.SaveChanges();
            }

            // =========================================================================
            // EMPLOYER 3: YarlTech Solutions
            // =========================================================================
            const string emp3Email = "employer3@yarltech.lk";
            var user3 = context.Users.FirstOrDefault(u => u.Email.ToLower() == emp3Email.ToLower());
            if (user3 == null)
            {
                user3 = new User
                {
                    FullName = "YarlTech Solutions",
                    Email = emp3Email,
                    PasswordHash = PasswordHasher.HashPassword(defaultEmployerPassword),
                    Role = "Employer",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                context.Users.Add(user3);
                context.SaveChanges();
            }

            var profile3 = context.EmployerProfiles.FirstOrDefault(p => p.UserId == user3.UserId.ToString());
            if (profile3 == null)
            {
                profile3 = new EmployerProfile
                {
                    UserId = user3.UserId.ToString(),
                    CompanyName = "YarlTech Solutions",
                    CompanyDescription = "Fast-growing regional tech accelerator nurturing emerging software engineering talent in Northern Province.",
                    Industry = "Software Development & Consulting",
                    Location = "Jaffna / Hybrid",
                    Website = "https://yarltech.lk",
                    IsApproved = false,
                    ApprovalStatus = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                context.EmployerProfiles.Add(profile3);
                context.SaveChanges();
            }

            // -------------------------------------------------------------------------
            // Job 3 — Junior Software Developer
            // -------------------------------------------------------------------------
            const string job3Title = "Junior Software Developer";
            var job3 = context.Jobs
                .Include(j => j.JobSkills)
                .FirstOrDefault(j => j.EmployerProfileId == profile3.EmployerProfileId && j.Title == job3Title);

            if (job3 == null)
            {
                job3 = new Job
                {
                    EmployerProfileId = profile3.EmployerProfileId,
                    Title = job3Title,
                    Description = "An excellent opportunity for a junior developer to contribute to real-world software projects while working with experienced developers. Candidates will participate in development, testing and maintenance activities.",
                    MinExperienceYears = 0,
                    MaxExperienceYears = 1,
                    SalaryMin = 70000m,
                    SalaryMax = 110000m,
                    EmploymentType = "Full Time",
                    Location = "Jaffna / Hybrid",
                    Education = "Diploma or Bachelor's Degree in Computer Science, IT or related field",
                    ApplicationDeadline = DateTime.UtcNow.AddMonths(2),
                    IsClosed = false,
                    Status = "Open",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                AddJobSkill(job3, context, skillsDict, "C#", 25m);
                AddJobSkill(job3, context, skillsDict, "SQL Server", 20m);
                AddJobSkill(job3, context, skillsDict, "HTML/CSS", 20m);
                AddJobSkill(job3, context, skillsDict, "JavaScript", 20m);

                context.Jobs.Add(job3);
                context.SaveChanges();
            }
            else if (job3.JobSkills == null || !job3.JobSkills.Any())
            {
                AddJobSkill(job3, context, skillsDict, "C#", 25m);
                AddJobSkill(job3, context, skillsDict, "SQL Server", 20m);
                AddJobSkill(job3, context, skillsDict, "HTML/CSS", 20m);
                AddJobSkill(job3, context, skillsDict, "JavaScript", 20m);
                context.SaveChanges();
            }
        }

        private static void AddJobSkill(
            Job job,
            ApplicationDbContext context,
            Dictionary<string, Skill> skillsDict,
            string skillName,
            decimal weight)
        {
            var key = skillName.ToLower().Trim();
            if (!skillsDict.TryGetValue(key, out var skill))
            {
                skill = new Skill
                {
                    SkillName = skillName,
                    Description = $"Technical skill competency in {skillName}"
                };
                context.Skills.Add(skill);
                context.SaveChanges();
                skillsDict[key] = skill;
            }

            job.JobSkills.Add(new JobSkill
            {
                SkillId = skill.SkillId,
                SkillName = skill.SkillName,
                Weight = weight
            });
        }
    }
}

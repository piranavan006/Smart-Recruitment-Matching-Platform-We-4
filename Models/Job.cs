using System.ComponentModel.DataAnnotations;

﻿
namespace SmartRecruitment.API.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        public int EmployerProfileId { get; set; }

        public EmployerProfile? EmployerProfile { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Location { get; set; }

        [MaxLength(100)]
        public string? Education { get; set; }

        [Range(0, 100)]
        public int MinExperienceYears { get; set; }

        [Range(0, 100)]
        public int MaxExperienceYears { get; set; }

        public decimal? SalaryMin { get; set; }

        public decimal? SalaryMax { get; set; }

        [Required]
        public DateTime ApplicationDeadline { get; set; }

        public bool IsClosed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<JobSkill> RequiredSkills { get; set; }
            = new List<JobSkill>();
        public int JobId { get; set; }

        public int EmployerProfileId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string EmploymentType { get; set; } = string.Empty;

        public int MinExperience { get; set; }

        public int MaxExperience { get; set; }

        public decimal MinSalary { get; set; }

        public decimal MaxSalary { get; set; }

        public string Status { get; set; } = "Open";

        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ClosingDate { get; set; }

        public EmployerProfile? EmployerProfile { get; set; }

        public ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();
    }
}
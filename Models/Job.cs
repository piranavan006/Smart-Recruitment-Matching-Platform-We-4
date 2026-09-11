
namespace SmartRecruitment.API.Models
{
    public class Job
    {
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
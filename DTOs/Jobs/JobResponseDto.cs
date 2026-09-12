namespace SmartRecruitment.API.DTOs
{
    public class JobResponseDto
    {
        public int Id { get; set; }

        public int EmployerProfileId { get; set; }

        public string? CompanyName { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? Location { get; set; }

        public string? Education { get; set; }

        public int MinExperienceYears { get; set; }

        public int MaxExperienceYears { get; set; }

        public decimal? SalaryMin { get; set; }

        public decimal? SalaryMax { get; set; }

        public DateTime ApplicationDeadline { get; set; }

        public bool IsClosed { get; set; }

        public List<JobSkillDto> RequiredSkills { get; set; }
            = new List<JobSkillDto>();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
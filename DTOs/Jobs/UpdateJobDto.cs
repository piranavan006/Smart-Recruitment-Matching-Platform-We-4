using System.ComponentModel.DataAnnotations;

namespace SmartRecruitment.API.DTOs
{
    public class UpdateJobDto
    {
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

        [Range(0, double.MaxValue)]
        public decimal? SalaryMin { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SalaryMax { get; set; }

        [Required]
        public DateTime ApplicationDeadline { get; set; }

        public List<JobSkillDto> RequiredSkills { get; set; }
            = new List<JobSkillDto>();
    }
}
using System.ComponentModel.DataAnnotations;

namespace SmartRecruitment.API.DTOs
{
    public class JobSkillDto
    {
        [Required]
        [MaxLength(100)]
        public string SkillName { get; set; } = string.Empty;

        [Range(1, 100)]
        public int Weight { get; set; } = 1;
    }
}
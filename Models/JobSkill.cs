using System.ComponentModel.DataAnnotations;

namespace SmartRecruitment.API.Models
{
    public class JobSkill
    {
        public int Id { get; set; }

        [Required]
        public int JobId { get; set; }

        public Job? Job { get; set; }

        [Required]
        [MaxLength(100)]
        public string SkillName { get; set; } = string.Empty;

        [Range(1, 100)]
        public int Weight { get; set; } = 1;
        public int JobSkillId { get; set; }

        public int JobId { get; set; }

        public int SkillId { get; set; }

        public decimal Weight { get; set; }

        // Navigation properties
        public Job? Job { get; set; }

        public Skill? Skill { get; set; }
    }
}
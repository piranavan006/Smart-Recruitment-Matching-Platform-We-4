using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRecruitment.API.Models
{
    public class JobSkill
    {
        [Key]
        public int Id { get; set; }


        // =====================
        // Job Foreign Key
        // =====================

        [ForeignKey(nameof(Job))]
        public int JobId { get; set; }


        // =====================
        // Skill Foreign Key
        // =====================

        [ForeignKey(nameof(Skill))]
        public int SkillId { get; set; }


        // =====================
        // Skill Details
        // =====================

        [Required]
        [MaxLength(100)]
        public string SkillName { get; set; } = string.Empty;


        public decimal Weight { get; set; } = 1;


        // =====================
        // Navigation Properties
        // =====================

        public Job? Job { get; set; }

        public Skill? Skill { get; set; }
    }
}
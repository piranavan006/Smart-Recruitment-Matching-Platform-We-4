namespace SmartRecruitment.API.Models
{
    public class JobSkill
    {
        public int JobSkillId { get; set; }

        public int JobId { get; set; }

        public int SkillId { get; set; }

        public decimal Weight { get; set; }

        // Navigation properties
        public Job? Job { get; set; }

        public Skill? Skill { get; set; }
    }
}

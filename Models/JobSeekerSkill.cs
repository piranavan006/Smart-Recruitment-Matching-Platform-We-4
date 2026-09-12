namespace SmartRecruitment.API.Models
{
    public class JobSeekerSkill
    {
        public int JobSeekerSkillId { get; set; }

        public int JobSeekerProfileId { get; set; }

        public int SkillId { get; set; }

        public decimal Weight { get; set; }

        // Navigation properties
        public JobSeekerProfile? JobSeekerProfile { get; set; }

        public Skill? Skill { get; set; }
    }
}

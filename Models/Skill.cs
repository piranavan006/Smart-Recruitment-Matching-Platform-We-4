namespace SmartRecruitment.API.Models
{
    public class Skill
    {
        public int SkillId { get; set; }

        public string SkillName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
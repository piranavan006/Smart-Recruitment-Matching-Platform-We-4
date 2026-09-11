namespace SmartRecruitment.API.DTOs.Skills
{
    public class SkillResponseDto
    {
        public int SkillId { get; set; }

        public string SkillName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
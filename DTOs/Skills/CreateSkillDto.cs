
namespace SmartRecruitment.API.DTOs.Skills
{
    public class CreateSkillDto
    {
        public string SkillName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
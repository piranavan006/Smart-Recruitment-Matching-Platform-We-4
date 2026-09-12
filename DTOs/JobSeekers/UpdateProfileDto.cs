namespace SmartRecruitment.API.DTOs.JobSeekers
{
    public class UpdateProfileDto
    {
        public string Summary { get; set; } = string.Empty;

        public string Skills { get; set; } = string.Empty;

        public string Experience { get; set; } = string.Empty;

        public string Education { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;
    }
}
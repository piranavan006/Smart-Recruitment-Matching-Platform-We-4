namespace SmartRecruitment.API.DTOs
{
    public class MatchResultDto
    {
        public int JobId { get; set; }

        public int JobSeekerId { get; set; }

        public string? JobTitle { get; set; }

        public string? CandidateName { get; set; }

        public double MatchScore { get; set; }

        public List<string> MissingSkills { get; set; }
            = new List<string>();

        public bool SkillsMatched { get; set; }

        public bool ExperienceMatched { get; set; }

        public bool EducationMatched { get; set; }

        public bool LocationMatched { get; set; }
    }
}
namespace SmartRecruitment.API.DTOs
{
    public class JobSearchDto
    {
        public string? Keyword { get; set; }

        public string? Location { get; set; }

        public string? Education { get; set; }

        public int? MinExperienceYears { get; set; }

        public int? MaxExperienceYears { get; set; }

        public decimal? SalaryMin { get; set; }

        public decimal? SalaryMax { get; set; }
    }
}
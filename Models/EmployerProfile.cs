namespace SmartRecruitment.API.Models
{
    public class EmployerProfile
    {
        public int EmployerProfileId { get; set; }

        public int UserId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string? CompanyDescription { get; set; }

        public string? Industry { get; set; }

        public string? Location { get; set; }

        public string? Website { get; set; }
    }
}
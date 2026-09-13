namespace SmartRecruitment.API.DTOs
{
    public class EmployerResponseDto
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string? Industry { get; set; }

        public string? Website { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public bool IsApproved { get; set; }

        public string ApprovalStatus { get; set; } = "Pending";
    }
}
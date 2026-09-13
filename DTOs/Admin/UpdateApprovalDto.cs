namespace SmartRecruitment.API.DTOs.Admin
{
    public class UpdateApprovalDto
    {
        public bool IsApproved { get; set; }

        public string? Status { get; set; } // "Approved", "Rejected", "Pending"

        public string? Reason { get; set; }
    }
}

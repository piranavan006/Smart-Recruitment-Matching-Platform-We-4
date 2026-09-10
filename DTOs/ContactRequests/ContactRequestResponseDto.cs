namespace SmartRecruitment.API.DTOs.ContactRequests
{
    public class ContactRequestResponseDto
    {
        public int ContactRequestId { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public string Message { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
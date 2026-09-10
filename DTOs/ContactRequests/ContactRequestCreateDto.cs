namespace SmartRecruitment.API.DTOs.ContactRequests
{
    public class ContactRequestCreateDto
    {
        public int ReceiverId { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
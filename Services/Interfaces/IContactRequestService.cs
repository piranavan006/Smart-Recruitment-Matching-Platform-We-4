using SmartRecruitment.API.DTOs.ContactRequests;

namespace SmartRecruitment.API.Services.Interfaces
{
    public interface IContactRequestService
    {
        Task<ContactRequestResponseDto>
            CreateAsync(
                int senderId,
                ContactRequestCreateDto dto);

        Task<List<ContactRequestResponseDto>>
            GetSentAsync(int senderId);

        Task<List<ContactRequestResponseDto>>
            GetReceivedAsync(int receiverId);

        Task<ContactRequestResponseDto?>
            RespondAsync(
                int requestId,
                int receiverId,
                ContactRequestStatusUpdateDto dto);
    }
}
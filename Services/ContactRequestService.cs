using SmartRecruitment.API.DTOs.ContactRequests;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Services
{
    public class ContactRequestService
        : IContactRequestService
    {
        private readonly IContactRequestRepository
            _repository;

        public ContactRequestService(
            IContactRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<ContactRequestResponseDto>
            CreateAsync(
                int senderId,
                ContactRequestCreateDto dto)
        {
            if (dto.ReceiverId <= 0)
            {
                throw new ArgumentException(
                    "Invalid receiver ID.");
            }

            if (dto.ReceiverId == senderId)
            {
                throw new ArgumentException(
                    "You cannot send a request to yourself.");
            }

            if (string.IsNullOrWhiteSpace(dto.Message))
            {
                throw new ArgumentException(
                    "Message is required.");
            }

            bool exists =
                await _repository.ExistsPendingAsync(
                    senderId,
                    dto.ReceiverId);

            if (exists)
            {
                throw new InvalidOperationException(
                    "A pending contact request already exists.");
            }

            var request = new ContactRequest
            {
                SenderId = senderId,

                ReceiverId = dto.ReceiverId,

                Message = dto.Message.Trim(),

                Status = "Pending",

                CreatedAt = DateTime.UtcNow
            };

            var result =
                await _repository.AddAsync(request);

            return MapToDto(result);
        }

        public async Task<List<ContactRequestResponseDto>>
            GetSentAsync(int senderId)
        {
            var requests =
                await _repository
                    .GetBySenderIdAsync(senderId);

            return requests
                .Select(MapToDto)
                .ToList();
        }

        public async Task<List<ContactRequestResponseDto>>
            GetReceivedAsync(int receiverId)
        {
            var requests =
                await _repository
                    .GetByReceiverIdAsync(receiverId);

            return requests
                .Select(MapToDto)
                .ToList();
        }

        public async Task<ContactRequestResponseDto?>
            RespondAsync(
                int requestId,
                int receiverId,
                ContactRequestStatusUpdateDto dto)
        {
            if (dto.Status != "Accepted" &&
                dto.Status != "Declined")
            {
                throw new ArgumentException(
                    "Status must be Accepted or Declined.");
            }

            var request =
                await _repository
                    .GetByIdAsync(requestId);

            if (request == null)
            {
                return null;
            }

            if (request.ReceiverId != receiverId)
            {
                throw new UnauthorizedAccessException(
                    "You cannot respond to this request.");
            }

            if (request.Status != "Pending")
            {
                throw new InvalidOperationException(
                    "This request has already been processed.");
            }

            request.Status = dto.Status;

            await _repository.UpdateAsync(request);

            return MapToDto(request);
        }

        private static ContactRequestResponseDto
            MapToDto(ContactRequest request)
        {
            return new ContactRequestResponseDto
            {
                ContactRequestId =
                    request.ContactRequestId,

                SenderId =
                    request.SenderId,

                ReceiverId =
                    request.ReceiverId,

                Message =
                    request.Message,

                Status =
                    request.Status,

                CreatedAt =
                    request.CreatedAt
            };
        }
    }
}
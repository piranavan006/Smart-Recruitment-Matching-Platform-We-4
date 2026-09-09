
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Services
{
    public class ContactRequestService : IContactRequestService
    {
        private readonly IContactRequestRepository _repository;

        public ContactRequestService(IContactRequestRepository repository)
        {
            _repository = repository;
        }

        // Interface methods இங்கே இருக்க வேண்டும்
    }
}
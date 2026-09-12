using SmartRecruitment.API.DTOs;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly IEmployerRepository _repository;

        public EmployerService(IEmployerRepository repository)
        {
            _repository = repository;
        }

        // =========================
        // CREATE EMPLOYER PROFILE
        // =========================
        public async Task<EmployerResponseDto> CreateProfileAsync(
            string userId,
            CreateEmployerProfileDto dto)
        {
            var existing =
                await _repository.GetByUserIdAsync(userId);

            if (existing != null)
            {
                throw new InvalidOperationException(
                    "Employer profile already exists.");
            }

            var employer = new EmployerProfile
            {
                UserId = userId,
                CompanyName = dto.CompanyName.Trim(),
                Industry = dto.Industry?.Trim(),
                Website = dto.Website?.Trim(),
                CompanyDescription = dto.Description?.Trim(),
                Location = dto.Location?.Trim()
            };

            var created =
                await _repository.CreateAsync(employer);

            return MapToDto(created);
        }

        // =========================
        // GET PROFILE
        // =========================
        public async Task<EmployerResponseDto?> GetProfileAsync(
            string userId)
        {
            var employer =
                await _repository.GetByUserIdAsync(userId);

            if (employer == null)
                return null;

            return MapToDto(employer);
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<EmployerResponseDto?> GetByIdAsync(
            int employerProfileId)
        {
            var employer =
                await _repository.GetByIdAsync(
                    employerProfileId);

            if (employer == null)
                return null;

            return MapToDto(employer);
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<List<EmployerResponseDto>>
            GetAllAsync()
        {
            var employers =
                await _repository.GetAllAsync();

            return employers
                .Select(MapToDto)
                .ToList();
        }

        // =========================
        // UPDATE PROFILE
        // =========================
        public async Task<EmployerResponseDto?>
            UpdateProfileAsync(
                string userId,
                UpdateEmployerProfileDto dto)
        {
            var employer =
                await _repository.GetByUserIdAsync(userId);

            if (employer == null)
                return null;

            employer.CompanyName =
                dto.CompanyName.Trim();

            employer.Industry =
                dto.Industry?.Trim();

            employer.Website =
                dto.Website?.Trim();

            employer.CompanyDescription =
                dto.Description?.Trim();

            employer.Location =
                dto.Location?.Trim();

            employer.UpdatedAt =
                DateTime.UtcNow;

            var updated =
                await _repository.UpdateAsync(employer);

            return MapToDto(updated);
        }

        // =========================
        // DELETE PROFILE
        // =========================
        public async Task<bool>
            DeleteProfileAsync(
                string userId)
        {
            var employer =
                await _repository.GetByUserIdAsync(userId);

            if (employer == null)
                return false;

            await _repository.DeleteAsync(
                employer.EmployerProfileId);

            return true;
        }

        // =========================
        // CHECK EXISTS
        // =========================
        public async Task<bool>
            ExistsByUserIdAsync(
                string userId)
        {
            return await _repository
                .ExistsByUserIdAsync(userId);
        }

        // =========================
        // MAPPING
        // =========================
        private static EmployerResponseDto
            MapToDto(
                EmployerProfile employer)
        {
            return new EmployerResponseDto
            {
                Id =
                    employer.EmployerProfileId,

                UserId =
                    employer.UserId,

                CompanyName =
                    employer.CompanyName,

                Industry =
                    employer.Industry,

                Website =
                    employer.Website,

                Description =
                    employer.CompanyDescription,

                Location =
                    employer.Location
            };
        }
    }
}
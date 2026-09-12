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

        public async Task<EmployerResponseDto> CreateProfileAsync(
            string userId,
            CreateEmployerProfileDto dto)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException(
                    "User ID is required.");
            }

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                throw new ArgumentException(
                    "Company name is required.");
            }

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
                Description = dto.Description?.Trim(),
                Location = dto.Location?.Trim()
            };

            var created =
                await _repository.CreateAsync(employer);

            return MapToDto(created);
        }

        public async Task<EmployerResponseDto?> GetProfileAsync(
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException(
                    "User ID is required.");
            }

            var employer =
                await _repository.GetByUserIdAsync(userId);

            if (employer == null)
            {
                return null;
            }

            return MapToDto(employer);
        }

        public async Task<EmployerResponseDto?> UpdateProfileAsync(
            string userId,
            UpdateEmployerProfileDto dto)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException(
                    "User ID is required.");
            }

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                throw new ArgumentException(
                    "Company name is required.");
            }

            var employer =
                await _repository.GetByUserIdAsync(userId);

            if (employer == null)
            {
                return null;
            }

            employer.CompanyName =
                dto.CompanyName.Trim();

            employer.Industry =
                dto.Industry?.Trim();

            employer.Website =
                dto.Website?.Trim();

            employer.Description =
                dto.Description?.Trim();

            employer.Location =
                dto.Location?.Trim();

            var updated =
                await _repository.UpdateAsync(employer);

            return MapToDto(updated);
        }

        private static EmployerResponseDto MapToDto(
            EmployerProfile employer)
        {
            return new EmployerResponseDto
            {
                Id = employer.Id,
                UserId = employer.UserId,
                CompanyName = employer.CompanyName,
                Industry = employer.Industry,
                Website = employer.Website,
                Description = employer.Description,
                Location = employer.Location
            };
        }
    }
}
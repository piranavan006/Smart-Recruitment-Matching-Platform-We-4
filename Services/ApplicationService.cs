using SmartRecruitment.API.DTOs.Applications;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repository;
        private readonly IJobSeekerRepository _jobSeekerRepository;

        public ApplicationService(
            IApplicationRepository repository,
            IJobSeekerRepository jobSeekerRepository)
        {
            _repository = repository;
            _jobSeekerRepository = jobSeekerRepository;
        }

        public async Task<ApplicationResponseDto>
            ApplyAsync(
                int jobSeekerId,
                ApplicationCreateDto dto)
        {
            if (dto.JobId <= 0)
            {
                throw new ArgumentException(
                    "Invalid Job ID.");
            }

            bool alreadyApplied =
                await _repository.ExistsAsync(
                    jobSeekerId,
                    dto.JobId);

            if (alreadyApplied)
            {
                throw new InvalidOperationException(
                    "You have already applied for this job.");
            }

            var profile = await _jobSeekerRepository.GetByUserIdAsync(jobSeekerId);
            if (profile == null)
            {
                profile = await _jobSeekerRepository.AddAsync(new JobSeekerProfile
                {
                    UserId = jobSeekerId
                });
            }

            var application = new Application
            {
                JobId = dto.JobId,

                JobSeekerId = jobSeekerId,

                JobSeekerProfileId = profile.JobSeekerProfileId,

                Status = "Pending",

                MatchScore = null,

                AppliedAt = DateTime.UtcNow,

                UpdatedAt = DateTime.UtcNow
            };

            var result =
                await _repository.AddAsync(application);

            return MapToDto(result);
        }

        public async Task<ApplicationResponseDto?>
            GetByIdAsync(int applicationId)
        {
            var application =
                await _repository.GetByIdAsync(
                    applicationId);

            if (application == null)
            {
                return null;
            }

            return MapToDto(application);
        }

        public async Task<List<ApplicationResponseDto>>
            GetMyApplicationsAsync(int jobSeekerId)
        {
            var applications =
                await _repository.GetByJobSeekerIdAsync(
                    jobSeekerId);

            return applications
                .Select(MapToDto)
                .ToList();
        }

        public async Task<List<ApplicationResponseDto>>
            GetByJobAsync(int jobId)
        {
            var applications =
                await _repository.GetByJobIdAsync(jobId);

            return applications
                .Select(MapToDto)
                .ToList();
        }

        public async Task<ApplicationResponseDto?>
            UpdateStatusAsync(
                int applicationId,
                ApplicationStatusUpdateDto dto)
        {
            string[] validStatuses =
            {
                "Pending",
                "Reviewed",
                "Shortlisted",
                "Accepted",
                "Rejected"
            };

            if (!validStatuses.Contains(
                dto.Status,
                StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Invalid application status.");
            }

            var application =
                await _repository.GetByIdAsync(
                    applicationId);

            if (application == null)
            {
                return null;
            }

            application.Status = dto.Status;

            application.UpdatedAt =
                DateTime.UtcNow;

            await _repository.UpdateAsync(
                application);

            return MapToDto(application);
        }

        private static ApplicationResponseDto
            MapToDto(Application application)
        {
            return new ApplicationResponseDto
            {
                ApplicationId =
                    application.ApplicationId,

                JobId =
                    application.JobId,

                JobSeekerId =
                    application.JobSeekerId,

                Status =
                    application.Status,

                MatchScore =
                    application.MatchScore,

                AppliedAt =
                    application.AppliedAt,

                UpdatedAt =
                    application.UpdatedAt
            };
        }
    }
}
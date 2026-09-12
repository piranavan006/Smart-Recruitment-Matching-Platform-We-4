using SmartRecruitment.API.DTOs;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IEmployerRepository _employerRepository;

        public JobService(
            IJobRepository jobRepository,
            IEmployerRepository employerRepository)
        {
            _jobRepository = jobRepository;
            _employerRepository = employerRepository;
        }

        public async Task<JobResponseDto> CreateAsync(
            string userId,
            CreateJobDto dto)
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

            ValidateJob(dto);

            var employer =
                await _employerRepository.GetByUserIdAsync(userId);

            if (employer == null)
            {
                throw new KeyNotFoundException(
                    "Employer profile not found.");
            }

            var job = new Job
            {
                EmployerProfileId = employer.Id,
                Title = dto.Title.Trim(),
                Description = dto.Description.Trim(),
                Location = dto.Location?.Trim(),
                Education = dto.Education?.Trim(),
                MinExperienceYears = dto.MinExperienceYears,
                MaxExperienceYears = dto.MaxExperienceYears,
                SalaryMin = dto.SalaryMin,
                SalaryMax = dto.SalaryMax,
                ApplicationDeadline =
                    dto.ApplicationDeadline,
                IsClosed = false
            };

            if (dto.RequiredSkills != null)
            {
                foreach (var skill in dto.RequiredSkills)
                {
                    if (string.IsNullOrWhiteSpace(
                        skill.SkillName))
                    {
                        continue;
                    }

                    job.RequiredSkills.Add(
                        new JobSkill
                        {
                            SkillName =
                                skill.SkillName.Trim(),

                            Weight =
                                skill.Weight < 1
                                    ? 1
                                    : skill.Weight
                        });
                }
            }

            var created =
                await _jobRepository.CreateAsync(job);

            return MapToDto(created);
        }

        public async Task<JobResponseDto?> GetByIdAsync(
            int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Invalid job ID.");
            }

            var job =
                await _jobRepository.GetByIdAsync(id);

            if (job == null)
            {
                return null;
            }

            return MapToDto(job);
        }

        public async Task<List<JobResponseDto>> GetMyJobsAsync(
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException(
                    "User ID is required.");
            }

            var employer =
                await _employerRepository.GetByUserIdAsync(userId);

            if (employer == null)
            {
                throw new KeyNotFoundException(
                    "Employer profile not found.");
            }

            var jobs =
                await _jobRepository.GetByEmployerIdAsync(
                    employer.Id);

            return jobs
                .Select(MapToDto)
                .ToList();
        }

        public async Task<JobResponseDto?> UpdateAsync(
            string userId,
            int id,
            UpdateJobDto dto)
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

            ValidateJob(dto);

            var employer =
                await _employerRepository.GetByUserIdAsync(userId);

            if (employer == null)
            {
                throw new KeyNotFoundException(
                    "Employer profile not found.");
            }

            var job =
                await _jobRepository.GetByIdAsync(id);

            if (job == null)
            {
                return null;
            }

            if (job.EmployerProfileId != employer.Id)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to update this job.");
            }

            if (job.IsClosed)
            {
                throw new InvalidOperationException(
                    "Closed jobs cannot be updated.");
            }

            job.Title =
                dto.Title.Trim();

            job.Description =
                dto.Description.Trim();

            job.Location =
                dto.Location?.Trim();

            job.Education =
                dto.Education?.Trim();

            job.MinExperienceYears =
                dto.MinExperienceYears;

            job.MaxExperienceYears =
                dto.MaxExperienceYears;

            job.SalaryMin =
                dto.SalaryMin;

            job.SalaryMax =
                dto.SalaryMax;

            job.ApplicationDeadline =
                dto.ApplicationDeadline;

            job.UpdatedAt =
                DateTime.UtcNow;

            job.RequiredSkills.Clear();

            if (dto.RequiredSkills != null)
            {
                foreach (var skill in dto.RequiredSkills)
                {
                    if (string.IsNullOrWhiteSpace(
                        skill.SkillName))
                    {
                        continue;
                    }

                    job.RequiredSkills.Add(
                        new JobSkill
                        {
                            JobId = job.Id,
                            SkillName =
                                skill.SkillName.Trim(),

                            Weight =
                                skill.Weight < 1
                                    ? 1
                                    : skill.Weight
                        });
                }
            }

            var updated =
                await _jobRepository.UpdateAsync(job);

            return MapToDto(updated);
        }

        public async Task<bool> CloseAsync(
            string userId,
            int id)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException(
                    "User ID is required.");
            }

            var employer =
                await _employerRepository.GetByUserIdAsync(userId);

            if (employer == null)
            {
                throw new KeyNotFoundException(
                    "Employer profile not found.");
            }

            var job =
                await _jobRepository.GetByIdAsync(id);

            if (job == null)
            {
                return false;
            }

            if (job.EmployerProfileId != employer.Id)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to close this job.");
            }

            if (job.IsClosed)
            {
                return true;
            }

            job.IsClosed = true;
            job.UpdatedAt = DateTime.UtcNow;

            await _jobRepository.UpdateAsync(job);

            return true;
        }

        public async Task<List<JobResponseDto>> SearchAsync(
            JobSearchDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var jobs =
                await _jobRepository.SearchAsync(
                    dto.Keyword,
                    dto.Location,
                    dto.Education,
                    dto.MinExperienceYears,
                    dto.MaxExperienceYears,
                    dto.SalaryMin,
                    dto.SalaryMax);

            return jobs
                .Select(MapToDto)
                .ToList();
        }

        private static void ValidateJob(
            CreateJobDto dto)
        {
            ValidateJobValues(
                dto.Title,
                dto.Description,
                dto.MinExperienceYears,
                dto.MaxExperienceYears,
                dto.SalaryMin,
                dto.SalaryMax,
                dto.ApplicationDeadline);
        }

        private static void ValidateJob(
            UpdateJobDto dto)
        {
            ValidateJobValues(
                dto.Title,
                dto.Description,
                dto.MinExperienceYears,
                dto.MaxExperienceYears,
                dto.SalaryMin,
                dto.SalaryMax,
                dto.ApplicationDeadline);
        }

        private static void ValidateJobValues(
            string title,
            string description,
            int minExperience,
            int maxExperience,
            decimal? salaryMin,
            decimal? salaryMax,
            DateTime deadline)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(
                    "Job title is required.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException(
                    "Job description is required.");
            }

            if (minExperience < 0)
            {
                throw new ArgumentException(
                    "Minimum experience cannot be negative.");
            }

            if (maxExperience < minExperience)
            {
                throw new ArgumentException(
                    "Maximum experience cannot be less than minimum experience.");
            }

            if (salaryMin.HasValue &&
                salaryMax.HasValue &&
                salaryMax.Value < salaryMin.Value)
            {
                throw new ArgumentException(
                    "Maximum salary cannot be less than minimum salary.");
            }

            if (deadline <= DateTime.UtcNow)
            {
                throw new ArgumentException(
                    "Application deadline must be in the future.");
            }
        }

        private static JobResponseDto MapToDto(
            Job job)
        {
            return new JobResponseDto
            {
                Id = job.Id,

                EmployerProfileId =
                    job.EmployerProfileId,

                CompanyName =
                    job.EmployerProfile?.CompanyName,

                Title =
                    job.Title,

                Description =
                    job.Description,

                Location =
                    job.Location,

                Education =
                    job.Education,

                MinExperienceYears =
                    job.MinExperienceYears,

                MaxExperienceYears =
                    job.MaxExperienceYears,

                SalaryMin =
                    job.SalaryMin,

                SalaryMax =
                    job.SalaryMax,

                ApplicationDeadline =
                    job.ApplicationDeadline,

                IsClosed =
                    job.IsClosed,

                CreatedAt =
                    job.CreatedAt,

                UpdatedAt =
                    job.UpdatedAt,

                RequiredSkills =
                    job.RequiredSkills
                        .Select(skill =>
                            new JobSkillDto
                            {
                                SkillName =
                                    skill.SkillName,

                                Weight =
                                    skill.Weight
                            })
                        .ToList()
            };
        }
    }
}
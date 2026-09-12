using SmartRecruitment.API.DTOs;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IJobSeekerMatchingProvider _profileProvider;

        private const double SkillWeight = 50.0;
        private const double ExperienceWeight = 25.0;
        private const double EducationWeight = 15.0;
        private const double LocationWeight = 10.0;

        public MatchingService(
            IJobRepository jobRepository,
            IJobSeekerMatchingProvider profileProvider)
        {
            _jobRepository = jobRepository;
            _profileProvider = profileProvider;
        }

        public async Task<List<MatchResultDto>> GetMatchesForJobAsync(int jobId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null)
                throw new KeyNotFoundException("Job not found.");

            var profiles = await _profileProvider.GetAllProfilesAsync();

            return profiles
                .Select(profile => CalculateMatch(job, profile))
                .OrderByDescending(x => x.MatchScore)
                .ThenBy(x => x.JobSeekerId)
                .ToList();
        }

        public async Task<List<MatchResultDto>> GetMatchesForJobSeekerAsync(
            int jobSeekerId)
        {
            var profile = await _profileProvider.GetProfileAsync(jobSeekerId);

            if (profile == null)
                throw new KeyNotFoundException(
                    "Job seeker profile not found.");

            var jobs = await _jobRepository.SearchAsync(
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            return jobs
                .Select(job => CalculateMatch(job, profile))
                .OrderByDescending(x => x.MatchScore)
                .ThenBy(x => x.JobId)
                .ToList();
        }

        private static MatchResultDto CalculateMatch(
            Job job,
            JobSeekerMatchingProfile profile)
        {
            var candidateSkills = new HashSet<string>(
                profile.Skills
                    .Where(skill => !string.IsNullOrWhiteSpace(skill))
                    .Select(Normalize),
                StringComparer.OrdinalIgnoreCase);

            var requiredSkills = job.JobSkills
                .Where(skill =>
                    !string.IsNullOrWhiteSpace(skill.SkillName))
                .ToList();

            double totalSkillWeight = requiredSkills.Sum(
    skill => Math.Max(1, (double)skill.Weight)
);

            double matchedSkillWeight = requiredSkills
                .Where(skill =>
                    candidateSkills.Contains(
                        Normalize(skill.SkillName)))
                .Sum(skill => Math.Max(1, (double)skill.Weight));

            double skillScore = totalSkillWeight == 0
                ? 0
                : matchedSkillWeight / totalSkillWeight * 100;

            var missingSkills = requiredSkills
                .Where(skill =>
                    !candidateSkills.Contains(
                        Normalize(skill.SkillName)))
                .Select(skill => skill.SkillName)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(skill => skill)
                .ToList();

            bool experienceMatched =
                profile.ExperienceYears >= job.MinExperienceYears &&
                profile.ExperienceYears <= job.MaxExperienceYears;

            double experienceScore;

            if (experienceMatched)
            {
                experienceScore = 100;
            }
            else if (profile.ExperienceYears < job.MinExperienceYears)
            {
                int gap =
                    job.MinExperienceYears -
                    profile.ExperienceYears;

                experienceScore = Math.Max(
                    0,
                    100 - gap * 20);
            }
            else
            {
                experienceScore = 100;
            }

            bool educationMatched =
                string.IsNullOrWhiteSpace(job.Education) ||
                string.Equals(
                    Normalize(job.Education),
                    Normalize(profile.Education),
                    StringComparison.OrdinalIgnoreCase);

            double educationScore = educationMatched ? 100 : 0;

            bool locationMatched =
                string.IsNullOrWhiteSpace(job.Location) ||
                string.IsNullOrWhiteSpace(profile.Location) ||
                string.Equals(
                    Normalize(job.Location),
                    Normalize(profile.Location),
                    StringComparison.OrdinalIgnoreCase);

            double locationScore = locationMatched ? 100 : 0;

            double finalScore =
                (skillScore * SkillWeight / 100) +
                (experienceScore * ExperienceWeight / 100) +
                (educationScore * EducationWeight / 100) +
                (locationScore * LocationWeight / 100);

            finalScore = Math.Round(
                Math.Clamp(finalScore, 0, 100),
                2);

            return new MatchResultDto
            {
                JobId = job.Id,
                JobSeekerId = profile.JobSeekerId,
                JobTitle = job.Title,
                CandidateName = profile.FullName,
                MatchScore = finalScore,
                MissingSkills = missingSkills,
                SkillsMatched = missingSkills.Count == 0,
                ExperienceMatched = experienceMatched,
                EducationMatched = educationMatched,
                LocationMatched = locationMatched
            };
        }

        private static string Normalize(string? value)
        {
            return (value ?? string.Empty)
                .Trim()
                .ToLowerInvariant();
        }
    }
}
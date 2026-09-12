using SmartRecruitment.API.DTOs;

namespace SmartRecruitment.API.Services.Interfaces
{
    public interface IMatchingService
    {
        Task<List<MatchResultDto>> GetMatchesForJobAsync(
            int jobId);

        Task<List<MatchResultDto>> GetMatchesForJobSeekerAsync(
            int jobSeekerId);
    }
}
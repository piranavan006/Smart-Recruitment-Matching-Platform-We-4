using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Repositories.Interfaces
{
    public interface ISkillRepository
    {
        Task<Skill?> GetByIdAsync(int skillId);

        Task<Skill?> GetByNameAsync(string skillName);

        Task<List<Skill>> GetAllAsync();

        Task<Skill> AddAsync(Skill skill);

        Task UpdateAsync(Skill skill);

        Task DeleteAsync(int skillId);

        Task<bool> ExistsByIdAsync(int skillId);

        Task<bool> ExistsByNameAsync(string skillName);
    }
}
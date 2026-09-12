using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<Skill?> GetByIdAsync(int skillId)
        {
            return await _skillRepository.GetByIdAsync(skillId);
        }

        public async Task<Skill?> GetByNameAsync(string skillName)
        {
            return await _skillRepository.GetByNameAsync(skillName);
        }

        public async Task<List<Skill>> GetAllAsync()
        {
            return await _skillRepository.GetAllAsync();
        }

        public async Task<Skill> AddAsync(Skill skill)
        {
            return await _skillRepository.AddAsync(skill);
        }

        public async Task UpdateAsync(Skill skill)
        {
            await _skillRepository.UpdateAsync(skill);
        }

        public async Task DeleteAsync(int skillId)
        {
            await _skillRepository.DeleteAsync(skillId);
        }

        public async Task<bool> ExistsByIdAsync(int skillId)
        {
            return await _skillRepository.ExistsByIdAsync(skillId);
        }

        public async Task<bool> ExistsByNameAsync(string skillName)
        {
            return await _skillRepository.ExistsByNameAsync(skillName);
        }
    }
}
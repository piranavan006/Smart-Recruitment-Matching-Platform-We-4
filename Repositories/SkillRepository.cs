using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Data;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;

namespace SmartRecruitment.API.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext _context;

        public SkillRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Skill?> GetByIdAsync(int skillId)
        {
            return await _context.Skills
                .FirstOrDefaultAsync(s => s.SkillId == skillId);
        }

        public async Task<Skill?> GetByNameAsync(string skillName)
        {
            return await _context.Skills
                .FirstOrDefaultAsync(s => s.SkillName == skillName);
        }

        public async Task<List<Skill>> GetAllAsync()
        {
            return await _context.Skills
                .OrderBy(s => s.SkillName)
                .ToListAsync();
        }

        public async Task<Skill> AddAsync(Skill skill)
        {
            await _context.Skills.AddAsync(skill);
            await _context.SaveChangesAsync();

            return skill;
        }

        public async Task UpdateAsync(Skill skill)
        {
            _context.Skills.Update(skill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int skillId)
        {
            var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.SkillId == skillId);

            if (skill == null)
                return;

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int skillId)
        {
            return await _context.Skills
                .AnyAsync(s => s.SkillId == skillId);
        }

        public async Task<bool> ExistsByNameAsync(string skillName)
        {
            return await _context.Skills
                .AnyAsync(s => s.SkillName == skillName);
        }
    }
}
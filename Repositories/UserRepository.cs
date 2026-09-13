using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Data;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;

namespace SmartRecruitment.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .ToListAsync();
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await GetByIdAsync(userId);

            if (user != null)
            {
                // 1. Remove notifications
                var notifications = await _context.Notifications
                    .Where(n => n.UserId == userId)
                    .ToListAsync();
                if (notifications.Any())
                {
                    _context.Notifications.RemoveRange(notifications);
                }

                // 2. Remove contact requests (both sent and received)
                var contactRequests = await _context.ContactRequests
                    .Where(c => c.SenderId == userId || c.ReceiverId == userId)
                    .ToListAsync();
                if (contactRequests.Any())
                {
                    _context.ContactRequests.RemoveRange(contactRequests);
                }

                // 3. Remove seeker profile, skills, CVs, and applications if JobSeeker
                var seekerProfile = await _context.JobSeekerProfiles
                    .FirstOrDefaultAsync(p => p.UserId == userId);
                if (seekerProfile != null)
                {
                    var apps = await _context.Applications
                        .Where(a => a.JobSeekerId == userId || a.JobSeekerProfileId == seekerProfile.JobSeekerProfileId)
                        .ToListAsync();
                    if (apps.Any())
                    {
                        _context.Applications.RemoveRange(apps);
                    }

                    var seekerSkills = await _context.JobSeekerSkills
                        .Where(s => s.JobSeekerProfileId == seekerProfile.JobSeekerProfileId)
                        .ToListAsync();
                    if (seekerSkills.Any())
                    {
                        _context.JobSeekerSkills.RemoveRange(seekerSkills);
                    }

                    var cvs = await _context.CVs
                        .Where(c => c.UserId == userId)
                        .ToListAsync();
                    if (cvs.Any())
                    {
                        _context.CVs.RemoveRange(cvs);
                    }

                    _context.JobSeekerProfiles.Remove(seekerProfile);
                }
                else
                {
                    var apps = await _context.Applications
                        .Where(a => a.JobSeekerId == userId)
                        .ToListAsync();
                    if (apps.Any())
                    {
                        _context.Applications.RemoveRange(apps);
                    }

                    var cvs = await _context.CVs
                        .Where(c => c.UserId == userId)
                        .ToListAsync();
                    if (cvs.Any())
                    {
                        _context.CVs.RemoveRange(cvs);
                    }
                }

                // 4. Remove employer profile, jobs, and job skills if Employer
                var employerProfile = await _context.EmployerProfiles
                    .Include(e => e.Jobs)
                    .ThenInclude(j => j.JobSkills)
                    .FirstOrDefaultAsync(e => e.UserId == userId.ToString());
                if (employerProfile != null)
                {
                    _context.EmployerProfiles.Remove(employerProfile);
                }

                // 5. Remove the user entity
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email);
        }
    }
}
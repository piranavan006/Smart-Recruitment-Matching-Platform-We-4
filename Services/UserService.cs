using SmartRecruitment.API.DTOs.Users;
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly IJobRepository _jobRepository;

        public UserService(
            IUserRepository userRepository,
            IEmployerRepository employerRepository,
            IJobRepository jobRepository)
        {
            _userRepository = userRepository;
            _employerRepository = employerRepository;
            _jobRepository = jobRepository;
        }

        // ==========================================
        // GET USER BY ID
        // ==========================================
        public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return null;

            return new UserResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }

        // ==========================================
        // GET ALL USERS
        // ==========================================
        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(user => new UserResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            }).ToList();
        }

        // ==========================================
        // UPDATE USER
        // ==========================================
        public async Task<UserResponseDto?> UpdateUserAsync(
            int userId,
            UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return null;

            user.FullName = dto.FullName;
            user.Email = dto.Email;

            await _userRepository.UpdateAsync(user);

            return new UserResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }

        // ==========================================
        // UPDATE USER STATUS
        // ==========================================
        public async Task<bool> UpdateUserStatusAsync(
            int userId,
            bool isActive)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return false;

            user.IsActive = isActive;

            await _userRepository.UpdateAsync(user);

            // If an employer user is deactivated, automatically close all their active vacancies
            if (!isActive)
            {
                var employer = await _employerRepository.GetByUserIdAsync(userId.ToString());
                if (employer != null)
                {
                    var jobs = await _jobRepository.GetByEmployerIdAsync(employer.EmployerProfileId);
                    foreach (var job in jobs)
                    {
                        if (!job.IsClosed)
                        {
                            job.IsClosed = true;
                            job.Status = "Closed";
                            job.UpdatedAt = DateTime.UtcNow;
                            await _jobRepository.UpdateAsync(job);
                        }
                    }
                }
            }

            return true;
        }

        // ==========================================
        // DELETE USER
        // ==========================================
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return false;

            if (user.Email.Equals("admin@smartrecruitment.com", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Default platform administrator cannot be deleted.");
            }

            await _userRepository.DeleteAsync(userId);

            return true;
        }
    }
}
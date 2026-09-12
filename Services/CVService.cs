using Microsoft.AspNetCore.Http;
using SmartRecruitment.API.DTOs.CV;
using SmartRecruitment.API.Helpers;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Services
{
    public class CVService : ICVService
    {
        private readonly ICVRepository _cvRepository;
        private readonly FileStorageHelper _fileStorageHelper;

        public CVService(
            ICVRepository cvRepository,
            FileStorageHelper fileStorageHelper)
        {
            _cvRepository = cvRepository;
            _fileStorageHelper = fileStorageHelper;
        }

        public async Task<CvResponseDto> UploadAsync(
            int userId,
            IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("CV file is required.");
            }

            var existingCv =
                await _cvRepository.GetByUserIdAsync(userId);

            if (existingCv != null)
            {
                _fileStorageHelper.DeleteCv(
                    existingCv.FilePath);
            }

            var result =
                await _fileStorageHelper.SaveCvAsync(file);

            var cv = new CV
            {
                UserId = userId,
                FileName = result.FileName,
                FilePath = result.FilePath,
                UploadedAt = DateTime.UtcNow
            };

            if (existingCv != null)
            {
                existingCv.FileName = cv.FileName;
                existingCv.FilePath = cv.FilePath;
                existingCv.UploadedAt = cv.UploadedAt;

                await _cvRepository.UpdateAsync(existingCv);

                return MapToDto(existingCv);
            }

            var savedCv =
                await _cvRepository.AddAsync(cv);

            return MapToDto(savedCv);
        }

        public async Task<CvResponseDto?> GetByUserIdAsync(
            int userId)
        {
            var cv =
                await _cvRepository.GetByUserIdAsync(userId);

            if (cv == null)
            {
                return null;
            }

            return MapToDto(cv);
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            var cv =
                await _cvRepository.GetByUserIdAsync(userId);

            if (cv == null)
            {
                return false;
            }

            _fileStorageHelper.DeleteCv(cv.FilePath);

            await _cvRepository.DeleteAsync(cv.CVId);

            return true;
        }

        private static CvResponseDto MapToDto(CV cv)
        {
            return new CvResponseDto
            {
                JobSeekerProfileId = cv.UserId,
                CVFileName = cv.FileName,
                CVFilePath = cv.FilePath,
                CVUploadedAt = cv.UploadedAt
            };
        }
    }
}
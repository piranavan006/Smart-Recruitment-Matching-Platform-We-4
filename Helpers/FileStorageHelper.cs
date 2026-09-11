using Microsoft.AspNetCore.Http;

namespace SmartRecruitment.API.Helpers
{
    public class FileStorageHelper
    {
        private readonly IWebHostEnvironment _environment;

        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        private static readonly string[] AllowedExtensions =
        {
            ".pdf",
            ".doc",
            ".docx"
        };

        public FileStorageHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<(string FileName, string FilePath)> SaveCvAsync(
            IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("CV file is required.");
            }

            if (file.Length > MaxFileSize)
            {
                throw new ArgumentException(
                    "CV file size must not exceed 5 MB.");
            }

            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
            {
                throw new ArgumentException(
                    "Only PDF, DOC and DOCX files are allowed.");
            }

            var uploadFolder = Path.Combine(
                _environment.ContentRootPath,
                "Uploads",
                "CVs");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var uniqueFileName =
                $"{Guid.NewGuid()}{extension}";

            var fullPath = Path.Combine(
                uploadFolder,
                uniqueFileName);

            using (var stream = new FileStream(
                fullPath,
                FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath =
                Path.Combine(
                    "Uploads",
                    "CVs",
                    uniqueFileName)
                .Replace("\\", "/");

            return (file.FileName, relativePath);
        }

        public bool DeleteCv(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return false;
            }

            var fullPath = Path.Combine(
                _environment.ContentRootPath,
                relativePath.Replace(
                    "/",
                    Path.DirectorySeparatorChar.ToString()));

            if (!File.Exists(fullPath))
            {
                return false;
            }

            File.Delete(fullPath);

            return true;
        }
    }
}
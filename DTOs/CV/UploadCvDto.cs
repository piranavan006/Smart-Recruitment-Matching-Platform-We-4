using Microsoft.AspNetCore.Http;

namespace SmartRecruitment.API.DTOs.CV
{
    public class UploadCvDto
    {
        public IFormFile File { get; set; } = null!;
    }
}
using System.ComponentModel.DataAnnotations;

namespace SmartRecruitment.API.DTOs
{
    public class UpdateEmployerProfileDto
    {
        [Required]
        [MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Industry { get; set; }

        [Url]
        [MaxLength(255)]
        public string? Website { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(150)]
        public string? Location { get; set; }
    }
}
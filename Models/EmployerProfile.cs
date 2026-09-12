using System.ComponentModel.DataAnnotations;

namespace SmartRecruitment.API.Models
{
    public class EmployerProfile
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Industry { get; set; }

        [MaxLength(255)]
        public string? Website { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(150)]
        public string? Location { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Job> Jobs { get; set; }
            = new List<Job>();
        public int EmployerProfileId { get; set; }

        public int UserId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string? CompanyDescription { get; set; }

        public string? Industry { get; set; }

        public string? Location { get; set; }

        public string? Website { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SmartRecruitment.API.Models
{
    public class EmployerProfile
    {
        public int EmployerProfileId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? CompanyDescription { get; set; }

        [MaxLength(100)]
        public string? Industry { get; set; }

        [MaxLength(150)]
        public string? Location { get; set; }

        [MaxLength(255)]
        public string? Website { get; set; }

        public bool IsApproved { get; set; } = false;

        [MaxLength(50)]
        public string ApprovalStatus { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Job> Jobs { get; set; }
            = new List<Job>();
    }
}
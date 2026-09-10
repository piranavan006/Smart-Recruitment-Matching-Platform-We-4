using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Application> Applications { get; set; }

        public DbSet<ContactRequest> ContactRequests { get; set; }
    }
}
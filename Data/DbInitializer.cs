using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Data
{
    public static class DbInitializer
    {
        /// <summary>
        /// Global database seeding coordinator.
        /// Delegates to DbSeeder for complete platform initialization.
        /// </summary>
        public static void SeedData(ApplicationDbContext context)
        {
            DbSeeder.Seed(context);
        }

        /// <summary>
        /// Legacy method for backward compatibility.
        /// </summary>
        public static void SeedAdminUser(ApplicationDbContext context)
        {
            DbSeeder.SeedAdminUser(context);
        }
    }
}

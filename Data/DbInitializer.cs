using SmartRecruitment.API.Helpers;
using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Data
{
    public static class DbInitializer
    {
        public static void SeedAdminUser(ApplicationDbContext context)
        {
            const string adminEmail = "admin@smartrecruitment.com";
            const string adminPassword = "Admin@12345";

            var existingAdmin = context.Users
                .FirstOrDefault(u => u.Email.ToLower() == adminEmail.ToLower());

            if (existingAdmin == null)
            {
                var admin = new User
                {
                    FullName = "Platform Administrator",
                    Email = adminEmail,
                    PasswordHash = PasswordHasher.HashPassword(adminPassword),
                    Role = "Administrator",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }
            else
            {
                // Ensure role is Administrator, account is active, and password is set to default
                existingAdmin.Role = "Administrator";
                existingAdmin.IsActive = true;
                existingAdmin.PasswordHash = PasswordHasher.HashPassword(adminPassword);
                context.SaveChanges();
            }
        }
    }
}

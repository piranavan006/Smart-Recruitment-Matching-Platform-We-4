using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using SmartRecruitment.API.Data;
using SmartRecruitment.API.Helpers;
using SmartRecruitment.API.Repositories;
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services;
using SmartRecruitment.API.Services.Interfaces;
using SmartRecruitment.API.Settings;

using System.Text;

namespace SmartRecruitment.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ================================
            // Controllers
            // ================================
            builder.Services.AddControllers();


            // ================================
            // Database
            // ================================
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));


            // ================================
            // User Repository & Service
            // ================================
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();


            // ================================
            // Email Settings & Service
            // ================================
            builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();


            // ================================
            // Authentication
            // ================================
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<JwtHelper>();


            // ================================
            // Skill Repository & Service
            // ================================
            builder.Services.AddScoped<ISkillRepository, SkillRepository>();
            builder.Services.AddScoped<ISkillService, SkillService>();


            // ================================
            // Contact Request Repository & Service
            // ================================
            builder.Services.AddScoped<IContactRequestRepository, ContactRequestRepository>();
            builder.Services.AddScoped<IContactRequestService, ContactRequestService>();


            // ================================
            // Notification Repository & Service
            // ================================
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();


            // ================================
            // Employer Repository & Service
            // ================================
            builder.Services.AddScoped<IEmployerRepository, EmployerRepository>();
            builder.Services.AddScoped<IEmployerService, EmployerService>();


            // ================================
            // Job Repository & Service
            // ================================
            builder.Services.AddScoped<IJobRepository, JobRepository>();
            builder.Services.AddScoped<IJobService, JobService>();


            // ================================
            // Job Seeker Repository & Service
            // ================================
            builder.Services.AddScoped<IJobSeekerRepository, JobSeekerRepository>();
            builder.Services.AddScoped<IJobSeekerService, JobSeekerService>();


            // ================================
            // Application Repository & Service
            // ================================
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();


            // ================================
            // CV Repository, Helper & Service
            // ================================
            builder.Services.AddScoped<ICVRepository, CVRepository>();
            builder.Services.AddScoped<FileStorageHelper>();
            builder.Services.AddScoped<ICVService, CVService>();


            // ================================
            // Matching Provider & Service
            // ================================
            builder.Services.AddScoped<IJobSeekerMatchingProvider, JobSeekerMatchingProvider>();
            builder.Services.AddScoped<IMatchingService, MatchingService>();


            // ================================
            // CORS
            // ================================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });


            // ================================
            // JWT Authentication
            // ================================
            var jwtKey = builder.Configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException(
                    "JWT Key is missing. Please add Jwt:Key in appsettings.json."
                );
            }

            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtIssuer))
            {
                throw new InvalidOperationException(
                    "JWT Issuer is missing. Please add Jwt:Issuer in appsettings.json."
                );
            }

            if (string.IsNullOrWhiteSpace(jwtAudience))
            {
                throw new InvalidOperationException(
                    "JWT Audience is missing. Please add Jwt:Audience in appsettings.json."
                );
            }

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtKey)
                            ),

                        ValidateIssuer = true,
                        ValidIssuer = jwtIssuer,

                        ValidateAudience = true,
                        ValidAudience = jwtAudience,

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero
                    };
            });


            // ================================
            // Authorization
            // ================================
            builder.Services.AddAuthorization();


            // ================================
            // Swagger / OpenAPI
            // ================================
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // ================================
            // Build Application
            // ================================
            var app = builder.Build();


            // ================================
            // HTTP Request Pipeline
            // ================================

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            // HTTPS
            app.UseHttpsRedirection();


            // CORS
            app.UseCors("AllowAll");

            // Authentication MUST come before Authorization
            app.UseAuthentication();
            app.UseAuthorization();


            // Controllers
            app.MapControllers();

            // Seed Default Admin User, Core Skills, Sample Employers & Weighted Vacancies
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                DbSeeder.Seed(dbContext);
            }

            // Run Application
            app.Run();
        }
    }
}
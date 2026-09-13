using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SmartRecruitment.API.Services.Interfaces;
using SmartRecruitment.API.Settings;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace SmartRecruitment.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService>? _logger;

        public EmailService(
            IOptions<EmailSettings> emailSettings,
            ILogger<EmailService>? logger = null)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendOtpAsync(string toEmail, string otp)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(
                    new MailboxAddress(
                        _emailSettings.SenderName,
                        _emailSettings.SenderEmail
                    )
                );

                email.To.Add(
                    MailboxAddress.Parse(toEmail)
                );

                email.Subject = "Smart Recruitment - Password Reset OTP";

                var bodyBuilder = new BodyBuilder
                {
                    TextBody =
                        $"Hello,\n\n" +
                        $"Your Smart Recruitment password reset OTP is: {otp}\n\n" +
                        $"This OTP is valid for 5 minutes.\n\n" +
                        $"If you did not request a password reset, please ignore this email.\n\n" +
                        $"Regards,\n" +
                        $"Smart Recruitment Team",

                    HtmlBody =
                        $@"<div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 520px; margin: 0 auto; padding: 28px; border: 1px solid #e2e8f0; border-radius: 12px; background: #ffffff; color: #1e293b;"">
                            <div style=""text-align: center; margin-bottom: 24px;"">
                                <h2 style=""color: #4f46e5; margin: 0; font-size: 22px; font-weight: 700;"">Smart Recruitment</h2>
                                <p style=""color: #64748b; font-size: 13px; margin: 4px 0 0 0;"">Matching Platform</p>
                            </div>
                            <div style=""font-size: 15px; line-height: 1.6;"">
                                <p style=""margin: 0 0 12px 0;"">Hello,</p>
                                <p style=""margin: 0 0 16px 0;"">We received a request to reset your password. Use the following One-Time Password (OTP) to complete verification:</p>
                                <div style=""text-align: center; margin: 24px 0;"">
                                    <span style=""display: inline-block; font-size: 32px; font-weight: 800; letter-spacing: 8px; color: #4f46e5; background: #eef2ff; padding: 14px 28px; border-radius: 10px; border: 1px solid #c7d2fe;"">{otp}</span>
                                </div>
                                <p style=""color: #e11d48; font-size: 13px; font-weight: 600; margin: 0 0 12px 0;"">⏱ This OTP will expire in 5 minutes.</p>
                                <p style=""color: #64748b; font-size: 13px; margin: 0 0 20px 0;"">If you did not request this password reset, please ignore this email. Your account remains secure.</p>
                            </div>
                            <hr style=""border: none; border-top: 1px solid #e2e8f0; margin: 24px 0;"" />
                            <div style=""text-align: center; color: #94a3b8; font-size: 12px;"">
                                <p style=""margin: 0;"">&copy; {DateTime.UtcNow.Year} Smart Recruitment Platform. All rights reserved.</p>
                            </div>
                        </div>"
                };

                email.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(
                    _emailSettings.SmtpServer,
                    _emailSettings.Port,
                    SecureSocketOptions.StartTls
                );

                await smtp.AuthenticateAsync(
                    _emailSettings.SenderEmail,
                    _emailSettings.Password
                );

                await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);

                _logger?.LogInformation("Password reset OTP email sent successfully to {ToEmail}", toEmail);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to send password reset OTP email to {ToEmail}", toEmail);
                throw;
            }
        }
    }
}
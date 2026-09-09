using System.Security.Cryptography;

namespace SmartRecruitment.API.Helpers
{
    public static class OtpGenerator
    {
        public static string GenerateOtp()
        {
            return RandomNumberGenerator
                .GetInt32(100000, 1000000)
                .ToString();
        }
    }
}
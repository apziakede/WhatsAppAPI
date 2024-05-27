namespace WhatsAppOTPAPI.Configs
{
    public static class GlobalConfig
    {
        public static string GenerateOTP(int length)
        {
            string otp = "";
            Random random = new();
            string allowedChars = "123456789";
            for (int i = 1; i <= length; i++)
            {
                otp += allowedChars[random.Next(0, allowedChars.Length)];
            }
            return otp;
        }

        public static DateTime GetDateUtcNow(this DateTime now)
        {
            return DateTime.UtcNow;
        }
    }
}

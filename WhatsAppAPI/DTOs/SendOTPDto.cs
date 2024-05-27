namespace WhatsAppOTPAPI.DTOs
{
    public class SendOTPDto
    {
        public string ToPhoneNumber { get; set; }
        public string OTPSource { get; set; }
        public int EllapseTimeInMinutes { get; set; }
        public int OTPLength { get; set; }
    }
}

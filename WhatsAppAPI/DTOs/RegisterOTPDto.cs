using WhatsAppOTPAPI.Enums;

namespace WhatsAppOTPAPI.DTOs
{
    public class RegisterOTPDto
    {
        public string OTPToken { get; set; }
        public string Message { get; set; }
        public string OTPSource { get; set; }
        public string ToPhoneNumber { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public OTPStatus Status { get; set; }
    }
}

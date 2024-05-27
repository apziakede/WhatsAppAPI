using WhatsAppOTPAPI.Enums;

namespace WhatsAppOTPAPI.Shared
{
    public class OTPManagerResponses
    {
        public string? OTP { get; set; }
        public string? Message { get; set; }
        public string Status { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime? ExpiresOn { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}

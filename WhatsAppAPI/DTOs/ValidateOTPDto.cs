using System.ComponentModel.DataAnnotations;

namespace WhatsAppOTPAPI.DTOs
{
    public class ValidateOTPDto
    {
        [Required]
        public string OTP { get; set; }
        [Required]
        public string ToPhoneNumber { get; set; }
    }
}

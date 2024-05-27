using WhatsAppOTPAPI.DTOs;
using WhatsAppOTPAPI.Shared;

namespace WhatsAppAPI.Interfaces
{
    public interface IOTPService
    {
        Task<OTPManagerResponses> SendOTP(SendOTPDto model);
       Task<OTPManagerResponses> ValidateOTP(ValidateOTPDto model);
    }
}

using WhatsAppOTPAPI.Models;
using WhatsAppOTPAPI.DTOs;

namespace WhatsAppOTPAPI.Mapper
{
    public static class Mapper
    {
        public static OTP Map(this RegisterOTPDto model)
        {
            if (model == null)
                return null;

            return new OTP
            {
                CreatedOn = model.CreatedOn,
                ExpiresOn = model.ExpiresOn,
                Message = model.Message,
                OTPSource = model.OTPSource,
                OTPToken = model.OTPToken,
                Status = model.Status,
                ToPhoneNumber = model.ToPhoneNumber
            };
        }
    }
}

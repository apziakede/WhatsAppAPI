using Microsoft.OpenApi.Extensions;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WhatsAppAPI.Interfaces;
using WhatsAppOTPAPI.Configs;
using WhatsAppOTPAPI.Data;
using WhatsAppOTPAPI.Enums;
using WhatsAppOTPAPI.Mapper;
using WhatsAppOTPAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WhatsAppOTPAPI.Shared;

namespace WhatsAppAPI.Services
{
    public class OTPService : IOTPService
    {
        private IConfiguration _configuration;
        private readonly AppDbContext _dbContext;
        public OTPService(IConfiguration configuration, AppDbContext appDbContext)
        {
            _configuration = configuration;
            _dbContext = appDbContext;
        }

        public async Task<OTPManagerResponses> SendOTP(SendOTPDto model)
        {
            try
            {
                if (model == null)
                    throw new NullReferenceException("Send OTP model is null");

                if (String.IsNullOrEmpty(model.ToPhoneNumber))
                    return new OTPManagerResponses
                    {
                        Message = "The phone number field is required.",
                        IsSuccess = false,
                        Status = Enum.GetName(typeof(OTPStatus), OTPStatus.Validation)
                    };

                TwilioClient.Init(_configuration["AuthSettings:AccountSId"], _configuration["AuthSettings:AuthToken"]);
                string otp = GlobalConfig.GenerateOTP(model.OTPLength);

                var messageOptions = new CreateMessageOptions(new PhoneNumber($"whatsapp:{model.ToPhoneNumber}"));
                messageOptions.From = new PhoneNumber($"{_configuration["AuthSettings:FromPhoneNumber"]}");
                messageOptions.Body = $"Your {model.OTPSource} OTP is {otp}";

                var message = MessageResource.Create(messageOptions);

                var registerOTPDto = new RegisterOTPDto();

                var mapper = registerOTPDto.Map();
                mapper.OTPSource = model.OTPSource;
                mapper.ToPhoneNumber = model.ToPhoneNumber;
                mapper.ExpiresOn = DateTime.Now.GetDateUtcNow().AddMinutes(model.EllapseTimeInMinutes);
                mapper.CreatedOn = DateTime.Now.GetDateUtcNow();
                mapper.Message = messageOptions.Body;
                mapper.OTPToken = otp;
                mapper.Status = OTPStatus.Unused;

                _dbContext.OTPs.Add(mapper);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                    return new OTPManagerResponses
                    {
                        OTP = mapper.OTPToken,
                        Message = mapper.Message,
                        IsSuccess = true,
                        CreatedOn = mapper.CreatedOn,
                        ExpiresOn = mapper.ExpiresOn,
                        Status = Enum.GetName(typeof(OTPStatus), OTPStatus.Success)
                    };

                return new OTPManagerResponses
                {
                    ErrorMessage = "An error occurred while trying to save the OTP details",
                    IsSuccess = false,
                    Status = Enum.GetName(typeof(OTPStatus), OTPStatus.Validation)
                };
            }
            catch (Exception ex)
            {
                return new OTPManagerResponses
                {
                    ErrorMessage = ex.Message,
                    IsSuccess = false,
                    Status = Enum.GetName(typeof(OTPStatus), OTPStatus.Failure)
                };
            }
        }

        public async Task<OTPManagerResponses> ValidateOTP(ValidateOTPDto model)
        {
            if (model == null)
                throw new NullReferenceException("Login model is null");

            if (model.OTP.IsNullOrEmpty())
                return new OTPManagerResponses
                {
                    Message = "OTP field cannot be empty.",
                    IsSuccess = false,
                    Status = Enum.GetName(typeof(OTPStatus), OTPStatus.Validation)
                };

            var otp = await _dbContext.OTPs.FirstOrDefaultAsync(e => e.OTPToken.Equals(model.OTP) && e.ToPhoneNumber.Equals(model.ToPhoneNumber));

            if (otp == null)
                return new OTPManagerResponses
                {
                    ErrorMessage = "Invalid OTP.",
                    IsSuccess = false,
                    Status = Enum.GetName(typeof(OTPStatus), OTPStatus.Invalid)
                };

            if (otp.Status.Equals(OTPStatus.Used))
                return new OTPManagerResponses
                {
                    ErrorMessage = "This OTP has been used.",
                    IsSuccess = false,
                    Status = Enum.GetName(typeof(OTPStatus), OTPStatus.Used)
                };

            if (otp.ExpiresOn <= DateTime.Now.GetDateUtcNow())
                return new OTPManagerResponses
                {
                    ErrorMessage = "This OTP has expired.",
                    IsSuccess = false,
                    Status = Enum.GetName(typeof(OTPStatus), OTPStatus.Expired)
                };


            otp.Status = OTPStatus.Used;
            _dbContext.OTPs.Update(otp);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
                return new OTPManagerResponses
                {
                    Status = Enum.GetName(typeof(OTPStatus), OTPStatus.Valid),
                    IsSuccess = true,
                    Message = "Validation completed successfully."
                };

            return new OTPManagerResponses
            {
                ErrorMessage = "Validation failed.",
                IsSuccess = false,
                Status = Enum.GetName(typeof(OTPStatus), OTPStatus.Failure)
            };
        }
    }
}

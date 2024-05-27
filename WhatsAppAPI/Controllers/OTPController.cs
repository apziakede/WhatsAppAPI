using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhatsAppAPI.Interfaces;
using WhatsAppOTPAPI.DTOs;

namespace WhatsAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        private readonly IOTPService _otpService;

        public OTPController(IOTPService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost("SendOTP")]
        public async Task<IActionResult> SendOTP(SendOTPDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _otpService.SendOTP(entity);
                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Kindly provide all the required fields");
        }

        [HttpPost("ValidateOTP")]
        public async Task<IActionResult> ValidateOTP(ValidateOTPDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _otpService.ValidateOTP(entity);
                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Kindly provide all the required fields");
        }
    }
}

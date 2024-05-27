using Microsoft.EntityFrameworkCore;
using WhatsAppOTPAPI.Models;

namespace WhatsAppOTPAPI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<OTP> OTPs { get; set; }
    }
}

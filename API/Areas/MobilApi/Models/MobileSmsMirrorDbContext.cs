using API.Areas.MobilApi.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace API.Areas.MobilApi.Models
{
    /// <summary>Sadece MobileSms tablosu — ConnectionStrings:SmsConnection veritabanı.</summary>
    public class MobileSmsMirrorDbContext : DbContext
    {
        public MobileSmsMirrorDbContext(DbContextOptions<MobileSmsMirrorDbContext> options)
            : base(options)
        {
        }

        public DbSet<MobileSms> MobileSms { get; set; }
    }
}

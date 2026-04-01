using API.Areas.MobilApi.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace API.Areas.MobilApi.Models
{
    /// <summary>
    /// İkinci veritabanına sadece MobileSms yazmak için ayrı bağlantı.
    /// </summary>
    public class MobileSmsMirrorDbContext : DbContext
    {
        public MobileSmsMirrorDbContext(DbContextOptions<MobileSmsMirrorDbContext> options)
            : base(options)
        {
        }

        public DbSet<MobileSms> MobileSms { get; set; }
    }
}

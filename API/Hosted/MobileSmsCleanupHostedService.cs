using System;
using System.Threading;
using System.Threading.Tasks;
using API.Areas.MobilApi.Helper;
using API.Areas.MobilApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SigortaDefterimV2API.Hosted
{
    /// <summary>
    /// MobileSms kayıtlarını SmsTarihi (TR saati) üzerinden siler.
    /// MobileSms:DeleteRecordsOlderThanMinutes &lt;= 0 ise silme yapılmaz (kayıtlar kalır).
    /// </summary>
    public class MobileSmsCleanupHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(30);

        public MobileSmsCleanupHostedService(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var olderThanMinutes = _configuration.GetValue<int?>("MobileSms:DeleteRecordsOlderThanMinutes");
                    if (olderThanMinutes.HasValue && olderThanMinutes.Value > 0)
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<MobileSmsMirrorDbContext>();
                            var threshold = Utils.getTRDateTime().Subtract(TimeSpan.FromMinutes(olderThanMinutes.Value));
                            await db.Database.ExecuteSqlRawAsync(
                                "DELETE FROM MobileSms WHERE SmsTarihi < {0}",
                                threshold);
                        }
                    }
                }
                catch
                {
                    // Sessiz: tablo/sütun yoksa veya geçici DB hatası uygulamayı düşürmemeli
                }

                try
                {
                    await Task.Delay(CheckInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}

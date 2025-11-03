using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using WaruSmart.API.Resources.Domain.Repositories;

namespace WaruSmart.API.Resources.Infrastructure.Persistence.EFC
{
    public class FogSyncBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        public FogSyncBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var fogRepo = scope.ServiceProvider.GetRequiredService<IIoTDataRepository>();
                    try
                    {
                        Console.WriteLine($"[{DateTime.Now}] Fog sync START");
                        await fogRepo.FogAsync();
                        Console.WriteLine($"[{DateTime.Now}] Fog sync OK");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[{DateTime.Now}] Fog sync ERROR: {ex.Message}");
                    }
                }
                await Task.Delay(_interval, stoppingToken); // Espera solo después de terminar la ejecución
            }
        }
    }
}

// No changes needed in this file. El servicio FogSyncBackgroundService ya está correctamente implementado.
// Asegúrate de que esté registrado en Program.cs con:
// builder.Services.AddHostedService<WaruSmart.API.Resources.Infrastructure.Persistence.EFC.FogSyncBackgroundService>();

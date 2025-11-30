using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BandLab.Persistence.Sqlite;

internal class DBSetup(IServiceProvider services) : IHostedService
{
    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<DB>()
            .Database
            .MigrateAsync(CancellationToken.None);
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken) =>
        Task.CompletedTask;
}

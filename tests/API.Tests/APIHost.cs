using BandLab.API;
using BandLab.Scaling.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace API.Tests;

public class APIHost(IScaleTasks? scaleTasks = null) : WebApplicationFactory<App>
{
    protected override IHost CreateHost(IHostBuilder setup)
    {
        setup.ConfigureHostConfiguration(config =>
        {
            var data = new Dictionary<string, string?>
            {
                ["ConnectionStrings:queues"] = "amqp://localhost:5672"
            };

            config.AddInMemoryCollection(data);
        });

        return base.CreateHost(setup);
    }

    protected override void ConfigureWebHost(IWebHostBuilder setup)
    {
        setup.UseEnvironment("Development");

        setup.ConfigureServices((context, services) =>
        {
            if (scaleTasks is not null)
                services.Replace(ServiceDescriptor.Singleton(scaleTasks));
        });
    }
}

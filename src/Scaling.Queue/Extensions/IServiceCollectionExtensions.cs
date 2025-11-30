using BandLab.Scaling.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace BandLab.Scaling.Queue;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddScaleTasks(this IServiceCollection services, IConfigurationSection section,
        Action<ScalingQueueOptions>? configure = null)
    {
        services.Configure<ScalingQueueOptions>(section)
            .AddSingleton<IScaleTasks>(sp => sp.GetRequiredService<ScaleTasks>());
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, ScaleTasks>(sp => sp.GetRequiredService<ScaleTasks>()));
        services.TryAddSingleton<ScaleTasks>();

        if (configure is not null)
            services.PostConfigure(configure);

        return services;
    }

    public static IServiceCollection AddScaleProcessing<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IServiceCollection services, IConfigurationSection section,
        Action<ScalingQueueOptions>? configure = null)
        where T : class, IScaleProcessing
    {
        services.Configure<ScalingQueueOptions>(section)
            .TryAddSingleton<IScaleProcessing, T>();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, ScaleProcessing>(sp => sp.GetRequiredService<ScaleProcessing>()));
        services.TryAddSingleton<ScaleProcessing>();

        if (configure is not null)
            services.PostConfigure(configure);

        return services;
    }

    public static IServiceCollection AddScaleEvents<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IServiceCollection services, IConfigurationSection section,
        Action<ScalingQueueOptions>? configure = null)
        where T : class, IScaleEvents
    {
        services.Configure<ScalingQueueOptions>(section)
            .TryAddSingleton<IScaleEvents, T>();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, ScaleEvents>(sp => sp.GetRequiredService<ScaleEvents>()));
        services.TryAddSingleton<ScaleEvents>();

        if (configure is not null)
            services.PostConfigure(configure);

        return services;
    }
}

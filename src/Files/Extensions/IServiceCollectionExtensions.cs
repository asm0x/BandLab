using BandLab.Files.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BandLab.Files;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddFileStorage(this IServiceCollection services, Func<IServiceProvider, string> directory)
    {
        services.TryAddSingleton<IFileStorage>(sp => new FileStorage(directory(sp)));

        return services;
    }

    public static IServiceCollection AddFileStorage(this IServiceCollection services)
    {
        services.TryAddSingleton<IFileStorage, FileStorage>();

        return services;
    }
}

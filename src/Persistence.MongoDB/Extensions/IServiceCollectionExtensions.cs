using BandLab.Persistence.MongoDB.Repositories;
using BandLab.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BandLab.Persistence.MongoDB;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDBRepositories(this IServiceCollection services, IConfigurationSection configuration,
        Action<MongoDBOptions>? configure = null)
    {
        services.TryAddSingleton<IMongoClientFactory, MongoClientFactory>();

        services.TryAddSingleton<IAccountsRepository, AccountsRepository>();
        services.TryAddSingleton<IPostsRepository, PostsRepository>();
        services.TryAddSingleton<ICommentsRepository, CommentsRepository>();

        services.AddHostedService<DBSetup>();

        services.Configure<MongoDBOptions>(configuration);

        if (configure is not null)
            services.PostConfigure(configure);

        return services;
    }
}

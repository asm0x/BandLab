using BandLab.Persistence.Sqlite.Repositories;
using BandLab.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BandLab.Persistence.Sqlite;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddSqliteRepositories(this IServiceCollection services, IConfigurationSection configuration)
    {
        services.AddDbContext<DB>(options =>
        {
            options
                // .UseModel(DBModel.Instance)
                .UseSqlite($"Data Source={Path.Combine(Path.GetDirectoryName(typeof(DB).Assembly.Location) ?? ".", "bandlab.db")}");
            // .UseSqlite(configuration["Connection"]);
        });

        services.TryAddScoped<IAccountsRepository, AccountsRepository>();
        services.TryAddScoped<IPostsRepository, PostsRepository>();
        services.TryAddScoped<ICommentsRepository, CommentsRepository>();

        services.AddHostedService<DBSetup>();

        return services;
    }
}

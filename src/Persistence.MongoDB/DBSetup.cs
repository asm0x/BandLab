using BandLab.Entities;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace BandLab.Persistence.MongoDB;

internal class DBSetup(IMongoClientFactory clientFactory) : Repository(clientFactory), IHostedService
{
    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        await Posts.Indexes.CreateOneAsync(new CreateIndexModel<Post>(Builders<Post>.IndexKeys.Descending(x => x.Comments),
            new CreateIndexOptions
            {
                Name = "Comments",
                Background = true
            }),
            null,
            CancellationToken.None);

        await Posts.Indexes.CreateOneAsync(new CreateIndexModel<Post>(Builders<Post>.IndexKeys.Ascending(x => x.Creator),
            new CreateIndexOptions
            {
                Name = "Creator",
                Background = true
            }),
            null,
            CancellationToken.None);

        await Comments.Indexes.CreateOneAsync(new CreateIndexModel<Comment>(Builders<Comment>.IndexKeys.Ascending(x => x.Creator),
            new CreateIndexOptions
            {
                Name = "Creator",
                Background = true
            }),
            null,
            CancellationToken.None);
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken) =>
        Task.CompletedTask;
}

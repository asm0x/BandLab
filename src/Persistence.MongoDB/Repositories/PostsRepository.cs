using BandLab.Entities;
using BandLab.Repositories;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace BandLab.Persistence.MongoDB.Repositories;

internal class PostsRepository : Repository, IPostsRepository
{
    public PostsRepository(IMongoClientFactory clientFactory) : base(clientFactory)
    {
        BsonClassMap.TryRegisterClassMap<Post>(entity =>
        {
            entity.AutoMap();
            entity.SetIgnoreExtraElements(true);
            entity.MapIdMember(c => c.Id);
        });
    }


    public async Task<(IEnumerable<T> data, long total)> Get<T>(int page, int size, Func<Post, T> projection) =>
        ((await (await Posts.FindAsync(Builders<Post>.Filter.Empty,
            new FindOptions<Post>
            {
                Sort = Builders<Post>.Sort.Descending(x => x.Comments),
                Skip = page * Math.Min(10, size),
                Limit = Math.Min(10, size)
            }))
            .ToListAsync())
            .Select(projection),
        await Posts.CountDocumentsAsync(Builders<Post>.Filter.Empty));

    public async Task<Post?> GetById(Guid id) =>
        await (await Posts.FindAsync(Builders<Post>.Filter.Eq(x => x.Id, id),
            new FindOptions<Post>
            {
                Limit = 1
            }))
        .FirstOrDefaultAsync();

    public async Task<Post> CreateAsync(Post post)
    {
        await Posts.InsertOneAsync(post);

        return post;
    }
}

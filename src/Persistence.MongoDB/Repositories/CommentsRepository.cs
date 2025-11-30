using BandLab.Entities;
using BandLab.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace BandLab.Persistence.MongoDB.Repositories;

internal class CommentsRepository : Repository, ICommentsRepository
{
    private readonly ILogger<CommentsRepository> log;

    public CommentsRepository(IMongoClientFactory clientFactory, ILogger<CommentsRepository> log) : base(clientFactory)
    {
        BsonClassMap.TryRegisterClassMap<Comment>(entity =>
        {
            entity.AutoMap();
            entity.SetIgnoreExtraElements(true);
            entity.MapIdMember(c => c.Id);
        });

        this.log = log;
    }


    public async Task<Comment?> GetById(Guid id) =>
        await (await Comments.FindAsync(Builders<Comment>.Filter.Eq(x => x.Id, id),
            new FindOptions<Comment>
            {
                Limit = 1
            }))
        .FirstOrDefaultAsync();

    public async Task<Comment> CreateAsync(Comment comment)
    {
        await Comments.InsertOneAsync(comment);
        AddComment(comment);

        return comment;
    }

    private async void AddComment(Comment comment)
    {
        try
        {
            await Posts.UpdateOneAsync(Builders<Post>.Filter.Eq(x => x.Id, comment.Post),
                Builders<Post>.Update
                    .PushEach(x => x.LastComments, [comment], -2)
                    .Inc(x => x.Comments, 1));
        }
        catch (Exception e)
        {
            log.LogError(e, "Failed to cache last comments: {failure}", e.Message);
        }
    }
}

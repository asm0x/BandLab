using BandLab.Entities;
using BandLab.Repositories;
using Microsoft.Extensions.Logging;

namespace BandLab.Persistence.Sqlite.Repositories;

public class CommentsRepository(DB context, ILogger<CommentsRepository> log) : ICommentsRepository
{
    public async Task<Comment?> GetById(Guid id)
    {
        return await context.Comments.FindAsync(id);
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        context.Comments.Add(comment);

        await context.SaveChangesAsync();

        await AddComment(comment);

        return comment;
    }

    private async Task AddComment(Comment comment)
    {
        await Task.Delay(TimeSpan.FromSeconds(5));
        try
        {
            var post = await context.Posts.FindAsync(comment.Post);
            if (post is not null)
            {
                post.Comments++;
                post.LastComments = [.. post.LastComments.Take(1).Append(comment)];

                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            log.LogError(e, "Failed to cache last comments: {failure}", e.Message);
        }
    }
}

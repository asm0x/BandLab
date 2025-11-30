using BandLab.Entities;
using BandLab.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BandLab.Persistence.Sqlite.Repositories;

public class PostsRepository(DB context) : IPostsRepository
{
    public async Task<(IEnumerable<T> data, long total)> Get<T>(int page, int size, Func<Post, T> projection)
    {
        return ((await context.Posts
            .OrderByDescending(x => x.Comments)
            .Skip(page * Math.Min(10, size))
            .Take(Math.Min(10, size))
            .ToListAsync())
            .Select(projection),
            await context.Posts.LongCountAsync());
    }

    public async Task<Post?> GetById(Guid id)
    {
        return await context.Posts.FindAsync(id);
    }

    public async Task<Post> CreateAsync(Post post)
    {
        context.Posts.Add(post);

        await context.SaveChangesAsync();

        return post;
    }
}

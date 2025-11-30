using BandLab.API.Models;
using BandLab.Repositories;

namespace BandLab.API;

public static class Posts
{
    public static WebApplication UsePosts(this WebApplication app)
    {
        app.MapGet("/posts",
            async (HttpContext context, IPostsRepository posts, ILogger<App> log,
                int page = 0, int size = 10) =>
            {
                return new Page<PostModel>(context.Site(),
                    await posts.Get(page, size,
                        entity => new PostModel(entity.Id,
                            entity.Caption,
                            context.CDN(entity.Image),
                            entity.Creator.ToString(),
                            entity.CreatedAt,
                            entity.Comments,
                            entity.LastComments.Length > 0
                                ? entity.LastComments
                                : null,
                            context.Site())),
                    page,
                    size);
            })
            .RequireAuthorization();

        return app;
    }
}

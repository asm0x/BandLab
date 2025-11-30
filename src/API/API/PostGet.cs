using BandLab.API.Models;
using BandLab.Repositories;

namespace BandLab.API;

public static class PostGet
{
    public static WebApplication UsePostGet(this WebApplication app)
    {
        app.MapGet("/posts/{postId}",
            async (HttpContext context, IPostsRepository posts, ILogger<App> log,
                Guid postId) =>
            {
                var entity = await posts.GetById(postId);
                if (entity is null)
                    return Results.NotFound();

                return Results.Ok(new PostModel(entity.Id,
                    entity.Caption,
                    context.CDN(entity.Image),
                    entity.Creator.ToString(),
                    entity.CreatedAt,
                    entity.Comments,
                    entity.LastComments.Length > 0
                        ? entity.LastComments
                        : null,
                    context.Site()));
            })
            .RequireAuthorization();

        return app;
    }
}

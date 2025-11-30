using BandLab.API.Models;
using BandLab.Repositories;

namespace BandLab.API;

public static class CommentGet
{
    public static WebApplication UseCommentGet(this WebApplication app)
    {
        app.MapGet("/posts/{postId}/comments/{commentId}",
            async (HttpContext context, ICommentsRepository comments, ILogger<App> log,
                Guid postId, Guid commentId) =>
            {
                var entity = await comments.GetById(commentId);
                if (entity is null)
                    return Results.NotFound();

                return Results.Ok(new CommentModel(entity.Id,
                    entity.Post,
                    entity.Content,
                    entity.Creator.ToString(),
                    entity.CreatedAt));
            })
            .RequireAuthorization();

        return app;
    }
}

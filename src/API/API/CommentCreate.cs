using BandLab.API.Models;
using BandLab.Entities;
using BandLab.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BandLab.API;

public static class CommentCreate
{
    public static WebApplication UseCommentCreate(this WebApplication app)
    {
        // Generation of id is server's responsibility.
        app.MapPost("/posts/{postId}/comments",
            async (HttpContext context, IPostsRepository posts, ICommentsRepository comments, ILogger<App> log,
                Guid postId, [FromBody] CreateCommentModel data) =>
            {
                Validate(data);

                return await Create(context, posts, comments, postId,
                    new Comment
                    {
                        Post = postId,
                        Content = data.Content,
                        Creator = context.UserId()
                    });
            })
            .RequireAuthorization();

        // Generation of id is client's responsibility.
        // This is idempotent.
        app.MapPut("/posts/{postId}/comments/{commentId}",
            async (HttpContext context, IPostsRepository posts, ICommentsRepository comments, ILogger<App> log,
                Guid postId, Guid commentId, [FromBody] CreateCommentModel data) =>
            {
                Validate(data);

                if (await comments.GetById(commentId) is not null)
                    return Results.NoContent();

                return await Create(context, posts, comments, postId,
                    new Comment
                    {
                        Id = commentId,
                        Post = postId,
                        Content = data.Content,
                        Creator = context.UserId()
                    });
            })
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> Create(HttpContext context, IPostsRepository posts, ICommentsRepository comments,
        Guid postId, Comment comment)
    {
        var post = await posts.GetById(postId);
        if (post is null)
            return Results.NotFound();

        var entity = await comments.CreateAsync(comment);

        return Results.Created(context.Link($"/posts/{postId}/comments/{entity.Id}"),
            new CommentModel(entity.Id,
                entity.Post,
                entity.Content,
                entity.Creator.ToString(),
                entity.CreatedAt));
    }

    private static void Validate(object data)
    {
        var results = new List<ValidationResult>();
        var validation = new ValidationContext(data);

        if (!Validator.TryValidateObject(data, validation, results, true))
        {
            throw new ValidationException(string.Join("\n", results.Select(x => x.ErrorMessage)));
        }
    }
}

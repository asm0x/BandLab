using BandLab.API.Models;
using BandLab.Entities;
using BandLab.Files.Interfaces;
using BandLab.Repositories;
using BandLab.Scaling.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BandLab.API;

public static class PostCreate
{
    private const int sizeLimit = 104_857_600;

    public static WebApplication UsePostCreate(this WebApplication app)
    {
        // Generation of id is server's responsibility.
        app.MapPost("/posts",
            [RequestSizeLimit(sizeLimit)]
            async (HttpContext context, IPostsRepository posts, IFileStorage storage, IScaleTasks scaling, ILogger<App> log,
                CreatePostModel data) =>
            {
                return await Create(context, posts, storage, scaling, data.Image,
                    new Post
                    {
                        Caption = data.Caption,
                        Image = data.Image is not null
                            ? $"{Guid.NewGuid()}{Path.GetExtension(data.Image.FileName)}"
                            : null,
                        Creator = context.UserId()
                    });
            })
            .RequireAuthorization();

        // Generation of id is client's responsibility.
        // This is idempotent.
        app.MapPut("/posts/{postId}",
            [RequestSizeLimit(sizeLimit)]
            async (HttpContext context, IPostsRepository posts, IFileStorage storage, IScaleTasks scaling, ILogger<App> log,
                Guid postId, CreatePostModel data) =>
            {
                if (await posts.GetById(postId) is not null)
                    return Results.NoContent();

                return await Create(context, posts, storage, scaling, data.Image,
                    new Post
                    {
                        Id = postId,
                        Caption = data.Caption,
                        Image = data.Image is not null
                            ? $"{Guid.NewGuid()}{Path.GetExtension(data.Image.FileName)}"
                            : null,
                        Creator = context.UserId()
                    });
            })
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> Create(HttpContext context, IPostsRepository posts, IFileStorage storage, IScaleTasks scaling, IFormFile? image,
        Post post)
    {
        post = await posts.CreateAsync(post);

        if (image is not null &&
            post.Image is not null)
        {
            await scaling.RunScale(post.Image,
                await storage.SaveAsync(post.Image, image.OpenReadStream()));
        }

        return Results.Created(context.Link($"/posts/{post.Id}"),
            new PostModel(post.Id,
                post.Caption,
                context.CDN(post.Image),
                post.Creator.ToString(),
                post.CreatedAt,
                post.Comments,
                post.LastComments.Length > 0
                    ? post.LastComments
                    : null,
                context.Site()));
    }
}

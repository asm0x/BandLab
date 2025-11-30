using API.Tests;
using BandLab.Clients;
using BandLab.Scaling.Interfaces;
using Moq;
using System.Net;
using System.Net.Http.Headers;

namespace BandLab.API.Tests;

public class Tests
{
    [Fact]
    public async Task PostAndCommentCreateTest()
    {
        var test = Client();
        var postId = await test.CreatePost("Caption", Image("01.jpg"));
        Assert.NotNull(postId);

        await test.CreateComment(postId.Value, "Comment");
    }

    [Fact]
    public async Task PostAndCommentWithIdsCreateTest()
    {
        var test = Client();
        var id = Guid.NewGuid();
        var postId = await test.CreatePost("Caption", Image("01.jpg"), id);
        Assert.Equal(id, postId);

        await test.CreateComment(postId.Value, "Comment", Guid.NewGuid());
    }

    [Fact]
    public async Task ImageProcessingTest()
    {
        var sync = new ManualResetEventSlim();
        var scaleTasks = new Mock<IScaleTasks>();
        scaleTasks.Setup(x => x.RunScale(It.IsAny<string>(), It.IsAny<string>()))
            .Callback(() => sync.Set())
            .Returns(() => default);

        var test = Client(scaleTasks.Object);
        var postId = await test.CreatePost("Caption", Image("01.jpg"));
        Assert.True(sync.Wait(TimeSpan.FromSeconds(1)));
    }

    [Fact]
    public async Task BadRequestTest()
    {
        var test = Client();
        var content = new MultipartFormDataContent();
        using var request = new HttpRequestMessage(HttpMethod.Post, "/posts")
        {
            Content = content
        };

        var response = await test.SendAsync(request);

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    private static Client Client(IScaleTasks? scaleTasks = null)
    {
        var api = new APIHost(scaleTasks);
        var token = api.CreateToken();
        var client = api.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return new Client(client);
    }

    private static string Image(string file) =>
        Path.Combine(AppContext.BaseDirectory, @"..\..\..\Images\" + file);
}

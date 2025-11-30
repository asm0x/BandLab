using BandLab.Infrastructure;
using System.Net.Http.Headers;

namespace BandLab.Clients;

public class Test(IConfiguration configuration, IHttpClientFactory clients, ILogger<Test> log) : IHostedService
{
    private const int users = 100;
    private const int tests = 100;

    private readonly CancellationTokenSource shutdown = new();
    private readonly string key = configuration["Key"]
        ?? throw new InvalidOperationException("Unable to get secret key");

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        for (var uid = 0; uid < users; uid++)
        {
            await Task.Delay(30, cancellationToken);
            _ = Client(uid);
        }
    }

    private async Task Client(int uid)
    {
        var client = clients.CreateClient();
        client.BaseAddress = new Uri("http://api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.Create(key));

        await Client(client);
    }

    private async Task Client(HttpClient client)
    {
        var test = new Client(client);

        for (var i = 0;
            i < tests && !shutdown.IsCancellationRequested;
            i++)
            try
            {
                var postId = await test.CreatePost("Test caption", null);
                if (postId is null)
                    throw new InvalidOperationException("Can't create post");
                await Delay();

                await test.CreateComment(postId.Value, "Test comment");
                await Delay();

                await test.GetPosts();
                await Delay();
            }
            catch (Exception e)
            {
                log.LogError(e, "Failed to call api: {failure}", e.Message);
            }
    }

    private Task Delay() =>
        // Task.Delay(Random.Shared.Next(100), shutdown.Token);
        Task.Delay(1000, shutdown.Token);

    public Task StopAsync(CancellationToken cancellationToken) =>
        shutdown.CancelAsync();
}

namespace BandLab.Clients;

public class Client(HttpClient client)
{
    public async Task<Guid?> CreatePost(string caption, string? image, Guid? id = null)
    {
        var content = new MultipartFormDataContent
        {
            { new StringContent(caption), "Caption" },
        };

        if (image is not null)
            content.Add(new ByteArrayContent(File.ReadAllBytes(image)), "Image", Path.GetFileName(image));

        using var request = id is not null
            ? new HttpRequestMessage(HttpMethod.Put, $"/posts/{id}")
            : new HttpRequestMessage(HttpMethod.Post, "/posts");

        request.Content = content;

        var response = (await client.SendAsync(request))
            .EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync(Models.Default.PostCreated);

        return result?.Id;
    }

    public async Task CreateComment(Guid postId, string content, Guid? id = null)
    {
        (id is not null
            ? await client.PutAsJsonAsync($"/posts/{postId}/comments/{id}", new CommentCreate(content), Models.Default.CommentCreate)
            : await client.PostAsJsonAsync($"/posts/{postId}/comments", new CommentCreate(content), Models.Default.CommentCreate))
            .EnsureSuccessStatusCode();
    }

    public async Task GetPosts()
    {
        (await client.GetAsync("/posts"))
            .EnsureSuccessStatusCode();
    }


    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) =>
        client.SendAsync(request);
}

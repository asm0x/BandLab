using BandLab.API;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace BandLab.API;

public class Page<T>(Uri? address, (IEnumerable<T> data, long total) result, int page, int size, HttpStatusCode code = HttpStatusCode.OK) : IResult
{
    public async Task ExecuteAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)code;

        var pages = Math.Ceiling((double)result.total / size);
        var args = new Dictionary<string, string?>
        {
            { "size", size.ToString() }
        };

        var links = new List<string>
        {
            $"<{address.WithArg(args, "page", "0")}>; rel=\"first\"",
            $"<{address.WithArg(args, "page", page.ToString())}>; rel=\"self\"",
            $"<{address.WithArg(args, "page", pages.ToString())}>; rel=\"last\"",
        };

        if (page > 0)
            links.Add($"<{address.WithArg(args, "page", (page - 1).ToString())}>; rel=\"prev\"");
        if (page < pages)
            links.Add($"<{address.WithArg(args, "page", (page + 1).ToString())}>; rel=\"next\"");

        context.Response.Headers.Link = links.ToArray();

        context.Response.Headers["X-Total-Items"] = result.total.ToString();
        context.Response.Headers["X-Page"] = page.ToString();
        context.Response.Headers["X-Page-Size"] = size.ToString();
        context.Response.Headers["X-Total-Pages"] = pages.ToString();

        context.Response.ContentType = "application/json";
        var jso = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
        var json = JsonSerializer.Serialize(result.data, jso);
        await context.Response.WriteAsync(json);
    }
}

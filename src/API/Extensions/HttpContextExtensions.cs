namespace BandLab.API;

public static class HttpContextExtensions
{
    private static Uri? cdn;

    public static Uri? Link(this HttpContext context, string? path) => path is not null
        ? new Uri(context.Site(), path)
        : null;

    public static Uri Site(this HttpContext context) =>
        new($"{context.Request.Scheme}://{context.Request.Host}");

    public static string? CDN(this HttpContext context, string? path) => path is not null
        ? new Uri(cdn ??= new Uri(context.RequestServices.GetRequiredService<IConfiguration>()["services:cdn:http:0"] ?? "http://cdn"), path).ToString()
        : null;

    public static Guid UserId(this HttpContext context) =>
        context.User.UserId();
}

using Microsoft.AspNetCore.WebUtilities;

namespace BandLab.API;

public static class UriExtensions
{
    public static string WithArg(this Uri? link, Dictionary<string, string?> args, string arg, string value)
    {
        args[arg] = value;

        return new Uri(QueryHelpers.AddQueryString(link?.ToString() ?? "/", args)).ToString();
    }
}

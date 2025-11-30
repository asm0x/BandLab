using API.Tests;
using BandLab.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BandLab.API.Tests;

internal static class APIHostExtensions
{
    public static string CreateToken(this APIHost host, Guid? sub = null) =>
        Token.Create(host.Services.GetRequiredService<IConfiguration>()["Key"]
            ?? throw new InvalidOperationException("Unable to get secret key"),
        sub);
}

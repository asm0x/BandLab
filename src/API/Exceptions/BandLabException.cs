using System.Net;

namespace BandLab.API;

public abstract class BandLabException(HttpStatusCode code, string? description) : Exception(description)
{
    public HttpStatusCode Code { get; } = code;
}

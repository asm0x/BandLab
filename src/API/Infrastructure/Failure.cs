using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BandLab.API;

/// <summary>
/// An <see cref="IResult"/> that on execution will write Problem Details HTTP API responses based on <see href="https://tools.ietf.org/html/rfc7807"/>
/// </summary>
public class Failure : IResult, IStatusCodeHttpResult, IContentTypeHttpResult, IValueHttpResult, IValueHttpResult<ProblemDetails>
{
    string IContentTypeHttpResult.ContentType => "application/problem+json";
    private const string type = @"http://bandlab/api/probs/bad-request";

    private readonly HttpStatusCode status;
    private readonly ProblemDetails result;

    ProblemDetails? IValueHttpResult<ProblemDetails>.Value => result;
    int? IStatusCodeHttpResult.StatusCode => (int)status;
    object? IValueHttpResult.Value => result;


    public Failure(PathString instance, HttpStatusCode status, string description)
    {
        this.status = status;

        result = new ProblemDetails
        {
            Type = type,
            Instance = instance,
            Title = description,
            Status = (int)status
        };
    }

    /// <inheritdoc/>
    public async Task ExecuteAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)status;

        await context.RequestServices.GetRequiredService<IProblemDetailsService>()
            .WriteAsync(new()
            {
                HttpContext = context,
                ProblemDetails = result
            });
    }
}

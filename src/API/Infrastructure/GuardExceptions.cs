using Microsoft.AspNetCore.Diagnostics;

namespace BandLab.API;

public class GuardExceptions : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        exception = exception switch
        {
            InvalidDataException => new ValidationException(),
            _ => exception
        };

        if (exception is BandLabException e)
        {
            context.Response.StatusCode = (int)e.Code;

            await new Failure(context.Request.Path, e.Code, e.Message)
                .ExecuteAsync(context);

            return true;
        }
        else
            return false;
    }
}

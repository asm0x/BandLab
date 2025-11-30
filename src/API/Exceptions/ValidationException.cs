using System.Net;

namespace BandLab.API;

public class ValidationException(string? description = null)
    : BandLabException(HttpStatusCode.UnprocessableEntity, description)
{
}

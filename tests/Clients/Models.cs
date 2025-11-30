using System.Text.Json.Serialization;

namespace BandLab.Clients;

internal record PostCreated(Guid? Id);

internal record CommentCreate(string Content);

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(PostCreated))]
[JsonSerializable(typeof(CommentCreate))]
internal partial class Models : JsonSerializerContext
{
}
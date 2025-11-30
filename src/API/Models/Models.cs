using BandLab.API.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BandLab.API;

[JsonSerializable(typeof(CreatePostModel))]
[JsonSerializable(typeof(CreateCommentModel))]
[JsonSerializable(typeof(CommentModel))]
[JsonSerializable(typeof(PostModel))]
[JsonSerializable(typeof(IEnumerable<PostModel>))]
[JsonSerializable(typeof(List<ValidationResult>))]
internal partial class ModelsSerialization : JsonSerializerContext
{
}

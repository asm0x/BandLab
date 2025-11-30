using BandLab.Scaling.Interfaces;
using System.Text.Json.Serialization;

namespace BandLab.Scaling.Queue;

[JsonSerializable(typeof(Scale))]
internal partial class ScaleModelsSerialization : JsonSerializerContext
{
}

using BandLab.Entities;
using System.Text.Json.Serialization;

namespace BandLab.Persistence.Sqlite;

[JsonSerializable(typeof(Comment[]))]
internal partial class EntitiesSerialization : JsonSerializerContext
{
}

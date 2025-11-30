using BandLab.Entities;
using System.Text.Json.Serialization;

namespace BandLab.API.Models;

public record PostModel(Guid Id, string? Caption, string? Image, string Creator, DateTime CreatedAt, int Comments, Comment[]? LastComments, [property: JsonIgnore] Uri Site)
{
    public PostLinks Links => new(Site, Id);
}

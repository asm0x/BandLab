namespace BandLab.API.Models;

public record CommentModel(Guid Id, Guid Post, string Content, string Creator, DateTime CreatedAt);

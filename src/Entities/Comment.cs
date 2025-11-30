namespace BandLab.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public required Guid Post { get; set; }
    public required string Content { get; set; }
    public required Guid Creator { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

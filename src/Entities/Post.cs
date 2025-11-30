namespace BandLab.Entities;

public class Post
{
    public Guid Id { get; set; }

    public string? Caption { get; set; }

    /// <summary>
    /// Link to uploaded image in CDN.
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// Stored number of comments for the post.
    /// </summary>
    public int Comments { get; set; }

    public required Guid Creator { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Cached last comments to improve read refrormance.
    /// </summary>
    public Comment[] LastComments { get; set; } = [];
}

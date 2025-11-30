namespace BandLab.API.Models;

public class PostLinks(Uri site, Guid id)
{
    public string Comment { get; set; } = $"{site}posts/{id}/comments";
}

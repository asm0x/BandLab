using BandLab.Entities;

namespace BandLab.Repositories;

public interface IPostsRepository
{
    Task<(IEnumerable<T> data, long total)> Get<T>(int page, int size, Func<Post, T> projection);
    Task<Post?> GetById(Guid id);
    Task<Post> CreateAsync(Post post);
}

using BandLab.Entities;

namespace BandLab.Repositories;

public interface ICommentsRepository
{
    Task<Comment?> GetById(Guid id);
    Task<Comment> CreateAsync(Comment comment);
}

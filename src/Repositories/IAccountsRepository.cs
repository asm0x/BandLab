using BandLab.Entities;

namespace BandLab.Repositories;

public interface IAccountsRepository
{
    Task<Account> GetById(Guid id);
    Task DeleteAsync(Account account);
}

using BandLab.Entities;
using BandLab.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BandLab.Persistence.Sqlite;

public class AccountsRepository(DB context) : IAccountsRepository
{
    public Task<Account> GetById(Guid id) =>
        Task.FromResult(new Account
        {
            Id = id,
            Name = id.ToString()
        });

    public async Task DeleteAsync(Account account)
    {
        await context.Comments
            .Where(x => x.Creator == account.Id)
            .ExecuteDeleteAsync();

        await context.Posts
            .Where(x => x.Creator == account.Id)
            .ExecuteDeleteAsync();
    }
}

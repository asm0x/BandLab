using BandLab.Entities;
using BandLab.Repositories;
using MongoDB.Driver;

namespace BandLab.Persistence.MongoDB;

internal class AccountsRepository(IMongoClientFactory clientFactory) : Repository(clientFactory), IAccountsRepository
{
    public Task<Account> GetById(Guid id) =>
        Task.FromResult(new Account
        {
            Id = id,
            Name = id.ToString()
        });

    public async Task DeleteAsync(Account account)
    {
        await Comments.DeleteManyAsync(Builders<Comment>.Filter.Eq(x => x.Creator, account.Id));
        await Posts.DeleteManyAsync(Builders<Post>.Filter.Eq(x => x.Creator, account.Id));
    }
}

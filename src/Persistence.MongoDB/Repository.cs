using BandLab.Entities;
using MongoDB.Driver;

namespace BandLab.Persistence.MongoDB;

internal abstract class Repository
{
    protected IMongoCollection<Account> Accounts;
    protected IMongoCollection<Post> Posts;
    protected IMongoCollection<Comment> Comments;

    public Repository(IMongoClientFactory clientFactory)
    {
        Accounts = clientFactory.Collection<Account>("Accounts");
        Posts = clientFactory.Collection<Post>("Posts");
        Comments = clientFactory.Collection<Comment>("Comments");
    }
}

using MongoDB.Driver;

namespace BandLab.Persistence.MongoDB;

public interface IMongoClientFactory
{
    MongoDBOptions Options { get; }
    MongoClient Client { get; }

    IMongoCollection<T> Collection<T>(string collection);
}
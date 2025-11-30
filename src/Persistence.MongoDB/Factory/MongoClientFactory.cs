using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace BandLab.Persistence.MongoDB;

public class MongoClientFactory : IMongoClientFactory
{
    public MongoDBOptions Options { get; init; }
    public MongoClient Client { get; init; }

    public IMongoCollection<T> Collection<T>(string collection) =>
        Client.GetDatabase(Options.Database)
            .GetCollection<T>(collection)
            .WithWriteConcern(WriteConcern.W1);


    public MongoClientFactory(IOptions<MongoDBOptions> options, ILogger<MongoClientFactory> log)
    {
        var connection = MongoClientSettings.FromConnectionString(options.Value.Connection);
        if (options.Value.User is not null)
            connection.Credential = MongoCredential.CreateCredential("admin", options.Value.User, options.Value.Password);

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        Options = options.Value;
        Client = new(connection);
    }
}

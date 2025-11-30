namespace BandLab.Persistence.MongoDB;

public class MongoDBOptions
{
    public string? Connection { get; set; }
    public string Database { get; set; } = "BandLab";
    public string? User { get; set; }
    public string? Password { get; set; }
}

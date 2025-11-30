using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BandLab.Persistence.Sqlite;

public class DBFactory : IDesignTimeDbContextFactory<DB>
{
    public DB CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder<DB>()
            .UseSqlite("Data Source=bandlab.db").Options);
}

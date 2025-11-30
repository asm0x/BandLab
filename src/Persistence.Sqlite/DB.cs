using BandLab.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace BandLab.Persistence.Sqlite;

public class DB(DbContextOptions<DB> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseModel(DBModel.Instance);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(x => x.Comments).IsDescending(true);
            entity.HasIndex(x => x.Creator);
            entity.Property(e => e.LastComments)
                .HasConversion(new ValueConverter<Comment[], string>(v => JsonSerializer.Serialize(v, EntitiesSerialization.Default.CommentArray),
                    v => JsonSerializer.Deserialize(v, EntitiesSerialization.Default.CommentArray) ?? new Comment[0]));
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(x => x.Creator);
            /*
            // This breaks sqlite database (malformed)
            entity.HasOne<Post>()
                .WithMany()
                .HasForeignKey(c => c.Post)
                .OnDelete(DeleteBehavior.Cascade);
            */

        });

        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
}

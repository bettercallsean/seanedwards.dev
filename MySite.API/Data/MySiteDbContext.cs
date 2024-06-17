using Microsoft.EntityFrameworkCore;
using MySite.API.Data.Entities;

namespace MySite.API.Data;

public class MySiteDbContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<LikedTweet> LikedTweets { get; set; }
    public new DbSet<T> Set<T>() where T : class => base.Set<T>();
    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
}

using ForoUniversitario.Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.Infrastructure.Persistence;

public class ForumDbContext : DbContext
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options)
        : base(options) { }

    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PostConfiguration());
    }
}

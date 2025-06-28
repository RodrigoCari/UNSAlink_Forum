using ForoUniversitario.Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.Infrastructure.Persistence;

public class ForoDbContext : DbContext
{
    public ForoDbContext(DbContextOptions<ForoDbContext> options)
        : base(options) { }

    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PostConfiguration());
    }
}

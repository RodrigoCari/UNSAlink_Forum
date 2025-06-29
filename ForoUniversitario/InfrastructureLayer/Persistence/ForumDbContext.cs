using ForoUniversitario.DomainLayer.Notifications;
using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.DomainLayer.Groups;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.InfrastructureLayer.Persistence;

public class ForumDbContext : DbContext
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options)
        : base(options) { }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new GroupConfiguration());
    }
}

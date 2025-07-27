using ForoUniversitario.DomainLayer.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForoUniversitario.InfrastructureLayer.Persistence;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
        builder.Property(u => u.Role).IsRequired().HasConversion<string>();

        builder.HasMany(u => u.Posts)
               .WithOne(p => p.Author)
               .HasForeignKey(p => p.AuthorId)
               .OnDelete(DeleteBehavior.Cascade); // Posts se eliminan si se borra el usuario
        
        builder.Property(u => u.Interests)
               .HasConversion(
                   v => string.Join(';', v),
                   v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
               )
               .HasColumnName("Interests");
    }
}

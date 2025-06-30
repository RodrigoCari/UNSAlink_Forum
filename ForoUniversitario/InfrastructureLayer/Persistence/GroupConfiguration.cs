using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForoUniversitario.InfrastructureLayer.Persistence;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Groups");

        builder.HasKey(g => g.Id);
        builder.Property(g => g.Name).IsRequired().HasMaxLength(200);
        builder.Property(g => g.Description).IsRequired().HasMaxLength(1000);

        builder.HasOne(g => g.Admin)
               .WithMany()
               .HasForeignKey(g => g.AdminId)
               .OnDelete(DeleteBehavior.Restrict); // Un grupo tiene un admin

        builder.HasMany(g => g.Members)
               .WithMany()
               .UsingEntity(j => j.ToTable("GroupMembers")); // muchos a muchos

        builder.HasMany(g => g.Viewers)
               .WithMany()
               .UsingEntity(j => j.ToTable("GroupViewers")); // muchos a muchos

        builder.HasMany(g => g.Posts)
               .WithOne(p => p.Group)
               .HasForeignKey(p => p.GroupId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

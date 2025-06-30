using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForoUniversitario.InfrastructureLayer.Persistence;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.CreatedAt)
               .IsRequired();

        builder.Property(p => p.Type)
               .IsRequired()
               .HasConversion<string>();

        builder.Property(p => p.AuthorId)
               .IsRequired();

        builder.Property(p => p.GroupId)
               .IsRequired();

        builder.OwnsOne(p => p.Content, content =>
        {
            content.Property(c => c.Text)
                   .HasColumnName("ContentText")
                   .IsRequired()
                   .HasMaxLength(1000);
        });

        builder.HasMany(p => p.Comments)
               .WithOne()
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Group>() // navegación implícita
               .WithMany(g => g.Posts)
               .HasForeignKey(p => p.GroupId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

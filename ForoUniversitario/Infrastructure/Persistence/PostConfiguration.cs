using ForoUniversitario.Domain.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForoUniversitario.Infrastructure.Persistence;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.FechaCreacion)
               .IsRequired();

        // Value Object: ContenidoPost → se mapea como objeto embebido
        builder.OwnsOne(p => p.Contenido, contenido =>
        {
            contenido.Property(c => c.Texto)
                     .HasColumnName("ContenidoTexto")
                     .IsRequired()
                     .HasMaxLength(1000);
        });
    }
}

using ForoUniversitario.DomainLayer.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForoUniversitario.InfrastructureLayer.Persistence;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(nameof(Notification));          // tabla = “Notifications”

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Message)
               .IsRequired()
               .HasMaxLength(1_000);

        builder.Property(n => n.IsRead)
               .IsRequired()
               .HasDefaultValue(false);                 // valor por defecto persistente

        builder.Property(n => n.CreatedAt)
               .IsRequired()
               .HasColumnType("datetime2");             // precisión SQL Server

        builder.Property(n => n.ReceiverId).IsRequired();

        builder.Property(n => n.NotificationType)
               .IsRequired()
               .HasConversion<string>();                // enum ⇆ string
    }
}

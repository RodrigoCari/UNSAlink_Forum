using ForoUniversitario.DomainLayer.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForoUniversitario.InfrastructureLayer.Persistence;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Message)
               .IsRequired()
               .HasMaxLength(1000);

        builder.Property(n => n.IsRead)
               .IsRequired();

        builder.Property(n => n.CreatedAt)
               .IsRequired();

        builder.Property(n => n.ReceiverId)
               .IsRequired();

        builder.Property(n => n.NotificationType)
               .IsRequired()
               .HasConversion<string>();
    }
}

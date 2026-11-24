namespace Platform.BuildingBlocks.Infrastructure.Persistence.Outbox;

using Microsoft.EntityFrameworkCore;

public static class OutboxModelBuilderExtensions
{
    public static void ConfigureOutbox(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("outbox_message");
            entity.HasKey(message => message.Id);
            entity.Property(message => message.Id).ValueGeneratedNever();
            entity.Property(message => message.Seq).ValueGeneratedOnAdd().UseIdentityByDefaultColumn();
            entity.Property(message => message.Type).IsRequired();
            entity.Property(message => message.Payload).HasColumnType("jsonb").IsRequired();
            entity.Property(message => message.Attempts).HasDefaultValue((short)0);
            entity.HasIndex(message => new { message.ProcessedAt, message.Seq })
                .HasDatabaseName("ix_outbox_message_processed_at_seq")
                .HasFilter("processed_at IS NULL");
            entity.ToTable(table => table.HasCheckConstraint(
                "ck_outbox_message_status",
                "status BETWEEN 0 AND 2"));
        });
    }
}

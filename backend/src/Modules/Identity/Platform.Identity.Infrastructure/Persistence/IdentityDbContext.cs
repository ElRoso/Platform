namespace Platform.Identity.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Platform.BuildingBlocks.Infrastructure.Persistence;
using Platform.BuildingBlocks.Infrastructure.Persistence.Outbox;

public sealed class IdentityDbContext : PlatformDbContext
{
    public const string SchemaName = "identity";

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options, SchemaName)
    {
    }

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureOutbox();
    }
}

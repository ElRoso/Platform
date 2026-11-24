namespace Platform.BuildingBlocks.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

public abstract class PlatformDbContext : DbContext
{
    protected PlatformDbContext(DbContextOptions options, string schema)
        : base(options)
    {
        Schema = schema;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected string Schema { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var type = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;

                if (type == typeof(DateTime))
                {
                    property.SetColumnType("timestamp with time zone");
                }
                else if (type.IsEnum)
                {
                    property.SetProviderClrType(typeof(short));
                    property.SetColumnType("smallint");
                }
            }
        }
    }
}

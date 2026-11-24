namespace Platform.BuildingBlocks.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

public static class NpgsqlDbContextOptionsExtensions
{
    public static DbContextOptionsBuilder UsePlatformNpgsql(
        this DbContextOptionsBuilder options,
        string connectionString,
        string schema)
    {
        return options
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsHistoryTable("__ef_migrations_history", schema))
            .UseSnakeCaseNamingConvention();
    }

    public static DbContextOptionsBuilder<TContext> UsePlatformNpgsql<TContext>(
        this DbContextOptionsBuilder<TContext> options,
        string connectionString,
        string schema)
        where TContext : DbContext
    {
        UsePlatformNpgsql((DbContextOptionsBuilder)options, connectionString, schema);
        return options;
    }
}

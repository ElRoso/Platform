namespace Platform.BuildingBlocks.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPlatformDbContext<TContext>(
        this IServiceCollection services,
        string connectionString,
        string schema)
        where TContext : PlatformDbContext
    {
        return services.AddDbContext<TContext>(options => options.UsePlatformNpgsql(connectionString, schema));
    }
}

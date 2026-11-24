namespace Platform.Identity.Infrastructure.Persistence;

using Microsoft.Extensions.DependencyInjection;
using Platform.BuildingBlocks.Infrastructure.Persistence;

public static class IdentityPersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityPersistence(
        this IServiceCollection services,
        string connectionString)
    {
        return services.AddPlatformDbContext<IdentityDbContext>(connectionString, IdentityDbContext.SchemaName);
    }
}

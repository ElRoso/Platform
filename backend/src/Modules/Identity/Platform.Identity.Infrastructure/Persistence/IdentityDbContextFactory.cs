namespace Platform.Identity.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Platform.BuildingBlocks.Infrastructure.Persistence;

public sealed class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
            ?? throw new InvalidOperationException("Falta ConnectionStrings__Default.");

        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UsePlatformNpgsql(connectionString, IdentityDbContext.SchemaName)
            .Options;

        return new IdentityDbContext(options);
    }
}

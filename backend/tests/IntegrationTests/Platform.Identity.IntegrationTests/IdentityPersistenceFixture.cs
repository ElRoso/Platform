namespace Platform.Identity.IntegrationTests;

using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Platform.BuildingBlocks.Infrastructure.Persistence;
using Platform.Identity.Infrastructure.Persistence;
using Testcontainers.PostgreSql;
using Xunit;

public sealed class IdentityPersistenceFixture : IAsyncLifetime
{
    private const string RoleName = "platform_it";
    private const string RolePassword = "test-password";
    private static readonly Regex DatabaseNamePattern = new(@"^platform_it_\d{8}T\d{6}Z_[0-9a-f]{8}$");

    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgis/postgis:17-3.7")
        .WithDatabase("postgres")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public string ConnectionString { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var guid8 = Guid.NewGuid().ToString("N")[..8];
        var name = $"platform_it_{DateTime.UtcNow:yyyyMMddTHHmmss}Z_{guid8}";
        var adminConnectionString = _container.GetConnectionString();

        await using (var admin = new NpgsqlConnection(adminConnectionString))
        {
            await admin.OpenAsync();
            await ExecuteAsync(admin, $"CREATE ROLE {RoleName} WITH LOGIN CREATEDB PASSWORD '{RolePassword}';");
            await ExecuteAsync(admin, $"CREATE DATABASE {name} OWNER {RoleName};");
        }

        ConnectionString = new NpgsqlConnectionStringBuilder(adminConnectionString)
        {
            Username = RoleName,
            Password = RolePassword,
            Database = name,
        }.ConnectionString;

        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UsePlatformNpgsql(ConnectionString, IdentityDbContext.SchemaName)
            .Options;

        await using var db = new IdentityDbContext(options);
        await db.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        var name = new NpgsqlConnectionStringBuilder(ConnectionString).Database;

        if (!DatabaseNamePattern.IsMatch(name ?? string.Empty))
        {
            throw new InvalidOperationException($"Se rechaza el borrado de la base '{name}'.");
        }

        await using var admin = new NpgsqlConnection(_container.GetConnectionString());
        await admin.OpenAsync();
        await ExecuteAsync(admin, $"DROP DATABASE {name} WITH (FORCE);");
        await _container.DisposeAsync();
    }

    public IdentityDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UsePlatformNpgsql(ConnectionString, IdentityDbContext.SchemaName)
            .Options;

        return new IdentityDbContext(options);
    }

    private static async Task ExecuteAsync(NpgsqlConnection connection, string sql)
    {
        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }
}

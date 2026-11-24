namespace Platform.Identity.IntegrationTests;

using Microsoft.EntityFrameworkCore;
using Npgsql;
using Platform.BuildingBlocks.Infrastructure.Persistence.Outbox;
using Platform.Identity.Infrastructure.Persistence;
using Xunit;

public sealed class IdentityPersistenceTests : IClassFixture<IdentityPersistenceFixture>
{
    private readonly IdentityPersistenceFixture _fixture;

    public IdentityPersistenceTests(IdentityPersistenceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Migration_Creates_Schema_Table_And_History()
    {
        Assert.Equal("identity", await ScalarAsync("SELECT nspname FROM pg_namespace WHERE nspname = 'identity';"));
        Assert.Equal("outbox_message", await ScalarAsync("SELECT tablename FROM pg_tables WHERE schemaname = 'identity' AND tablename = 'outbox_message';"));
        Assert.Equal("__ef_migrations_history", await ScalarAsync("SELECT tablename FROM pg_tables WHERE schemaname = 'identity' AND tablename = '__ef_migrations_history';"));
        Assert.Equal("1", await ScalarAsync("SELECT count(*) FROM identity.__ef_migrations_history;"));
    }

    [Fact]
    public async Task Columns_Use_SnakeCase_And_Expected_Types()
    {
        Assert.Equal("jsonb", await ScalarAsync("SELECT data_type FROM information_schema.columns WHERE table_schema = 'identity' AND table_name = 'outbox_message' AND column_name = 'payload';"));
        Assert.Equal("smallint", await ScalarAsync("SELECT data_type FROM information_schema.columns WHERE table_schema = 'identity' AND table_name = 'outbox_message' AND column_name = 'status';"));
        Assert.Equal("timestamp with time zone", await ScalarAsync("SELECT data_type FROM information_schema.columns WHERE table_schema = 'identity' AND table_name = 'outbox_message' AND column_name = 'occurred_at';"));
    }

    [Fact]
    public async Task QueryTracking_Is_NoTracking_And_AsTracking_Is_Explicit()
    {
        await InsertAsync(OutboxMessageStatus.Pending);

        await using var db = _fixture.CreateContext();
        Assert.Equal(QueryTrackingBehavior.NoTracking, db.ChangeTracker.QueryTrackingBehavior);

        var detached = await db.OutboxMessages.FirstAsync();
        Assert.Equal(EntityState.Detached, db.Entry(detached).State);

        var tracked = await db.OutboxMessages.AsTracking().FirstAsync();
        Assert.Equal(EntityState.Unchanged, db.Entry(tracked).State);
    }

    [Fact]
    public async Task NonUtc_DateTime_Is_Rejected_And_Utc_Is_Preserved()
    {
        await using (var writeDb = _fixture.CreateContext())
        {
            writeDb.Add(NewMessage(OutboxMessageStatus.Pending) with { OccurredAt = DateTime.Now });
            await Assert.ThrowsAnyAsync<Exception>(() => writeDb.SaveChangesAsync());
        }

        var occurredAt = DateTime.UtcNow;

        await using (var writeDb = _fixture.CreateContext())
        {
            writeDb.Add(NewMessage(OutboxMessageStatus.Pending) with { OccurredAt = occurredAt });
            await writeDb.SaveChangesAsync();
        }

        await using (var readDb = _fixture.CreateContext())
        {
            var message = await readDb.OutboxMessages.OrderBy(message => message.Seq).LastAsync();
            Assert.Equal(DateTimeKind.Utc, message.OccurredAt.Kind);
        }
    }

    [Fact]
    public async Task Check_Constraint_Rejects_Invalid_Status()
    {
        await using var db = _fixture.CreateContext();
        db.Add(NewMessage((OutboxMessageStatus)3));
        await Assert.ThrowsAnyAsync<Exception>(() => db.SaveChangesAsync());
    }

    [Fact]
    public async Task Seq_Is_Identity_And_Id_Is_UuidV7()
    {
        var firstId = Guid.CreateVersion7();
        var secondId = Guid.CreateVersion7();

        await using (var writeDb = _fixture.CreateContext())
        {
            writeDb.Add(NewMessage(OutboxMessageStatus.Pending) with { Id = firstId });
            writeDb.Add(NewMessage(OutboxMessageStatus.Pending) with { Id = secondId });
            await writeDb.SaveChangesAsync();
        }

        await using var readDb = _fixture.CreateContext();
        var messages = await readDb.OutboxMessages
            .Where(message => message.Id == firstId || message.Id == secondId)
            .OrderBy(message => message.Seq)
            .ToListAsync();

        Assert.Equal(2, messages.Count);
        Assert.Equal(firstId, messages[0].Id);
        Assert.Equal(secondId, messages[1].Id);
        Assert.Equal(messages[0].Seq + 1, messages[1].Seq);
        Assert.Equal('7', firstId.ToString()[14]);
    }

    private async Task InsertAsync(OutboxMessageStatus status)
    {
        await using var db = _fixture.CreateContext();
        db.Add(NewMessage(status));
        await db.SaveChangesAsync();
    }

    private static OutboxMessage NewMessage(OutboxMessageStatus status) => new()
    {
        Id = Guid.CreateVersion7(),
        Type = "integration.test",
        Payload = "{}",
        OccurredAt = DateTime.UtcNow,
        Status = status,
    };

    private async Task<string?> ScalarAsync(string sql)
    {
        await using var connection = new NpgsqlConnection(_fixture.ConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        var value = await command.ExecuteScalarAsync();
        return value?.ToString();
    }
}

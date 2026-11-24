namespace Platform.BuildingBlocks.Infrastructure.Persistence.Outbox;

public sealed record OutboxMessage
{
    public Guid Id { get; init; }

    public long Seq { get; init; }

    public string Type { get; init; } = null!;

    public string Payload { get; init; } = null!;

    public DateTime OccurredAt { get; init; }

    public DateTime? ProcessedAt { get; init; }

    public OutboxMessageStatus Status { get; init; }

    public short Attempts { get; init; }

    public string? LastError { get; init; }
}

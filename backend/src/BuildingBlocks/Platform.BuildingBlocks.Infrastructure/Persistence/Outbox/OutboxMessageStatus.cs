namespace Platform.BuildingBlocks.Infrastructure.Persistence.Outbox;

public enum OutboxMessageStatus : short
{
    Pending = 0,
    Processed = 1,
    Failed = 2,
}

namespace MockSys.Reporting.Data.Entities;

public record SyncLog
{
    public SyncLog(DateTime transactionDateFilter)
    {
        Id = Guid.NewGuid();
        SyncTimestamp = DateTime.UtcNow;
        TransactionDateFilter = transactionDateFilter.ToUniversalTime().Date;
        Status = SyncLogStatus.InProgress;
        SyncedRecordsCount = 0;
    }

    public Guid Id { get; set; }
    public DateTime SyncTimestamp { get; set; }
    public DateTime TransactionDateFilter { get; set; }
    public SyncLogStatus Status { get; set; }
    public int SyncedRecordsCount { get; set; }
    public string? ErrorMessage { get; set; }
}

public enum SyncLogStatus
{
    Success,
    Failed,
    InProgress
}
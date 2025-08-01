using Microsoft.EntityFrameworkCore;
using MockSys.Reporting.Data.DbContexts;
using MockSys.Reporting.Data.Entities;
using MockSys.Reporting.Data.DataClients;

namespace MockSys.Reporting.Domain.Services;

public interface ITransactionSyncService
{
    private const int DEFAULT_RETRO_LIMIT = 30;
    Task SyncTransactionsAsync(DateTime transactionDateFilter);
    IAsyncEnumerable<DateTime> GetIncompleteSyncLogDatesAsAsyncEnumerable(int pastDayLimit = DEFAULT_RETRO_LIMIT);
    IAsyncEnumerable<SyncLog> GetFailedSyncLogsAsAsyncEnumerable(int pastDayLimit = DEFAULT_RETRO_LIMIT);
}

public class TransactionSyncService(IFakeDataApiClient fakeDataApiClient, ReportingDbContext reportingDbContext) : ITransactionSyncService
{
    private const int DEFAULT_RETRO_LIMIT = 30;

    public async Task SyncTransactionsAsync(DateTime transactionDateFilter)
    {
        // Ensure the date has not been processed yet
        if (await IsTransactionDateSyncedAsync(transactionDateFilter))
        {
            return; // No need to sync again
        }

        try
        {
            // Attempt to sync transactions from the fake data provider, excluding existing records
            var existingTransactionIds = await GetExistingSyncTransactionIdsAsync(transactionDateFilter);
            await SyncTransactionsNoFailsafeAsync(transactionDateFilter, existingTransactionIds);
        }
        catch (Exception ex)
        {
            await RecordFailedSync(transactionDateFilter, ex);
        }
    }

    public async IAsyncEnumerable<DateTime> GetIncompleteSyncLogDatesAsAsyncEnumerable(int pastDayLimit = DEFAULT_RETRO_LIMIT)
    {
        // Retrieve the dates of transaction syncs that have been completed as a HashSet for O(1) lookup
        var limitedPastSyncLogDates = await reportingDbContext.SyncLogs
            .AsNoTracking()
            .Where(log => log.TransactionDateFilter >= DateTime.UtcNow.AddDays(-pastDayLimit))
            .GroupBy(log => log.TransactionDateFilter)
            .Where(group => group.Any(log => log.Status == SyncLogStatus.Success))
            .Select(group => group.Key)
            .ToHashSetAsync();

        // Today (UTC) is not considered a missing sync date
        for (int i = pastDayLimit; i > 0; i--)
        {
            var syncLogDate = DateTime.UtcNow.AddDays(-i).Date;

            if (!limitedPastSyncLogDates.Contains(syncLogDate))
            {
                yield return syncLogDate;
            }
        }
    }

    public async IAsyncEnumerable<SyncLog> GetFailedSyncLogsAsAsyncEnumerable(int pastDayLimit = DEFAULT_RETRO_LIMIT)
    {
        await foreach (var log in reportingDbContext.SyncLogs
            .AsNoTracking()
            .Where(log => log.TransactionDateFilter >= DateTime.UtcNow.AddDays(-pastDayLimit) && log.Status == SyncLogStatus.Failed)
            .AsAsyncEnumerable())
        {
            yield return log;
        }
    }

    private async Task<bool> IsTransactionDateSyncedAsync(DateTime transactionDate)
    {
        var cleanDate = transactionDate.ToUniversalTime().Date;
        var existingSuccessSyncLog = await reportingDbContext.SyncLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(log => log.TransactionDateFilter == cleanDate && log.Status == SyncLogStatus.Success);

        return existingSuccessSyncLog != null;
    }

    private async Task SyncTransactionsNoFailsafeAsync(DateTime transactionDateFilter, HashSet<Guid>? existingSyncTransactionIds = null)
    {
        // Create a new pending sync log record
        var newSyncLog = new SyncLog(transactionDateFilter);
        reportingDbContext.SyncLogs.Add(newSyncLog);

        // Retrieve and sync new sales transactions from the fake data provider
        await foreach (var salesTransaction in fakeDataApiClient.GetSyncTransactionsAsAsyncEnumerable(transactionDateFilter))
        {
            if (existingSyncTransactionIds == null || !existingSyncTransactionIds.Contains(salesTransaction.Id))
            {
                reportingDbContext.SyncTransactions.Add(salesTransaction.ToSyncTransaction());
                ++newSyncLog.SyncedRecordsCount;
            }
        }

        // Update the sync log record with a success status if the date has passed
        if (transactionDateFilter.ToUniversalTime().Date != DateTime.UtcNow.Date)
        {
            newSyncLog.Status = SyncLogStatus.Success;
        }

        // Save changes to the database and commit the transaction
        await reportingDbContext.SaveChangesAsync();
    }

    private async Task RecordFailedSync(DateTime transactionDateFilter, Exception ex)
    {
        // record a failed sync log record
        reportingDbContext.SyncLogs.Add(
            new SyncLog(transactionDateFilter)
            {
                Status = SyncLogStatus.Failed,
                ErrorMessage = ex.Message
            });

        // Save changes to the database
        await reportingDbContext.SaveChangesAsync();
    }

    private async Task<HashSet<Guid>> GetExistingSyncTransactionIdsAsync(DateTime transactionDateFilter) =>
        await reportingDbContext.SyncTransactions
           .AsNoTracking()
           .Where(transaction => transaction.Timestamp.ToUniversalTime().Date == transactionDateFilter.ToUniversalTime().Date)
           .Select(transaction => transaction.Id)
           .ToHashSetAsync();
}

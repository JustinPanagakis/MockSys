using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MockSys.Reporting.Domain.Events;
using MockSys.Reporting.Domain.Services;

namespace MockSys.Reporting.DataSync.Functions;

public class PastSyncDispatchFunction(ILogger<PastSyncDispatchFunction> logger, ITransactionSyncService transactionSyncService, ServiceBusClient serviceBusClient)
{
    // This function will be triggered by a timer every minute.
    [Function(nameof(PastSyncDispatchFunction))]
    public async Task Run([TimerTrigger("0 * * * * *", RunOnStartup = true)] TimerInfo timerInfo)
    {
        logger.LogInformation("Creating past patch sync events on: {Date}", DateTime.Now);
        
        await using var sender = serviceBusClient.CreateSender("sync-queue");

        await foreach (var syncDate in transactionSyncService.GetIncompleteSyncLogDatesAsAsyncEnumerable())
        {
            var message = new SyncEvent() { SyncDate = syncDate };

            logger.LogInformation("Creating past patch sync event for: {Message}", message.SyncDate);

            var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(message))
            {
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "application/json"
            };

            await sender.SendMessageAsync(serviceBusMessage);
        }

        if (timerInfo.ScheduleStatus is not null)
        {
            logger.LogInformation("Next timer schedule at: {Next}", timerInfo.ScheduleStatus.Next);
        }
    }
}

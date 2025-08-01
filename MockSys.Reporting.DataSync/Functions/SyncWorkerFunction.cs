using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MockSys.Reporting.Domain.Events;
using MockSys.Reporting.Domain.Services;

namespace MockSys.Reporting.DataSync.Functions;

public class SyncWorkerFunction(ILogger<SyncWorkerFunction> logger, ITransactionSyncService transactionSyncService, ServiceBusClient serviceBusClient)
{
    [Function(nameof(SyncWorkerFunction))]
    public async Task Run(
        [ServiceBusTrigger("sync-queue", Connection = "mocksys-servicebus")]
        ServiceBusReceivedMessage message)
    {
        var syncEvent = JsonSerializer.Deserialize<SyncEvent>(message.Body);

        if (syncEvent == null)
        {
            logger.LogError("Invalid sync message received: {MessageId}", message.MessageId);
            return;
        }

        logger.LogInformation("Syncing Message {id} for: {SyncDate}", message.MessageId, syncEvent.SyncDate);

        await transactionSyncService.SyncTransactionsAsync(syncEvent.SyncDate);

        logger.LogInformation("Sync Message {id} for: {SyncDate} Complete. Sending report event for {ReportDate}.", message.MessageId, syncEvent.SyncDate, syncEvent.SyncDate);

        await using var sender = serviceBusClient.CreateSender("reports-queue");

        var reportEvent = new ReportEvent() { ReportDate = syncEvent.SyncDate };
        var reportMessage = new ServiceBusMessage(JsonSerializer.Serialize(reportEvent));

        logger.LogInformation("Sending report event for {ReportDate}.", reportEvent.ReportDate);
        await sender.SendMessageAsync(reportMessage);
    }
}

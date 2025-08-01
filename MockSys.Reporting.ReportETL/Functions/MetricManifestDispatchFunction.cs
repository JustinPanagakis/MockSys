using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MockSys.Reporting.Domain.Events;
using MockSys.Reporting.Domain.MetricRunners;

namespace MockSys.Reporting.ReportETL.Functions;

public class MetricManifestDispatchFunction(ILogger<MetricManifestDispatchFunction> logger, MetricRunnerManifest metricRunnerManifest, ServiceBusClient serviceBusClient)
{
    [Function(nameof(MetricManifestDispatchFunction))]
    public async Task Run(
        [ServiceBusTrigger("reports-queue", Connection = "mocksys-servicebus")]
        ServiceBusReceivedMessage message)
    {
        var reportEvent = JsonSerializer.Deserialize<ReportEvent>(message.Body);

        if (reportEvent == null)
        {
            logger.LogError("Invalid report message received: {messageId}", message.MessageId);
            return;
        }

        logger.LogInformation("Running report metrics for {Date}", reportEvent.ReportDate);

        await using var sender = serviceBusClient.CreateSender("metrics-queue");

        foreach (var metricName in metricRunnerManifest.Metrics)
        {
            var metricEvent = new MetricEvent() { Date = reportEvent.ReportDate, MetricName = metricName };
            var metricMessage = new ServiceBusMessage(JsonSerializer.Serialize(metricEvent));

            logger.LogInformation("Sending metric {MetricName} for date {Date}", metricName, reportEvent.ReportDate);
            await sender.SendMessageAsync(metricMessage);
        }
    }
}
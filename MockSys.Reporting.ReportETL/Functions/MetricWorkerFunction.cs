using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MockSys.Reporting.Domain.Events;
using MockSys.Reporting.Domain.Factories;
using MockSys.Reporting.Domain.Services;

namespace MockSys.Reporting.ReportETL.Functions;

public class MetricWorkerFunction(ILogger<MetricWorkerFunction> logger, IMetricRunnerFactory metricRunnerFactory, IMetricStorageService metricStorageService)
{
    [Function(nameof(MetricWorkerFunction))]
    public async Task Run(
        [ServiceBusTrigger("metrics-queue", Connection = "mocksys-servicebus")]
        ServiceBusReceivedMessage message)
    {
        var metricEvent = JsonSerializer.Deserialize<MetricEvent>(message.Body);

        if (metricEvent == null)
        {
            logger.LogError("Invalid sync message received: {messageId}", message.MessageId);
            return;
        }

        logger.LogInformation("Processing metric {Metric} for {Date}", metricEvent.MetricName, metricEvent.Date);

        var runner = metricRunnerFactory.GetRunner(metricEvent.MetricName);
       
        var result = await runner.RunAsync(metricEvent.Date);
        await metricStorageService.StoreMetricResultAsync(result, metricEvent.Date);

        logger.LogInformation("Metric {Metric} for {Date} Complete", metricEvent.MetricName, metricEvent.Date);
    }
}

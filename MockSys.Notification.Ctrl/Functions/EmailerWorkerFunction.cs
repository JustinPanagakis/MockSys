using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MockSys.Notification.Ctrl.Functions
{
    public class EmailerWorkerFunction
    {
        private readonly ILogger<EmailerWorkerFunction> _logger;

        public EmailerWorkerFunction(ILogger<EmailerWorkerFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(EmailerWorkerFunction))]
        public async Task Run(
            [ServiceBusTrigger("email-queue", Connection = "mocksys-servicebus")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}

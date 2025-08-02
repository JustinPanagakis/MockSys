using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MockSys.Notification.Ctrl.Dtos;
using MockSys.Notification.Domain.Services.Contracts;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace MockSys.Notification.Ctrl.Functions
{
    public class EmailerWebFunction
    {
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;

        public EmailerWebFunction(ILoggerFactory loggerFactory, IEmailService emailService)
        {
            _logger = loggerFactory.CreateLogger<EmailerWebFunction>();
            _emailService = emailService;
        }

        [Function("EmailerWebFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "send-email")]
            HttpRequestData req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            SendEmailRequest? request;
            try
            {
                request = JsonSerializer.Deserialize<SendEmailRequest>(requestBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid JSON format.");
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteStringAsync("Invalid JSON.");
                return bad;
            }

            if (request is null || string.IsNullOrWhiteSpace(request.To))
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                await bad.WriteStringAsync("Missing 'to' field.");
                return bad;
            }

            _logger.LogInformation("Sending email to {To}", request.To);
            await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Email sent.");
            return response;
        }
    }
}

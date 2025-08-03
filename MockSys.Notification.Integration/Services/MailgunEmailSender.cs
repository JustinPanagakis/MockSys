using MockSys.Notification.Integration.Services.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSys.Notification.Integration.Services
{
    public class MailgunEmailSender : IEmailSender
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _domain;

        public MailgunEmailSender(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["Mailgun:ApiKey"];
            _domain = config["Mailgun:Domain"];
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"https://api.mailgun.net/v3/{_domain}/messages");

            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("from", $"Mailgun Sandbox <postmaster@{_domain}>"),
            new KeyValuePair<string, string>("to", to),
            new KeyValuePair<string, string>("subject", subject),
            new KeyValuePair<string, string>("text", body)
        });

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{_apiKey}")));
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}

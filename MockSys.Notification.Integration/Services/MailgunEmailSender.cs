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

        //public static async Task<RestResponse> Send()
        //{
        //    var options = new RestClientOptions("https://api.mailgun.net")
        //    {
        //        Authenticator = new HttpBasicAuthenticator("api", Environment.GetEnvironmentVariable("API_KEY") ?? "API_KEY")
        //    };

        //    var client = new RestClient(options);
        //    var request = new RestRequest("/v3/sandbox4608fb1dde05410d91ec77dea0af61ad.mailgun.org/messages", Method.Post);
        //    request.AlwaysMultipartFormData = true;
        //    request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox4608fb1dde05410d91ec77dea0af61ad.mailgun.org>");
        //    request.AddParameter("to", "Jack Giannini <jack.giannini@gmail.com>");
        //    request.AddParameter("subject", "Hello Jack Giannini");
        //    request.AddParameter("text", "Congratulations Jack Giannini, you just sent an email with Mailgun! You are truly awesome!");
        //    return await client.ExecuteAsync(request);
        //}
    }
}

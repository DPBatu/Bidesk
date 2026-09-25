using Bidesk.Application.Abstraction.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Bidesk.Infrastructure.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WhatsAppService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendMessageAsync(string phoneNumber, string message)
        {
            // Test modu: appsettings -> Notifications:Enabled / Notifications:WhatsApp kapaliysa gonderim yapilmaz.
            if (!NotificationGate.IsChannelEnabled(_configuration, "WhatsApp"))
                return;

            var url = $"https://graph.facebook.com/v21.0/{_configuration["WhatsApp:PhoneId"]}/messages";

            var payload = new
            {
                messaging_product = "whatsapp",
                to = phoneNumber,
                type = "template",
                template = new
                {
                    name = "hello_world",
                    language = new { code = "en_US" }
                }
            };

            //var payload = new
            //{
            //    messaging_product = "whatsapp",
            //    to = phoneNumber,
            //    type = "document", // Eğer resim göndereceksen "image", video göndereceksen "video" yap
            //    document = new
            //    {
            //        link = documentUrl, // PDF, resim veya video linki buraya gelecek
            //        caption = "hello_world.", // İsteğe bağlı açıklama
            //        filename = fileName // Dosyanın adı (örn: "Rapor.pdf")
            //    }
            //};

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["WhatsApp:AccessToken"]);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"WhatsApp API error: {responseContent}");
            }
        }
    }
}

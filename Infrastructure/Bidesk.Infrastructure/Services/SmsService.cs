using Bidesk.Application.Abstraction.Services;
using Bidesk.Application.Features;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace Bidesk.Infrastructure.Services
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseRes> SendSmsAsync(string phoneNumber, string message)
        {
            // Test modu: appsettings -> Notifications:Enabled / Notifications:Sms kapaliysa gonderim yapilmaz.
            if (!NotificationGate.IsChannelEnabled(_configuration, "Sms"))
            {
                await Task.CompletedTask;
                return new BaseRes { Message = "Bildirimler kapalı olduğundan SMS gönderilmedi.", IsSuccess = true };
            }

#if DEBUG
            // DEBUG modunda gerçek SMS gönderimi yapılmaz.
            await Task.CompletedTask;
            return new BaseRes { Message = "DEBUG modunda SMS gönderilmedi.", IsSuccess = true };
#else
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    return new BaseRes { Message = "Hata", ErrorMessage = "Telefon numarası boş olamaz.", IsSuccess = false };

                if (string.IsNullOrWhiteSpace(message))
                    return new BaseRes { Message = "Hata", ErrorMessage = "Mesaj içeriği boş olamaz.", IsSuccess = false };

                var user = _configuration["BideskSMS:User"];
                var pass = _configuration["BideskSMS:Password"];

                if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
                    return new BaseRes { Message = "Hata", ErrorMessage = "SMS kullanıcı adı veya şifre eksik.", IsSuccess = false };

                string formattedPhoneNumber = FormatPhoneNumber(phoneNumber);

                string SanitizeForXml(string input)
                {
                    return string.IsNullOrEmpty(input) ? string.Empty : System.Security.SecurityElement.Escape(input);
                }

                string xmlContent = $@"
                                    <MainmsgBody>
                                      <Command>0</Command>
                                      <PlatformID>1</PlatformID>
                                      <ChannelCode>618</ChannelCode>
                                      <UserName>{user}</UserName>
                                      <PassWord>{pass}</PassWord>
                                      <Mesgbody>{SanitizeForXml(message)}</Mesgbody>
                                      <Numbers>{formattedPhoneNumber}</Numbers>
                                      <Type>1</Type>
                                      <Originator>DOGU PRES</Originator>
                                    </MainmsgBody>";


                using var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://processor.smsorigin.com/xml/process.aspx")
                {
                    Content = new StringContent(xmlContent, Encoding.UTF8, "application/xml")
                };

                request.Headers.Add("Cookie", "ASP.NET_SessionId=mcgqpfofepyivfmuqjwnnbzl");

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                var resultCode = responseContent.Trim();

                if (!response.IsSuccessStatusCode)
                {
                    return new BaseRes
                    {
                        Message = "HTTP Hatası",
                        ErrorMessage = $"Durum Kodu: {response.StatusCode}, İçerik: {responseContent}",
                        IsSuccess = false
                    };
                }

                if (!resultCode.StartsWith("ID", StringComparison.OrdinalIgnoreCase))
                {
                    string errorDetail = resultCode switch
                    {
                        "1" => "Eksik veya hatalı XML alanı.",
                        "2" => "Kullanıcı adı veya şifre hatalı.",
                        "3" => "Mesaj metni boş.",
                        "4" => "Mesaj alıcı numarası eksik.",
                        "5" => "Gönderici başlığı geçersiz.",
                        "20" => "Tanımsız hata. XML yapısını ve Originator'ı kontrol edin.",
                        "21" => "Kanal kodu geçersiz veya kullanıcı kanal koduna yetkili değil.",
                        "22" => "Yetersiz kredi.",
                        _ => $"Bilinmeyen hata kodu: {resultCode}"
                    };

                    return new BaseRes
                    {
                        Message = "SMS gönderilemedi",
                        ErrorMessage = $"Hata Kodu: {resultCode} - {errorDetail}",
                        IsSuccess = false
                    };
                }

                return new BaseRes
                {
                    Message = "SMS başarıyla gönderildi",
                    IsSuccess = true
                };
            }
            catch (ArgumentException ex)
            {
                return new BaseRes { Message = "Geçersiz bilgi", ErrorMessage = ex.Message, IsSuccess = false };
            }
            catch (Exception ex)
            {
                return new BaseRes
                {
                    Message = "Beklenmeyen hata",
                    ErrorMessage = $"{ex.Message}\n{ex.StackTrace}",
                    IsSuccess = false
                };
            }
#endif
        }
        private static string FormatPhoneNumber(string phoneNumber)
        {
            // Boşlukları ve tireleri kaldır
            phoneNumber = Regex.Replace(phoneNumber, @"[^\d]", "");

            // Eğer numara +90 ile başlıyorsa, + işaretini kaldır
            if (phoneNumber.StartsWith("90"))
                phoneNumber = phoneNumber.Remove(0, 2);

            // Eğer numara 0 ile başlıyorsa, 0'ı kaldır
            if (phoneNumber.StartsWith("0"))
                phoneNumber = phoneNumber.Remove(0, 1);

            // Eğer numara 10 haneli değilse, hata döndür
            if (phoneNumber.Length != 10)
                throw new ArgumentException("Geçersiz telefon numarası formatı.", new Exception { });

            // 90 ile başlayarak yeni formata çevir
            return "90" + phoneNumber;
        }
    }
}

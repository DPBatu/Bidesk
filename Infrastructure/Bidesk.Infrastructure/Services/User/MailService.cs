using Bidesk.Application.Abstraction.Services.User;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Bidesk.Infrastructure.Services.User
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
        #if (DEBUG)
                    to = _configuration["BideskErrMail:MailAddr"].ToString();
        #elif (RELEASE)
                //to = "sabireozkurt@dogupres.com";
        #endif


            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            // Test modu: appsettings -> Notifications:Enabled / Notifications:Mail kapaliysa gonderim yapilmaz.
            if (!NotificationGate.IsChannelEnabled(_configuration, "Mail"))
                return;

            string senderEmail = _configuration["Mail:Username"];
            //string senderPassword = "your-email-password";
            string smtpServer = _configuration["Mail:Host"];
            int smtpPort = Convert.ToInt32(_configuration["Mail:Port"]);

            using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
            {
                client.EnableSsl = false;
                client.Credentials = new NetworkCredential(senderEmail, _configuration["Mail:Password"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(senderEmail);
#if (DEBUG)
                                mailMessage.To.Add(_configuration["BideskErrMail:MailAddr"].ToString());
#elif (RELEASE)
                // Notifications:TestRecipient doluysa tum mailler o adrese yonlendirilir.
                foreach (var to in NotificationGate.ResolveRecipients(_configuration, tos))
                    mailMessage.To.Add(to);
#endif

                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;

                await client.SendMailAsync(mailMessage);
            }
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            StringBuilder mail = new();
            mail.AppendLine("Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br><strong><a target=\"_blank\" href=\"");
            mail.AppendLine(_configuration["AngularClientUrl"]);
            mail.AppendLine("/update-password/");
            mail.AppendLine(userId);
            mail.AppendLine("/");
            mail.AppendLine(resetToken);
            mail.AppendLine("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT : Eğer ki bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla...<br><br><br>DPPortal");

            await SendMailAsync(to, "Şifre Yenileme Talebi", mail.ToString());

        }
    }
}

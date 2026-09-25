using Bidesk.Application.Abstraction.Services;
using Bidesk.Application.Abstraction.Services.User;
using Bidesk.Infrastructure.Services;
using Bidesk.Infrastructure.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace Bidesk.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IMailService, MailService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
            serviceCollection.AddScoped<ISmsService, SmsService>();
            serviceCollection.AddScoped<IWhatsAppService, WhatsAppService>();
            //serviceCollection.AddHttpClient<IWhatsAppService, WhatsAppService>();
        }
    }
}

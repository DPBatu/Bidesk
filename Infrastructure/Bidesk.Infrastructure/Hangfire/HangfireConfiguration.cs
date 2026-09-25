using Microsoft.Extensions.Configuration;

namespace Bidesk.Infrastructure.Hangfire
{
    static class HangfireConfiguration
    {
        public static string ConnectionStringDbHangfire
        {
            get
            {
#if (DEBUG)
                return ConnectionStringDev;
#elif (RELEASE)
                return ConnectionStringProd;
#endif
            }
        }

        public static string ConnectionStringDev
        {
            get
            {
                ConfigurationManager configurationManager = new();
                configurationManager.SetBasePath(Directory.GetCurrentDirectory());
                configurationManager.AddJsonFile("appsettings.json");
                return configurationManager.GetConnectionString("DEV");
            }
        }

        public static string ConnectionStringProd
        {
            get
            {
                ConfigurationManager configurationManager = new();
                configurationManager.SetBasePath(Directory.GetCurrentDirectory());
                configurationManager.AddJsonFile("appsettings.json");
                return configurationManager.GetConnectionString("PROD");
            }
        }
    }
}

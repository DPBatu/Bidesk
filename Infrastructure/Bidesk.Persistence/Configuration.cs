using Bidesk.Application.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Bidesk.Persistence
{
    public static class Configuration
    {
        public static string DevConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Bidesk.API"));
                configurationManager.AddJsonFile("appsettings.json");
                string? config = configurationManager.GetConnectionString("DEV");

                if (string.IsNullOrEmpty(config)) throw new AppConfigInfoMissingException("Database bağlantı bilgileri alınamadı!");

                return config;
            }
        }

        public static string ProdConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new();
                configurationManager.SetBasePath(Directory.GetCurrentDirectory());
                configurationManager.AddJsonFile("appsettings.json");
                string? config = configurationManager.GetConnectionString("PROD");

                if (string.IsNullOrEmpty(config)) throw new AppConfigInfoMissingException("Database bağlantı bilgileri alınamadı!");

                return config;
            }
        }
    }
}

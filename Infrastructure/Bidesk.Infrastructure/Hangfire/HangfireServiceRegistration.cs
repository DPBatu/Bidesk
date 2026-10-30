using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;

namespace Bidesk.Infrastructure.Hangfire
{
    public static class HangfireServiceRegistration
    {
        public static void AddHangfireServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(server => server.UseNpgsqlConnection(HangfireConfiguration.ConnectionStringDbHangfire), new PostgreSqlStorageOptions { SchemaName = "HangFireBidesk" })
            );

            serviceCollection.AddHangfireServer(config =>
            {
                config.ServerName = $"{Environment.MachineName}.KAIZEN-API.{Guid.NewGuid()}";
            });
        }
    }
}

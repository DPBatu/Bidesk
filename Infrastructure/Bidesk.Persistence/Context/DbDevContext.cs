using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bidesk.Persistence.Context
{
    public class DbDevContext : BaseContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql(Configuration.DevConnectionString, options => options.MigrationsHistoryTable("_EFMigrationsHistory", "Bidesk"))
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
            

        public DbDevContext(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}

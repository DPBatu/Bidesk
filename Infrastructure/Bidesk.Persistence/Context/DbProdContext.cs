
using Bidesk.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bidesk.Persistence.Context
{
    public class DbProdContext : BaseContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql(Configuration.ProdConnectionString, options => options.MigrationsHistoryTable("_EFMigrationsHistory", "Bidesk"));

        public DbProdContext(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}

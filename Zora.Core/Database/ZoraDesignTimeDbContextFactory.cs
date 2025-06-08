using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Zora.Core.Database;

internal class ZoraDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ZoraDbContext>
{
    public ZoraDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ZoraDbContext>();
        optionsBuilder.UseSqlServer(
            connectionString: "Server=localhost\\MSSQLSERVER02;Database=ZoraDB;User Id=sa3;Password=P@ssw0rd;TrustServerCertificate=True;",
            x => x.MigrationsHistoryTable("__ZoraMigrations", ZoraDbContext.SchemaName)
        );

        return new ZoraDbContext(optionsBuilder.Options);
    }
}

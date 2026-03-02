using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace xFood.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<xFoodDbContext>
    {
        public xFoodDbContext CreateDbContext(string[] args)
        {
            // Usa LocalDB igual ao appsettings
            var conn = "Host=ep-autumn-dawn-acwjz8kg-pooler.sa-east-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_6haKkwMy2dzE;SSL Mode=Require;Trust Server Certificate=true";

            var opts = new DbContextOptionsBuilder<xFoodDbContext>()
                .UseNpgsql(conn)
                .Options;

            return new xFoodDbContext(opts);
        }
    }
}

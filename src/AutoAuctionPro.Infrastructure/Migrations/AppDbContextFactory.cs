using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AutoAuctionPro.Infrastructure.Migrations
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            Env.Load(".env");

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            // Ler variáveis do .env
            var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var pass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

            var connString = $"Host={host};Port={port};Database={dbName};Username={user};Password={pass}";

            builder.UseNpgsql(connString);

            return new AppDbContext(builder.Options);
        }
    }
}

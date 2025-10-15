using AutoAuctionPro.Infrastructure;
using DotNetEnv;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AutoAuctionPro.Tests
{
    public static class DbConnection
    {
        public static AppDbContext CreateInMemoryDb()
        {
            SQLitePCL.Batteries.Init();

            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(conn).Options;
            var db = new AppDbContext(options);

            db.Database.EnsureCreated();

            return db;
        }

        public static AppDbContext CreatePostgresDb()
        {
            Env.Load(".env");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(GetConnectionString())
                .Options;

            var db = new AppDbContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            return db;
        }

        private static string GetConnectionString()
        {
            var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            var db = Environment.GetEnvironmentVariable("POSTGRES_DB");
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var pass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

            var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={pass}";

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Please set the environment variable DATABASE_CONNECTION");

            return connectionString;
        }
    }
}

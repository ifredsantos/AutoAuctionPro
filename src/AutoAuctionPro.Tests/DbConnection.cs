using AutoAuctionPro.Infrastructure;
using DotNetEnv;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            return db;
        }

        private static string GetConnectionString()
        {
            var conn = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");

            if (string.IsNullOrEmpty(conn))
                throw new InvalidOperationException("Please set the environment variable DATABASE_CONNECTION");

            return conn;
        }
    }
}

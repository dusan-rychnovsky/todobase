using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using todobase.Models;
using Xunit;

namespace todobase.integrationtests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var dbConnection = new SqliteConnection("DataSource=:memory:");
            dbConnection.Open();
            Startup.DbConnection = dbConnection;

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(dbConnection)
                .Options;

            using (var dbContext = new AppDbContext(dbOptions))
            {
                await dbContext.Database.MigrateAsync();
                await dbContext.Todos.AddRangeAsync(
                    new[]
                    {
                        new Todo { TodoId = 1L, Label = "Do the vacuuming." },
                        new Todo { TodoId = 2L, Label = "Feed the cat." }
                    });
                await dbContext.SaveChangesAsync();
            }

            var builder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>();

            var server = new TestServer(builder);
            var client = server.CreateClient();

            var response = await client.GetAsync("/");
            var content = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            Assert.Contains("Todos:", content);
            Assert.Contains("Do the vacuuming.", content);
            Assert.Contains("Feed the cat.", content);
        }
    }
}

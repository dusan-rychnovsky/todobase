using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Threading.Tasks;
using Xunit;

namespace todobase.integrationtests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>();

            var server = new TestServer(builder);
            var client = server.CreateClient();

            var response = await client.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Todos:", content);
        }
    }
}

using Grace.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Uber.Server.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseGrace()
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((host, config) => config.AddJsonFile("AppSettings.User.json", optional: true, reloadOnChange: true))
                .Build();
    }
}

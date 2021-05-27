using CoreWebApi.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            using IHost host = CreateHostBuilder(args).Build();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(builder =>
                builder.ClearProviders()
                    .AddLogWatcher(configuration =>
                    {
                        configuration.Initialise();
                    }))
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
    }
}

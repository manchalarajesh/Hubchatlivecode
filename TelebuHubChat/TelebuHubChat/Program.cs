using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TelebuHubChat.LogClasses;

namespace TelebuHubChat
{
    public class Program
    {
	[Obsolete]
        public static void Main(string[] args)
        {
	    ConfigureLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
		    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var isDevelopment = environment == Microsoft.AspNetCore.Hosting.EnvironmentName.Development;
                    if (!isDevelopment)
                    {
		          webBuilder.UseKestrel();
   		          webBuilder.UseUrls("http://172.31.16.142:60010");
                    }
                    webBuilder.UseStartup<Startup>();
                });

	[Obsolete]
        public static void ConfigureLogger()
        {
            var configuration = new
                ConfigurationBuilder()
                       .AddJsonFile("appsettings.json")
                       .Build();

            var errorSection = configuration.GetSection("ErrorLog");
            var auditSection = configuration.GetSection("AuditLog");
            LogProperties.errorLogger = new LoggerConfiguration()
                .Enrich.With(new EnrichProp())
                .ReadFrom.ConfigurationSection(errorSection)
                .CreateLogger();

            LogProperties.infoLogger = new LoggerConfiguration()
                .Enrich.With(new EnrichProp())
                .ReadFrom.ConfigurationSection(auditSection)
                .CreateLogger();
        }
    }
}

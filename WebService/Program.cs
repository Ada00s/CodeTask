using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using WebService.Handlers;
using WebService.Models;

namespace WebService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config
						.SetBasePath(Directory.GetCurrentDirectory())
						.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
						.AddEnvironmentVariables()
						.AddCommandLine(args);
				})
				.ConfigureServices((hostContext, services) =>
				{
					services
						.AddOptions()
						.Configure<ServiceConfig>(hostContext.Configuration.GetSection("ServiceConfig"));
					var config = new ServiceConfig();
					hostContext.Configuration.GetSection("ServiceConfig").Bind(config);
					services
						.AddSingleton<SqsHandler>(sh => new SqsHandler(config))
						.AddSingleton<DbHandler>(dh => new DbHandler(config));
				})
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
	}
}

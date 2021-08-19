using System;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FlcIO.App
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IConfigurationRoot configuration = GetConfiguration();
			ConfiguraLog(configuration);

			try
			{
				Log.Information("Iniciando o projeto Web Messages!");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Erro de execução!");
			}
			finally
			{
				Log.CloseAndFlush();
			}

		}

		private static void ConfiguraLog(IConfigurationRoot configuration)
		{
			Log.Logger = new LoggerConfiguration()
							.ReadFrom.Configuration(configuration)
							.CreateLogger();
		}

		private static IConfigurationRoot GetConfiguration()
		{
			string ambiente = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{ambiente}.json")
				.Build();
			return configuration;
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}

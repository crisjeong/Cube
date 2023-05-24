using Cube.Socket.Server.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreServer;
using Serilog;
using System.Net;

namespace Cube.Socket.Server;


public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder();

        BuildConfig(builder);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .Destructure.ToMaximumCollectionCount(10)
            .CreateLogger();

        await CreateHostBuilder(args)
            .Build()
            .RunAsync();
        
        await Log.CloseAndFlushAsync();
    }

    public static void BuildConfig(IConfigurationBuilder builder) =>
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                //TODO
                services.AddHostedService<Worker>();
                services.AddSingleton(new ChatServer(IPAddress.Any, 1111));

            })
            .UseConsoleLifetime()
            .UseSerilog();
}